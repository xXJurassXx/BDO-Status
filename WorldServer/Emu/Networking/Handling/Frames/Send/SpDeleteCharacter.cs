using System.IO;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpDeleteCharacter : APacketProcessor
    {
        private readonly long _characterid;
        private readonly byte _result;
        private readonly int _date;

        public SpDeleteCharacter(long id, byte result, int date)
        {
            _characterid = id;
            _date = date;
            _result = result;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.WriteQ(_characterid);
                writer.WriteQ(_date);
                writer.WriteC(_result);

                return stream.ToArray();
            }
        }
    }
}
