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

            new SpCharacterCustimozationData(_player).Send(_connection);
            new SpCharacterCustomizationResponse(_player).Send(_connection);
        }

        public void SendPlayerName()
        {
            new SpSetPlayerName(_player).Send(_connection);
        }

        public void SendPlayerEquipment()
        {
            new SpSetPlayerEquipment(_player).Send(_connection);
        }

        public void SendFamilyName()
        {
            new SpSetPlayerFamilyName(_player, _spawnedConnection.Account).Send(_connection);
        }

        public void SendSpawnPlayerData()
        {
            new SpSpawnPlayer(_player).Send(_connection);
        }

        public class SpSetPlayerName : APacketProcessor
        {
            private readonly Player _player;

            public SpSetPlayerName(Player player)
            {
                _player = player;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.WriteD(_player.GameSessionId);                   
                    writer.WriteQ(_player.Uid);
                    writer.WriteD(1); //unk, type maybe or flag
                    writer.Write(BinaryExt.WriteFixedString(_player.DatabaseCharacterData.CharacterName, Encoding.Unicode, 62));
                    writer.WriteC(_player.DatabaseCharacterData.AppearancePresets[0]); //face
                    writer.WriteC(_player.DatabaseCharacterData.AppearancePresets[1]); //hair
                    writer.WriteC(_player.DatabaseCharacterData.AppearancePresets[2]); //goatee
                    writer.WriteC(_player.DatabaseCharacterData.AppearancePresets[3]); //mustache
                    writer.WriteC(_player.DatabaseCharacterData.AppearancePresets[4]); //sideburns
                    writer.Write("05".ToBytes()); //020104070705 unk

                    return stream.ToArray();
                }
            }
        }

        public class SpSetPlayerFamilyName : APacketProcessor
        {
            private readonly Player _player;
            private readonly AccountData _accountInfo;

            public SpSetPlayerFamilyName(Player player, AccountData accountInfo)
            {
                _player = player;
                _accountInfo = accountInfo;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(_player.GameSessionId);
                    writer.Write("2873000000000000".ToBytes()); //unk
                    writer.WriteD(1);
                    writer.Write(BinaryExt.WriteFixedString(_accountInfo.FamilyName, Encoding.Unicode, 62));
                    writer.Skip(402);

                    return stream.ToArray();
                }
            }
        }

        public class SpSetPlayerEquipment : APacketProcessor
        {
            private readonly Player _player;

            public SpSetPlayerEquipment(Player player)
            {
                _player = player;
            }

            public override byte[] WritedData()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.WriteD(_player.GameSessionId);
                    writer.WriteQ(_player.Uid);
                    writer.WriteD(3); //unk 
                    writer.Write("00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +  //Weapon 
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +  //Second weapon
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
                                 "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToBytes()); //TODO

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
