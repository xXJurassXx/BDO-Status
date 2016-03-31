using System.IO;
using Commons.Utils;
/*
   Author: Sagara, InCube, RBW
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_LoginUserToAuthenticServer : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
				/* d,c,d */
				writer.Write((int)1324048112); // clientSessionId [TODO: client session generator, it must NEVER be equal for all players]
				writer.Write((byte)0); // registerFamilyName boolean [0=no, 1=yes]
				writer.Write((int)4530); // serverVersion [updated from new server/client updates]

				return stream.ToArray();
            }
        }
    }
}
