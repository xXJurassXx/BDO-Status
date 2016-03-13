using System.IO;
using Commons.Utils;
/*
   Author:Sagara, InCube
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_GetContentServiceInfo : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                //karyzir for InCube: i no have more time for analyse it
                //That client config packet
                writer.Write("04060CC0C62D0000000000E093040000000000E0930400E093040060EA0000000000000004080C10141518191A1C1F2020202020202020202020202020202020202020000000002E000000805101000000000080F403000000000080812B00000000000001010D000196000000010000".ToBytes());

                return stream.ToArray();
            }
        }
    }
}
