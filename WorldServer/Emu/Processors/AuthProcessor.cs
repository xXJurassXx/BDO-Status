using System;
using Commons.Models.Account;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models.MySql.Mapping.LoginMap;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Processors
{
    public class AuthProcessor : IProcessor
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Login server dao factory
        /// </summary>
        private static ISessionFactory _lsDbFactory;

        /// <summary>
        /// Initilize service action
        /// </summary>
        /// <param name="previousInstanceContext"></param>
        public void OnLoad(object previousInstanceContext)
        {
            var config = Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(cs => cs.Is(
                $"Server={CfgDatabase.Default.LoginServerDaoHost};" +
                $"Database={CfgDatabase.Default.LoginServerDaoDatabase};" +
                $"User={CfgDatabase.Default.LoginServerDaoUsername}" +
                $";Password={CfgDatabase.Default.LoginServerDaoPassword};" +
                "CharSet=utf8"))).Mappings(m =>
                {
                    m.FluentMappings.AddFromNamespaceOf<UserMap>();
                });

            var export = new SchemaUpdate(config.BuildConfiguration());

            export.Execute(false, true);

            _lsDbFactory = config.BuildSessionFactory();
        }

        public void AuthProcess(ClientConnection client, string token)
        {
            using (var db = _lsDbFactory.OpenSession())
            {
                var model = db.QueryOver<AccountData>().Where(p => p.GameHash == token).Take(1).SingleOrDefault(); //get account data by token
                if (model == null)
                {
                    client.CloseConnection();
                    return;
                }
                if (model.ExpireTime < DateTime.Now)
                {
                    client.CloseConnection();
                    return;
                }

                client.Account = model;

				new SMSG_GetContentServiceInfo().Send(client, false);
                new SMSG_ChargeUser().Send(client, false);

                Core.Act(s => s.LobbyProcessor.GetCharacterList(client));
            }
        }



        public object OnUnload()
        {
            return null;
        }
    }
}
