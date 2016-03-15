using System;
using System.IO;
using System.Linq;
using System.Text;
using Commons.Enums;
using WorldServer.Emu.Models.Creature.Player;
/**
* Author: InCube, Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_Chat : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
					/* c,c,c,c,ss */
                    var chatType = (ChatType) reader.ReadByte();
					var chatNoticeType = reader.ReadByte();
					var unk1 = reader.ReadByte(); // -1
					var unk2 = reader.ReadByte(); // 1

					if (!Enum.IsDefined(typeof (ChatType), chatType))
                        return;

                    var message = Encoding.ASCII.GetString(data.Skip(4).ToArray()).Replace("\0", "");

                    client.ActivePlayer.Action(Player.PlayerAction.Chat, chatType, message);
                }
            }
        }
    }
}
