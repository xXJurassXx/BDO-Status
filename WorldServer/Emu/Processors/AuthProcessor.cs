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

				// SMSG_GetContentServiceInfo
				//new SpRaw("040409C0C62D0000000000E093040000000000400D0300400D030060EA0000000000000004080C10181C1F202020202020202020202020202020202020202020202020380000000100000080510100000000008051010000000000805101000000000000010009000096000000003200040100040200040300040400040500040600040700040800040900040B00040C00040D00040E00040F00041000041100041200041300041400041500041600041700041800041900041F00042100042200042400042500042600042700042800042900042A00042B00042C00042D00042E00043D00043E00043F00044000046500046600046700046800046900046A00046B00046C00", 0x0C9C).SendRaw(client);
				// SMSG_ChargeUser
				//new SpRaw("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 0x0C0A).SendRaw(client);

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
