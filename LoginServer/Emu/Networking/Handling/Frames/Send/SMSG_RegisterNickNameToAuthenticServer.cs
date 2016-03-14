using System.IO;
using Commons.Utils;
/*
   Author: Sagara, InCube, RBW
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
				/* d */
                writer.Write((int)34829344); // not static system message - change each login try - could be a time

                return stream.ToArray();
            }
        }
    }
}
