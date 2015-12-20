using System;
using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    public class SpServerlist : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Skip(4);
                writer.WriteD(DateTime.Now.Millisecond);
                writer.Skip(4);

                writer.WriteH(1); //realms count

                writer.WriteH(1); //ChanelId
                writer.WriteH(1); //realm id

                writer.Write(BinaryExt.WriteFixedString(" Channel - 1", Encoding.Unicode, 62));
                writer.Write(BinaryExt.WriteFixedString(" BDEmulator", Encoding.Unicode, 62));

                writer.Write("CB4B00".ToBytes());

                writer.Write(BinaryExt.WriteFixedString("127.0.0.1", Encoding.ASCII, 16));
                writer.WriteH(8889);

                writer.WriteC(1);
                writer.WriteC(1);
                writer.WriteC(1);

                writer.WriteC(0);
                writer.WriteC(8);

                writer.Write("0000FEFFFFFFFFFFFFFFD48D6155000000000000000000000000000000000000".ToBytes());
                return stream.ToArray();
            }
        }
    }
}
