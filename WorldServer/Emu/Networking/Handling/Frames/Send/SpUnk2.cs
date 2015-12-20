/*
   Author: InCube
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpUnk2 : APacketProcessor
    {
        private byte[] unk = //0x0c94
        {
            0xFE,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0x00
        };

        public override byte[] WritedData()
        {
            return unk;
        }
    }
}
