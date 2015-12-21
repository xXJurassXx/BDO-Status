using System;
using System.IO;
using System.Linq;
using System.Text;
using Commons.Enums;
using WorldServer.Emu.Networking.Handling.Frames.Send;

/**
* Author: InCube
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class RpChat : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var chatType = (ChatType) reader.ReadByte();
                    // Check if chat packet is valid
                    if (!Enum.IsDefined(typeof (ChatType), chatType))
                        return;

                    // Get string
                    var message = Encoding.ASCII.GetString(data.Skip(4).ToArray()).Replace("\0", "");
                    new SpChat(message, client.Account.FamilyName, chatType).Send(client);
                    //Log.Debug("Chat type: {0}, Text: {1}", chatType, message);
                }
            }
        }
    }
}
