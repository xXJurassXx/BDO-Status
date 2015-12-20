using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    public class SpSetHash : APacketProcessor
    {
        private readonly string _hash;
        public SpSetHash(string hash)
        {
            _hash = hash;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {    
                writer.Write(BinaryExt.WriteFixedString(_hash, Encoding.ASCII, 38));       
                writer.Write("00000000000000000000000000000000000000000000000000087039C3CB000000003D6E313800000000C0E0FF420100000008E7E695000000004F405F4802000000B473D1C501000000EC08903D01000000FA623C5F010000007E1A168100000000EBC7F908020000008378B1790000000027B850F501000000A2712A210100000023DAD81901000000B17769C001000000DF04ABB800000000E48FDAEB0100000076164AE201000000EB326C0F000000001E59AF3702000000".ToBytes());

                return stream.ToArray();
            }
        }
    }
}
