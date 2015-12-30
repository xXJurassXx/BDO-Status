using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpCharacterCustomizationResponse : APacketProcessor
    {
        private readonly Player _character;
        public SpCharacterCustomizationResponse(Player character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.WriteD(_character.GameSessionId);
                writer.WriteQ(_character.Uid);
                writer.Write("E86C0000".ToBytes()); //unk todo
                writer.Write("02000100320301003B03".ToBytes()); //unk
                return stream.ToArray();
            }
        }
    }
}