using System.IO;
using System.Text;
using Commons.Models.Character;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpCharacterCustimozationData : APacketProcessor
    {
        private readonly CharacterData _character;
        public SpCharacterCustimozationData(CharacterData character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("04A4C200".ToBytes()); //todo game sessions
                writer.Write("46B2C16FF2862300".ToBytes()); //46B2C16FF2862300
                //writer.WriteQ(_character.CharacterId);
                writer.WriteD(1);
                writer.Write(_character.AppearanceOptions);
                writer.Write(BinaryExt.WriteFixedString(_character.Surname, Encoding.Unicode, 62));

                return stream.ToArray();
            }
        }
    }
}
