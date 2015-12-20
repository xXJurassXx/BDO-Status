using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Models.Character;
using Commons.UID;
using Commons.Utils;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models.MySql.Mapping.WorldMap;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Processors
{
    public class LobbyProcessor : IProcessor
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Game server dao factory
        /// </summary>
        private static ISessionFactory _gsDbFactory;

        /// <summary>
        /// Character uids generator
        /// </summary>
        private static UInt32UidFactory _characterUidsFactory;

        /// <summary>
        /// Initilize service action
        /// </summary>
        /// <param name="previousInstanceContext"></param>
        public void OnLoad(object previousInstanceContext)
        {
            var config = Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(cs => cs.Is(
                    $"Server={CfgDatabase.Default.GameServerDaoHost};" +
                    $"Database={CfgDatabase.Default.GameServerDaoDatabase};" +
                    $"User={CfgDatabase.Default.GameServerDaoUsername}" +
                    $";Password={CfgDatabase.Default.GameServerDaoPassword};" +
                    "CharSet=utf8"))).Mappings(m =>
                    {
                        m.FluentMappings.AddFromNamespaceOf<CharacterMap>();
                    });
            
            var export = new SchemaUpdate(config.BuildConfiguration());

            export.Execute(false, true);

            _gsDbFactory = config.BuildSessionFactory();

            var usedIds =  //Get all used character id`s
                _gsDbFactory.OpenSession().CreateSQLQuery("select `c_character_id` from `bd_characters`").List();

            _characterUidsFactory = usedIds.Any() //Configure id factory
                ? new UInt32UidFactory((uint) usedIds[usedIds.Count - 1]) : new UInt32UidFactory();

        }

        public void GetCharacterList(ClientConnection connection)
        {
            using (var db = _gsDbFactory.OpenSession())
            {
                var list = db.QueryOver<CharacterData>().Where(p => p.AccountId == connection.Account.Id).List();
                if(list != null)
                    connection.Characters = (List<CharacterData>) list;
                else
                    connection.Characters = new List<CharacterData>();
                
                new SpCharacterList(connection.Account, connection.Characters).Send(connection);
            }           
        }

        public void CreateCharacterProcess(ClientConnection connection, CharacterData info)
        {
            var characterData = info;

            using (var db = _gsDbFactory.OpenSession())
            {
                if (db.QueryOver<CharacterData>().Where(s => s.CharacterName == info.CharacterName).Take(1).SingleOrDefault() != null)
                {
                    new SpCreateCharacterError().Send(connection);

                    return;
                }
                   
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        characterData.CharacterId = _characterUidsFactory.Next();
                        characterData.Surname = connection.Account.FamilyName;
                        characterData.Level = 1;
                        characterData.CreationDate = DateTime.Now;

                        db.Save(characterData);

                        connection.Characters.Add(characterData);

                        new SpCreateCharacter(characterData).Send(connection, false);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        Log.Error($"Cannot create character\n{ex}");
                    }                   
                }
            }          
        }

        public void DeleteCharacterProcess(ClientConnection connection, long characterId)
        {
            using (var db = _gsDbFactory.OpenSession())
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var deletionTime = (int)(DateTime.Now.UnixMilliseconds() / 1000);

                        db.Delete(connection.Characters.First(s => s.CharacterId == characterId));

                        new SpDeleteCharacter(characterId, 1, deletionTime).Send(connection);

                        transaction.Commit();
                    }
                    catch 
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public object OnUnload()
        {
            return null;
        }
    }
}
