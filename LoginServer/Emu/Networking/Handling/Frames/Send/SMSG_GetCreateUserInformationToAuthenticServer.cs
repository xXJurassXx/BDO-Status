using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author: Sagara, InCube, RBW
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
				/* S(62),c,c,c,Q*20 */
                writer.Write(BinaryExt.WriteFixedString(_hash, Encoding.Unicode, 62));
				writer.Write((byte)0); // usePinCode boolean: 0=no, 1=yes [NA/EU always 0]
				writer.Write((byte)0); // registerPinCode boolean: 0=request, 1=register [NA/EU always 0]
				writer.Write((byte)7); // index of correct pin panel number order block to be used/show to user [should not be static]
				// 20 * 8 bytes pin panel number order
				writer.Write("D5D68AF200000000".ToBytes());
				writer.Write("421F3F0002000000".ToBytes());
				writer.Write("622E4A7600000000".ToBytes());
				writer.Write("EC329D5E00000000".ToBytes());
				writer.Write("26D9A3DD00000000".ToBytes());
				writer.Write("BD0D10A900000000".ToBytes());
				writer.Write("F6C0FD5201000000".ToBytes());
				writer.Write("96C2317101000000".ToBytes()); // correct/used pin panel order
				writer.Write("232B9C6000000000".ToBytes());
				writer.Write("3B135D2601000000".ToBytes());
				writer.Write("A766CD8D00000000".ToBytes());
				writer.Write("86CC1F0402000000".ToBytes());
				writer.Write("D1F4155601000000".ToBytes());
				writer.Write("D970D22200000000".ToBytes());
				writer.Write("610E617601000000".ToBytes());
				writer.Write("0D80FA9200000000".ToBytes());
				writer.Write("8F94852C00000000".ToBytes());
				writer.Write("6A9D73D401000000".ToBytes());
				writer.Write("02ABBD2C02000000".ToBytes());
				writer.Write("77B2F20001000000".ToBytes());

                return stream.ToArray();
            }
        }
    }
}