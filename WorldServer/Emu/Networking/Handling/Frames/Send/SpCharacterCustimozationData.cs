using System.IO;
using System.Text;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpCharacterCustimozationData : APacketProcessor
    {
        private readonly Player _character;
        public SpCharacterCustimozationData(Player character)
        {
            _character = character;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(_character.GameSessionId); 
                writer.Write(_character.Uid); //46B2C16FF2862300
                writer.WriteD(1);
                writer.Write(_character.DatabaseCharacterData.AppearanceOptions);
                writer.Write(BinaryExt.WriteFixedString(_character.DatabaseCharacterData.Surname, Encoding.Unicode, 62));

                return stream.ToArray();
            }
        }
    }
}
