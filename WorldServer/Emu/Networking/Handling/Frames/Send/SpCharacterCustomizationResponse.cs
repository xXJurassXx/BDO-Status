using System.IO;
using Commons.Models.Character;
using Commons.Utils;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpCharacterCustomizationResponse : APacketProcessor
    {
        private readonly CharacterData _character;
        public SpCharacterCustomizationResponse(CharacterData character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("04A4C200".ToBytes()); //game session
                writer.Write("46B2C16FF2862300".ToBytes());
                //writer.WriteQ(_character.CharacterId);
                writer.Write("E86C000002000100320301003B03".ToBytes()); //unk todo

                return stream.ToArray();
            }
        }
    }
}