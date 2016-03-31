using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Enums;
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
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Models.MySql.Mapping.WorldMap;
using WorldServer.Emu.Models.Storages;
using WorldServer.Emu.Models.Storages.Abstracts;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
using WorldServer.Emu.Networking.Handling.Frames.Send.OPSBlob;
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

        private static object _gsObj;

        /// <summary>
        /// Character uids generator
        /// </summary>
        private static Int32UidFactory _characterUidsFactory;

        /// <summary>
        /// Items uids generator
        /// </summary>
        private static Int32UidFactory _itemsUidsFactory;

        /// <summary>
        /// Game session uid`s generator
        /// </summary>
        private static Int32UidFactory _gameSessionFactory;

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
                        m.FluentMappings.AddFromNamespaceOf<CharacterMap>().AddFromNamespaceOf<StorageMap>();
                    });
            
            var export = new SchemaUpdate(config.BuildConfiguration());

            export.Execute(false, true);

            _gsDbFactory = config.BuildSessionFactory();

            var usedIds =  //Get all used character id`s
                _gsDbFactory.OpenSession().CreateSQLQuery("select `c_character_id` from `bd_characters`").List();

            var usedItemUids = _gsDbFactory.OpenSession().CreateSQLQuery("select `i_item_uid` from `bd_items`").List();

            if (usedIds.Any())
            {
                var cId = int.Parse(usedIds[usedIds.Count - 1].ToString());

                _characterUidsFactory = new Int32UidFactory(cId);
            }
            else _characterUidsFactory = new Int32UidFactory();
            
            if (usedItemUids.Any())
            {
                var iUid = int.Parse(usedItemUids[usedItemUids.Count - 1].ToString());

                _itemsUidsFactory = new Int32UidFactory(iUid);
            }
            else _itemsUidsFactory = new Int32UidFactory();

            _gameSessionFactory = new Int32UidFactory();
            _gsObj = new object();
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

                foreach (var characterData in connection.Characters)               
                    characterData.EquipmentData = new EquipmentStorage(db.QueryOver<CharacterItem>().Where(i => 
                    i.CharacterId == characterData.CharacterId && i.StorageType == (int)StorageType.Equipment).List().ToDictionary<CharacterItem, short, AStorageItem>(e => 
                    (short)(e.Slot + 1), e => new InventoryItem(e.ItemId, e.Count) {StorageType = (StorageType) e.StorageType}), 48);

				new SMSG_LoginUserToFieldServer(connection.Account, connection.Characters).Send(connection);
                new SMSG_FixedCharge();
            }
        }

        public void CreateCharacterProcess(ClientConnection connection, CharacterData info)
        {
            var characterData = info;

            using (var db = _gsDbFactory.OpenSession())
            {
                if (db.QueryOver<CharacterData>().Where(s => s.CharacterName == info.CharacterName).Take(1).SingleOrDefault() != null)
                {
                    new SMSG_CreateCharacterToFieldNak().Send(connection);

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
                        characterData.CreatedId = 0; // for nhibernate driver

						/* TEMP FIX UNTIL DATA IS FOUND AND LOADED */
						switch (characterData.ClassType)
						{
							case ClassType.Warrior:
                                characterData.PositionX = -153728;
								characterData.PositionY = 531;
								characterData.PositionZ = 130574;
								break;
							case ClassType.Ranger:
                                characterData.PositionX = -138793;
								characterData.PositionY = -1208;
								characterData.PositionZ = 137342;
								break;
							case ClassType.Sorcerer:
                                characterData.PositionX = -135621;
								characterData.PositionY = 802;
								characterData.PositionZ = 107359;
								break;
							case ClassType.Giant:
                                characterData.PositionX = -120118;
								characterData.PositionY = -1625;
								characterData.PositionZ = 118794;
								break;
							case ClassType.Tamer:
                                characterData.PositionX = -159853;
								characterData.PositionY = 2089;
								characterData.PositionZ = 123726;
								break;
							case ClassType.BladeMaster:
                                characterData.PositionX = -154408;
								characterData.PositionY = -335;
								characterData.PositionZ = 135204;
								break;
							case ClassType.BladeMasterWomen:
								characterData.PositionX = -154408;
								characterData.PositionY = -335;
								characterData.PositionZ = 135204;
								break;
							case ClassType.Valkyrie:
                                characterData.PositionX = -145479;
								characterData.PositionY = 2185;
								characterData.PositionZ = 110947;
								break;
							case ClassType.Kunoichi:
                                characterData.PositionX = -136152;
								characterData.PositionY = -730;
								characterData.PositionZ = 128869;
								break;
							case ClassType.Ninja:
								characterData.PositionX = -136152;
								characterData.PositionY = -730;
								characterData.PositionZ = 128869;
								break;
							case ClassType.Wizard:
                                characterData.PositionX = -157013;
								characterData.PositionY = 946;
								characterData.PositionZ = 128052;
								break;
							case ClassType.WizardWomen:
								characterData.PositionX = -157013;
								characterData.PositionY = 946;
								characterData.PositionZ = 128052;
								break;
						}

                        var inventory = InventoryStorage.GetDefault(characterData.ClassType);
                        foreach (var daoItem in inventory.Items.Select(item => new CharacterItem
                        {
                            CharacterId = characterData.CharacterId,
                            ItemId = item.Value.ItemId,
                            ItemUid = _itemsUidsFactory.Next(),
                            Slot = item.Key - 1,
                            Count = item.Value.Count,
                            StorageType = (int) ((InventoryItem)item.Value).StorageType
                        }))                       
                            db.Save(daoItem);
                        
                        db.Save(characterData);

                        connection.Characters.Add(characterData);

                        new SMSG_CreateCharacterToField(characterData).Send(connection, false);

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
                        var deleted = connection.Characters.First(s => s.CharacterId == characterId);

                        var equipment = db.QueryOver<CharacterItem>().Where(i => i.CharacterId == deleted.CharacterId).List();
                        for (int i = 0; i < equipment.Count; i++)
                            db.Delete(equipment[i]);
                        
                        db.Delete(deleted);

						// TODO: Delayed removed task
						bool isDelayedDelete = false;
                        new SMSG_RemoveCharacterFromField(characterId, isDelayedDelete, deletionTime).Send(connection);

                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(ex);
                    }
                }
            }
        }

        public void UpdateCharacter(ClientConnection connection)
        {
            using (var db = _gsDbFactory.OpenSession())
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var equipment = db.QueryOver<CharacterItem>().Where(i => i.CharacterId == connection.ActivePlayer.DatabaseCharacterData.CharacterId).List();
                        for (int i = 0; i < equipment.Count; i++)
                            db.Delete(equipment[i]);

                        /*Update inventory items*/
                        foreach (var daoItem in connection.ActivePlayer.Inventory.Items.Select(item => new CharacterItem
                        {
                            CharacterId = connection.ActivePlayer.DatabaseCharacterData.CharacterId,
                            ItemId = item.Value.ItemId,
                            ItemUid = _itemsUidsFactory.Next(),
                            Slot = item.Key - 1,
                            Count = item.Value.Count,
                            StorageType = (int)((InventoryItem)item.Value).StorageType
                        })) db.Save(daoItem);

                        /*Update equipment items*/
                        foreach (var daoItem in connection.ActivePlayer.Equipment.Items.Select(item => new CharacterItem
                        {
                            CharacterId = connection.ActivePlayer.DatabaseCharacterData.CharacterId,
                            ItemId = item.Value.ItemId,
                            ItemUid = _itemsUidsFactory.Next(),
                            Slot = item.Key - 1,
                            Count = item.Value.Count,
                            StorageType = (int)((InventoryItem)item.Value).StorageType
                        }))db.Save(daoItem);

                        db.Update(connection.ActivePlayer.DatabaseCharacterData);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Cannot update player id:{connection.ActivePlayer.DatabaseCharacterData.CharacterId} exception:\n{ex}");

                        transaction.Rollback();
                    }
                }
            }
        }

		public void InitialEnterWorld(ClientConnection connection, long characterId)
		{
			
			var player = new Player(connection, connection.Characters.First(s => s.CharacterId == characterId))
			{
				GameSessionId = _gameSessionFactory.Next(),
			};

			using (var db = _gsDbFactory.OpenSession())
			{
				var daoItems = db.QueryOver<CharacterItem>().Where(i => i.CharacterId == characterId).List();
				var items = new List<CharacterItem>();
				var equipItems = new List<CharacterItem>();

				foreach (var it in daoItems)
				{
					if (it.StorageType == (int)StorageType.Equipment)
						equipItems.Add(it);
					if (it.StorageType == (int)StorageType.Inventory)
						items.Add(it);
				}

				player.Inventory = new InventoryStorage(items.ToDictionary<CharacterItem, short, AStorageItem>(e => (short)(e.Slot + 1), e => new InventoryItem(e.ItemId, e.Count) { StorageType = (StorageType)e.StorageType }), 48);
				player.Equipment = new EquipmentStorage(equipItems.ToDictionary<CharacterItem, short, AStorageItem>(e => (short)(e.Slot + 1), e => new InventoryItem(e.ItemId, e.Count) { StorageType = (StorageType)e.StorageType }), 48);
			}

			connection.ActivePlayer = player;
			connection.ActivePlayer.PlayerActions += (action, parameters) =>
			{
				switch (action)
				{
					case Player.PlayerAction.Logout:
						if (connection.ActivePlayer != null)
						{
							UpdateCharacter(connection);
							_gameSessionFactory.ReleaseUniqueInt(connection.ActivePlayer.GameSessionId);
						}
						break;
				}
			};

			var sessionId = BitConverter.GetBytes(connection.ActivePlayer.GameSessionId).ToHex();
			var uid = BitConverter.GetBytes(connection.ActivePlayer.Uid).ToHex();
			
			new SMSG_CancelFieldEnterWaiting().Send(connection);
			new SMSG_SetGameTime().Send(connection);
			new SMSG_EnterPlayerCharacterToField(connection.ActivePlayer).Send(connection);
			new SMSG_LoadField().Send(connection);
			new SMSG_VariExtendSlot().Send(connection);
			new SMSG_SkillList(connection.ActivePlayer).Send(connection);
			new SMSG_SkillAwakenList().Send(connection);
			new SMSG_InventorySlotCount().Send(connection);
			new SMSG_AddItemToInventory(connection.ActivePlayer, 0).Send(connection); // normal
			new SMSG_AddItemToInventory(connection.ActivePlayer, 16).Send(connection); // cash or other
			new SMSG_GetAllEquipSlot(connection.ActivePlayer).Send(connection);
			new SMSG_LifeExperienceInformation(0).Send(connection);
			new SMSG_LifeExperienceInformation(1).Send(connection);
			new SMSG_LifeExperienceInformation(2).Send(connection);
			new SMSG_LifeExperienceInformation(3).Send(connection);
			new SMSG_LifeExperienceInformation(4).Send(connection);
			new SMSG_LifeExperienceInformation(5).Send(connection);
			new SMSG_LifeExperienceInformation(6).Send(connection);
			new SMSG_LifeExperienceInformation(7).Send(connection);
			new SMSG_LifeExperienceInformation(8).Send(connection);
			// SMSG_PaymentPassword
			new SpRaw("0077E6EA56000000000747F8593300000000D2DA91B4000000004FEC1DD201000000C3615D160100000021E9FFAB0100000058B599E40100000099747B61000000002991FDF500000000C39B5DDE01000000EFA508A1000000009F4D912C01000000D0E7FC420200000081E2840E000000008929F016010000009948079101000000181BC443010000007728217800000000CB8591F8000000001140914802000000F34B1BA401000000.", 0x0CE0).SendRaw(connection);
			// SMSG_ListFitnessExperience
			new SpRaw("00000000010000000E01000000000000010000000100000000000000000000000200000001000000000000000000000000000000000000000000000000000000", 0x1090).SendRaw(connection);
			new SMSG_SetCharacterLevels(connection.ActivePlayer).Send(connection);
			// SMSG_SetCharacterPrivatePoints
			new SpRaw(sessionId + "707B19000000000000007A440000000000000000000000000000400D0300400D0300400D0300400D0300", 0x0F7D).SendRaw(connection);
			// SMSG_SetCharacterStats
			new SpRaw("01" + sessionId + "0000E0400000E0400000284100006041000060400000000000000000707B190000000000010000000000C040030000000000000040420F00000000000000000000000000", 0x0F80).SendRaw(connection);
			// SMSG_SetCharacterSpeeds
			new SpRaw("000000000000000000000000", 0x0F82).SendRaw(connection);
			// SMSG_SetCharacterSkillPoints
			new SpRaw("0100050008008C080000", 0x0F85).SendRaw(connection);
			// SMSG_SetCharacterProductSkillPoints
			new SpRaw("0000000000000000", 0x0F89).SendRaw(connection);
			// SMSG_UpdateStamina
			new SpRaw("02E8030000C8000000E8030000", 0x0EC0).SendRaw(connection);
			// SMSG_SetCharacterStatPoint
			new SpRaw(sessionId + "0500000005000000050000000A0000000A0000000A0000000000000005000000000000000500000000000000050000000000000005000000", 0x0F7F).SendRaw(connection);
			// SMSG_QuickSlotList
			new SpRaw(uid + "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 0x0D2D).SendRaw(connection);
			// SMSG_ListQuestSelectType
			new SpRaw("00000000000000000000000000000000000000000000000000", 0x0E89).SendRaw(connection);
			// SMSG_ListQuestSortType
			new SpRaw("00000000000000000000", 0x0E8C).SendRaw(connection);
			// SMSG_SetMurdererState
			new SpRaw(sessionId + "0000000000000000", 0x0D45).SendRaw(connection);
			// SMSG_ExplorePointList
			new SpRaw("01000000000000000A00000000000000", 0x0E9C).SendRaw(connection);
			// SMSG_SupportPointList
			new SpRaw("01000000000000000100000000000000", 0x0EC2).SendRaw(connection);
			// SMSG_ProgressingQuestList
			//new SpRaw("02009E000400000000000000000000000000000000000000000077FDE6560000000092010100000000000000000000000000000000000000000078FDE65600000000", 0x0E5D).SendRaw(connection);
			// SMSG_ClearedQuestList
			//new SpRaw("0C00F701010000000000000000009F00010000000000000000009E00010000000000000000009F00020000000000000000009F0003000000000000000000E9036B0000000000000000009F00040000000000000000009F00050000000000000000009F00060000000000000000009F00070000000000000000009E00030000000000000000009E0002000000000000000000", 0x0E5E).SendRaw(connection);
			// SMSG_ListCheckedQuest
			//new SpRaw("3530332C302C3135382C342C3430322C312C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 0x0E5F).SendRaw(connection);
			// SMSG_UpdateTitleKey
			new SpRaw("00000000" + sessionId, 0x0CE3).SendRaw(connection);
			// SMSG_ListIntimacy
			new SpRaw("0000", 0x0EC4).SendRaw(connection);
			// SMSG_ExplorationInfo
			new SpRaw("00000000", 0x0E9E).SendRaw(connection);
			// SMSG_UpdateExplorationPoint
			new SpRaw("0000", 0x0EA1).SendRaw(connection);
			// SMSG_PlantInfo
			new SpRaw("0000", 0x0EC9).SendRaw(connection);
			new SpRaw("0000", 0x0EC9).SendRaw(connection);
			new SpRaw("0000", 0x0EC9).SendRaw(connection);
			// SMSG_ReservedLearningSkill
			new SpRaw("000000000300000000000000", 0x0D32).SendRaw(connection);
			// SMSG_ListHouseForTownManagement
			new SpRaw("0000", 0x0E8F).SendRaw(connection);
			// SMSG_ListHouseLargeCraft
			new SpRaw("0000", 0x0E90).SendRaw(connection);
			// SMSG_SetWp
			new SpRaw("20001E00", 0x0C70).SendRaw(connection);
			// SMSG_ListMyVendor
			new SpRaw("0000", 0x0FE9).SendRaw(connection);
			// SMSG_ListProgressChallenge
			//new SpRaw("000000", 0x10CF).SendRaw(connection);
			// SMSG_ListCompleteChallenge
			//new SpRaw("000000", 0x10D0).SendRaw(connection);
			// SMSG_ListPalette
			new SpRaw("01000000", 0x0C2B).SendRaw(connection);
			new SMSG_AddPlayers(connection.ActivePlayer, 1).Send(connection);
			// SMSG_ChargeUser
			new SpRaw("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 0x0C0A).SendRaw(connection);
			new SMSG_PlayerLogOnOff(connection.ActivePlayer).Send(connection);
			// SMSG_AddGuildHouseCraftList
			new SpRaw("8CE9FFFFFFFFFFFF0000", 0x1013).SendRaw(connection);
			// SMSG_DailyWorkingCountList
			new SpRaw("8CE9FFFFFFFFFFFF0000", 0x1015).SendRaw(connection);
			// SMSG_SetMyHouseForTownManagement
			new SpRaw("00000000000000000000010100", 0x0E97).SendRaw(connection);
			// SMSG_MaidInfoTotal
			new SpRaw("0000", 0x0FBE).SendRaw(connection);
			// SMSG_SetCharacterPublicPoints
			new SpRaw("8CE9FFFFFFFFFFFF8CE9FFFFFFFFFFFF" + sessionId + "000000000000000000003A4300003A4300003A4300FCFFFF", 0x0F75).SendRaw(connection);
			// SMSG_SetCharacterRelatedPoints
			new SpRaw("8CE9FFFFFFFFFFFF8CE9FFFFFFFFFFFF" + sessionId + "0000000000000000A3000000A3000000A300000000", 0x0F76).SendRaw(connection);
			// SMSG_LoadCustomizedKeys
			new SpRaw("0100000000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF0000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FF", 0x10BB).SendRaw(connection);
			// SMSG_BidTerritoryTradeAuthority
			new SpRaw("00000000FFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 0x0F52).SendRaw(connection);
			// SMSG_SupplyTerritoryStart
			new SpRaw("00000000000000000000000000", 0x0F9C).SendRaw(connection);
			new SMSG_LoadFieldComplete().Send(connection);
			new SMSG_EnterPlayerCharacterToFieldComplete().Send(connection);
			// SMSG_EnablePvP
			new SpRaw(sessionId + "0000", 0x0D42).SendRaw(connection);
			// SMSG_EnableAdrenalin
			new SpRaw("00", 0x0D46).SendRaw(connection);
			new SMSG_RefreshPcCustomizationCache(connection.ActivePlayer).Send(connection);
			new SMSG_RefreshPcLearnedActiveSkillsCache(connection.ActivePlayer).Send(connection);
			new SBpPlayerSpawn.SMSG_RefreshPcEquipSlotCache(connection.ActivePlayer).Send(connection);
			new SBpPlayerSpawn.SMSG_RefreshUserBasicCache(connection.ActivePlayer).Send(connection);
			new SBpPlayerSpawn.SMSG_RefreshPcBasicCache(connection.ActivePlayer).Send(connection);

			Core.Act(s =>
			{
				s.CharacterProcessor.EndLoad(connection);
				s.WorldProcessor.EndLoad(connection);
			});
		}

		public void PrepareForEnterOnWorld(ClientConnection connection, long characterId)
        {
			// NOT USED
		}

        public void EnterOnWorldProcess(ClientConnection connection, int gameSession)
        {
			// NOT USED
        }

		public void BackToServerSelection(ClientConnection connection, int cookieId)
		{
			// TODO: method / task
			new SMSG_ExitFieldServerToServerSelection().Send(connection);
		}

		public void SetReadyToPlay(ClientConnection connection)
		{
			// Unlock Character - Special Thanks to MadHunter
			new SMSG_RideOnVehicle(connection.ActivePlayer).Send(connection);
		}

		public object OnUnload()
        {
            return null;
        }

        #region static
        public static int GetCharactersCount(int accountId)
        {
            lock (_gsObj)
            using (var db = _gsDbFactory.OpenSession())
                {
                    int count = db.QueryOver<CharacterData>().Where(p => p.AccountId == accountId).List().Count;

                    return count;
                }
        }

        #endregion

    }
}
