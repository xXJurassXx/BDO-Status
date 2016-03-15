using System.IO;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    internal class SMSG_CreateCharacterToFieldNak : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
				/* d */
                writer.Write("2284ADB1"); // name already used. TODO: find others

                return stream.ToArray();
            }
        }
    }
}
