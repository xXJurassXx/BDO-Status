using System.IO;
using System.Text;
using Commons.Models.Character;
using Commons.Utils;
using WorldServer.Emu.Extensions;
/*
   Author: Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpCharacterInformation : APacketProcessor
    {
        private readonly CharacterData _character;
        public SpCharacterInformation(CharacterData character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("04A4C200".ToBytes());
                writer.Write("D309000000000000".ToBytes()); 
                writer.Write("46B2C16FF2862300".ToBytes());
                //writer.WriteQ(_character.CharacterId);
                writer.Write("B1E57656".ToBytes()); //todo - static server date time
                writer.Skip(12);
                writer.Write("1E000000".ToBytes());
                writer.Write(BinaryExt.WriteFixedString(_character.CharacterName, Encoding.Unicode, 62));
                writer.Write(_character.Level);
                writer.Write("ABE7FFFFFFFFFFFF".ToBytes());
                writer.Write(BinaryExt.WriteFixedString(_character.Surname, Encoding.Unicode, 62));
                writer.WriteH(4); //unk
                writer.WriteH(_character.ClassType.Ordinal());

                return stream.ToArray();
            }
        }
    }
}
