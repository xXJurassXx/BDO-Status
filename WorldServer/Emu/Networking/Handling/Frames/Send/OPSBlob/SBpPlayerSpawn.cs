using System.IO;
using System.Text;
using Commons.Models.Account;
using Commons.Utils;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
   TODO - Analyse all data
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send.OPSBlob
{
    public class SBpPlayerSpawn
    {
        private readonly Player _player;
        private readonly ClientConnection _spawnedConnection;
        private readonly ClientConnection _connection;

        public SBpPlayerSpawn(ClientConnection spawnedConnection, ClientConnection connection)
        {
            _player = _spawnedConnection.ActivePlayer;
            _spawnedConnection = spawnedConnection;
            _connection = connection;
        }

        /// <summary>
        /// Spawn other player for me
        /// </summary>
        public void SpawnPlayer()
        {
            SendPlayerName();
            SendFamilyName();
            SendPlayerEquipment();
            SendSpawnPlayerData();

            new SMSG_RefreshPcCustomizationCache(_player).Send(_connection);
            new SMSG_RefreshPcLearnedActiveSkillsCache(_player).Send(_connection);
        }

        public void SendPlayerName()
        {
            new SMSG_RefreshPcBasicCache(_player).Send(_connection);
        }

        public void SendPlayerEquipment()
        {
            new SMSG_RefreshPcEquipSlotCache(_player).Send(_connection);
        }

        public void SendFamilyName()
        {
            new SMSG_RefreshUserBasicCache(_player, _spawnedConnection.Account).Send(_connection);
        }

        public void SendSpawnPlayerData()
        {
            new SpSpawnPlayer(_player).Send(_connection);
        }

        public class SMSG_RefreshPcBasicCache : APacketProcessor
        {
            private readonly Player _player;

            public SMSG_RefreshPcBasicCache(Player player)
            {
                _player = player;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
					// SMSG_RefreshPcBasicCache
					
                    writer.Write((int)_player.GameSessionId);                   
                    writer.Write((long)_player.Uid);
                    writer.Write((int)1); // incremental
                    writer.Write(BinaryExt.WriteFixedString(_player.DatabaseCharacterData.CharacterName, Encoding.Unicode, 62));
                    writer.Write((byte)_player.DatabaseCharacterData.AppearancePresets[0]); // face
                    writer.Write((byte)_player.DatabaseCharacterData.AppearancePresets[1]); // hair
                    writer.Write((byte)_player.DatabaseCharacterData.AppearancePresets[2]); // goatee
                    writer.Write((byte)_player.DatabaseCharacterData.AppearancePresets[3]); // mustache
                    writer.Write((byte)_player.DatabaseCharacterData.AppearancePresets[4]); // sideburns
                    writer.Write((byte)5); // TODO: eyebrows

                    return stream.ToArray();
                }
            }
        }

        public class SMSG_RefreshUserBasicCache : APacketProcessor
        {
            private readonly Player _player;
            private readonly AccountData _accountInfo;

            public SMSG_RefreshUserBasicCache(Player player, AccountData accountInfo)
            {
                _player = player;
                _accountInfo = accountInfo;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
					// SMSG_RefreshUserBasicCache

					writer.Write((int)_player.GameSessionId);
                    writer.Write((long)_player.DatabaseCharacterData.AccountId);
                    writer.Write((int)1); // incremental
                    writer.Write(BinaryExt.WriteFixedString(_accountInfo.FamilyName, Encoding.Unicode, 62));
                    writer.Write(new byte [402]); // possibly text string

                    return stream.ToArray();
                }
            }
        }

        public class SMSG_RefreshPcEquipSlotCache : APacketProcessor
        {
            private readonly Player _player;

            public SMSG_RefreshPcEquipSlotCache(Player player)
            {
                _player = player;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
					// SMSG_RefreshPcEquipSlotCache

					writer.Write((int)_player.GameSessionId);
                    writer.Write((long)_player.Uid);
                    writer.Write((int)1); // incremental 
					/* h,h,Q,h,h*12,c */
                    writer.Write("00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" + 
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToBytes());

                    return stream.ToArray();
                }
            }
        }

        public class SpSpawnPlayer : APacketProcessor
        {
            private readonly Player _player;

            public SpSpawnPlayer(Player player)
            {
                _player = player;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write("0100000100FCFFFF0000000000000000000000000100".ToBytes());
                    writer.WriteD(_player.GameSessionId);
                    writer.WriteF(_player.Position.Point.X);
                    writer.WriteF(_player.Position.Point.Y);
                    writer.WriteF(_player.Position.Point.Z);
                    writer.WriteD(0);
                    writer.WriteF(_player.Position.Cosinus);
                    writer.WriteF(_player.Position.Sinus);
                    writer.WriteD(_player.DatabaseCharacterData.ClassType.Ordinal());
                    writer.Write("00003243".ToBytes()); //curr hp
                    writer.Write("00003243".ToBytes()); //max hp
                    writer.Write("F5FCFFFFFFFFFFFFF5FCFFFFFFFFFFFF02946DE49500000000000000000000000000000000030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000309A00000000000000000000000000000000000000000000000000000000000010300000000000000000000000000000000000000000000000000000000000001030000000000000000000000000000000000000000000000000000000000000183B000000000000000000000000000000000000000000000000000000000000583A000000000000000000000000000000000000000000000000000000000000D8EF0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000178D000000000000000000000000000000000000000000000000000000000000180000000000000000000000000000000000000000000000000000000000000089AE000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000077D1000000000000000000000000000000000000000000000000000000000000B0DB00000000000000000000000000000000000000000000000000000000000049F000000000000000000000000000000000000000000000000000000000000001F1000000000000000000000000000000000000000000000000000000000000A5510000000000000000000000000000000000000000000000000000000000006AEF000000000000000000000000000000000000000000000000000000000000FD01000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FFFF883C0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FD0100000000000000000000000000000000000000000000000000000000000060F100000000000000000000000000000000000000000000000000000000000060F10000000000000000000000000000000000000000000000000000000000000300287300000000000001000000".ToBytes());
                    writer.WriteQ(_player.Uid);
                    writer.WriteD(1);
                    writer.WriteD(1);
                    writer.WriteD(3);
                    writer.Write("010000000000000000000000000000004238000000FCFFFF25000000000000000000000000053401000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000FCFFFF0108000000000000000300000000000000000040085F0CC16FF286230000000000E86C00000000000000000000000000000000000000FCFFFF0000000000000000000000000200000000000000000000000000".ToBytes());
                    return stream.ToArray();
                }
            }
        }
    }
}
