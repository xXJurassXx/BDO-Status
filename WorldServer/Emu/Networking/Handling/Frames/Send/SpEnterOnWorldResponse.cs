namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpEnterOnWorldResponse : APacketProcessor
    {
        public override byte[] WritedData()
        {
            return new byte[12];
        }
    }
}
