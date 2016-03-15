using System.IO;
using Commons.Utils;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_RemoveCharacterFromField : APacketProcessor
    {
        private readonly long _characterid;
        private readonly bool _result;
        private readonly int _date;

        public SMSG_RemoveCharacterFromField(long id, bool result, int date)
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
				/* Q,Q,c */
                writer.Write((long)_characterid);
                writer.Write((long)_date);
                writer.Write((bool)_result);

                return stream.ToArray();
            }
        }
    }
}
