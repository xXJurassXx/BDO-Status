using System.IO;
using System.Text;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author: Sagara. RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SMSG_RefreshPcCustomizationCache : APacketProcessor
    {
        private readonly Player _character;

        public SMSG_RefreshPcCustomizationCache(Player character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(_character.GameSessionId); 
                writer.Write(_character.Uid);
                writer.WriteD(1);
                writer.Write(_character.DatabaseCharacterData.AppearanceOptions);
                writer.Write(BinaryExt.WriteFixedString(_character.DatabaseCharacterData.Surname, Encoding.Unicode, 62));

                return stream.ToArray();
            }
        }
    }
}
