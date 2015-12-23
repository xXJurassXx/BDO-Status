using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpInventory : APacketProcessor
    {
        private readonly Player _player;

        public SpInventory(Player player)
        {
            _player = player;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var inventory = _player.Inventory;
                
                writer.WriteC(1);
                writer.WriteC(1);
                writer.WriteD(_player.GameSessionId);
                writer.Skip(12);
                writer.WriteH(inventory.Items.Count);
                writer.WriteC(0);

                for (short i = 0; i < inventory.Items.Count; i++)
                {
                    var item = inventory.Items[(short) (i + 1)];
                   
                    writer.WriteC(i);
                    writer.WriteD(item.ItemId);
                    writer.WriteQ(item.Count);
                    writer.Write("FFFFFFFFFFFFFFFF".ToBytes());
                    writer.WriteD(i == 0 ? 0 : 1);
                    writer.WriteD(0);
                    writer.WriteC(0);
                    writer.Write("0100FF7FFF7F3A38E56FF2862300FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000000000000000".ToBytes());
                }

                return stream.ToArray();
            }
        }
    }
}
