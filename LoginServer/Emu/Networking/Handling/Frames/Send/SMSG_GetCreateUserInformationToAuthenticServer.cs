using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author:Sagara, InCube
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_GetCreateUserInformationToAuthenticServer : APacketProcessor
    {
        private readonly string _hash;
        public SMSG_GetCreateUserInformationToAuthenticServer(string hash)
        {
            _hash = hash;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {    
                writer.Write(BinaryExt.WriteFixedString(_hash, Encoding.ASCII, 62));       
                writer.Write("010007D5D68AF200000000421F3F0002000000622E4A7600000000EC329D5E0000000026D9A3DD00000000BD0D10A900000000F6C0FD520100000096C2317101000000232B9C60000000003B135D2601000000A766CD8D0000000086CC1F0402000000D1F4155601000000D970D22200000000610E6176010000000D80FA92000000008F94852C000000006A9D73D40100000002ABBD2C0200000077B2F20001000000".ToBytes());

                return stream.ToArray();
            }
        }
    }
}