using System.IO;
using Commons.Utils;
/*
   Author:Sagara, InCube
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_RegisterNickNameToAuthenticServer : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                //karyzir for InCube: i no have more time for analyse it
                writer.Write("0A6C4401".ToBytes());

                return stream.ToArray();
            }
        }
    }
}
