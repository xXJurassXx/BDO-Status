/*
   Author:Sagara
*/
using System;
using Commons.Models.Account;
using Commons.Networking.Remoted;
using Commons.Utils;
using LoginServer.Emu.Interfaces;
using LoginServer.Emu.Networking;
using NLog;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LoginServer.Configs;
using LoginServer.Emu.Networking.Handling.Frames.Send;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace LoginServer.Emu.Processors
{
    public class AuthProcessor : IProcessor
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Database session factory
        /// </summary>
        private static ISessionFactory _dbFactory;

        /// <summary>
        /// Initilize service action
        /// </summary>
        /// <param name="previousInstanceContext"></param>
        public void OnLoad(object previousInstanceContext)
        {
            var config = Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(cs => cs.Is(
                    $"Server={CfgDatabase.Default.Host};" +
                    $"Database={CfgDatabase.Default.Database};" +
                    $"User={CfgDatabase.Default.Username}" +
                    $";Password={CfgDatabase.Default.Password};" +
                    "CharSet=utf8"))).Mappings(m => m.FluentMappings.AddFromAssemblyOf<AuthProcessor>());

            var export = new SchemaUpdate(config.BuildConfiguration());

            export.Execute(false, true);

            _dbFactory = config.BuildSessionFactory();
        }

        /// <summary>
        /// Process authorize, check token and configure connection
        /// </summary>
        /// <param name="client"></param>
        /// <param name="token"></param>
        public void AuthProcess(ClientConnection client, string token)
        {
            using (var db = _dbFactory.OpenSession())
            {
                var model = db.QueryOver<AccountData>().Where(p => p.Token == token).Take(1).SingleOrDefault(); //get account data by token
                if (model == null) //if data is null, database not contains token
                {
                    Log.Info($"Cannot find account by token: {token}");
                    client.CloseConnection();
                    return;
                }

                if (model.ExpireTime < DateTime.Now) //if token time has expire, close connection
                {
                    Log.Info($"Token time has expired, account id {model.Id}, close connection");
                    client.CloseConnection();
                    return;
                }

                client.AccountInfo = model;

                var gameToken = RndExt.RandomString(7);

                db.CreateSQLQuery($"UPDATE bd_accounts SET a_game_hash=? WHERE a_id={model.Id} ").SetString(0, gameToken).ExecuteUpdate();

                new SMSG_GetCreateUserInformationToAuthenticServer(gameToken).Send(client);
            }
        }

        public static int GetCharacterCount(int accountId)
        {
            try
            {
                var obj = (IRMIRealmService)Activator.GetObject(typeof(IRMIRealmService), $"tcp://{CfgNetwork.Default.RemotedHost}:5546/realm_service");

                return obj.CharacterCount(accountId);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured\n{ex}");

                return 0;
            }           
        }

        public object OnUnload()
        {
            return null;
        }
    }
}
