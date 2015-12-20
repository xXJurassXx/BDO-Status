using System.Collections.Generic;
using System.IO;
using System.Text;
using Commons.Models.Account;
using Commons.Models.Character;
using Commons.Utils;
using NHibernate.Util;
using WorldServer.Emu.Extensions;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpCharacterList : APacketProcessor
    {
        private AccountData _account;
        private readonly List<CharacterData> _characters;

        private readonly byte[] _staticField = "FFFF00000100FFFFFFFFFFFFFFFF0000000000000000".ToBytes();
        private readonly byte[] _inventoryField = "00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FEFFFFFFFFFFFFFF0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToBytes();

        public SpCharacterList(AccountData account, List<CharacterData> characters)
        {
            _account = account;
            _characters = characters;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Skip(24);
                writer.Write(BinaryExt.WriteFixedString(_account.FamilyName, Encoding.Unicode, 62));

                /*GAG || If charaters not exist */

                if (!_characters.Any())
                {
                    writer.WriteH(65535);
                    writer.WriteD(0);
                    writer.WriteQ(-1);
                    writer.WriteQ(0);
                    writer.WriteC(0);
                    writer.WriteQ(0);
                    writer.WriteH(0);
                    writer.WriteC(254);
                    writer.WriteD(65535);
                    writer.WriteC(0);
                    writer.WriteC(254);
                    writer.WriteQ(-1);
                    writer.WriteQ(0);
                    writer.WriteQ(0);
                    writer.WriteQ(0);
                    writer.WriteC(0);
                    writer.WriteC(254);
                    writer.WriteC(255);
                    writer.WriteH(65535);
                    writer.WriteQ(-1);
                    writer.Skip(51);

                    return stream.ToArray();
                }

                writer.Write(_staticField);
                writer.WriteC(_characters.Count);

                for (int index = 0; index < _characters.Count; index++)
                {
                    var characterInfo = _characters[index];

                    writer.WriteH(characterInfo.ClassType.Ordinal());
                    writer.WriteQ(characterInfo.CharacterId);
                    writer.WriteC(0);
                    writer.Write(BinaryExt.WriteFixedString(characterInfo.CharacterName, Encoding.Unicode, 62));
                    writer.WriteQ(characterInfo.Level);
                    writer.Write(_inventoryField);
                    writer.Write(characterInfo.AppearancePresets);
                    writer.WriteC((byte)characterInfo.Zodiac);
                    writer.Write(characterInfo.AppearanceOptions);
                    writer.Skip(62);
                    writer.Write("FFFFFFFFFFFFFFFF001E69745600000000717A715600000000805FF2C70000B8C40028DD47FFFFFFFFFFFFFFFF0000040E7456000000000000000000000000000000000000000000000000000000000000D0FFFF43".ToBytes());
                }

                return stream.ToArray();
            }
        }
    }  
}