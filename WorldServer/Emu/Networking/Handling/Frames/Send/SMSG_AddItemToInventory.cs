using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_AddItemToInventory : APacketProcessor
    {
        private readonly Player _player;
		private readonly short _type;

        public SMSG_AddItemToInventory(Player player, short type)
        {
            _player = player;
			_type = type;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var inventory = _player.Inventory;
                
                writer.Write((byte)1);
                writer.Write((byte)1);
                writer.Write((int)_player.GameSessionId);
				writer.Write((int)0);
				writer.Write((int)0);
				writer.Write((int)0);

				if (_type == 0)
				{
					writer.Write((short)inventory.Items.Count);
					for (short i = 0; i < inventory.Items.Count; i++)
					{
						var item = inventory.Items[(short)(i + 1)];

						writer.Write((byte)0);
						writer.Write((byte)i); // slot?
						writer.Write((short)item.ItemId);
						writer.Write((short)0); // enchant
						writer.Write((long)item.Count);
						writer.Write((long)-1); // time
						writer.Write((byte)0);
						writer.Write((long)0);
						writer.Write((short)1);
						writer.Write("FF7F".ToBytes()); // endurance
						writer.Write("FF7F".ToBytes()); // endurance
						writer.Write("3A38E56FF2862300".ToBytes()); // uid
						writer.Write("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000000000000000".ToBytes()); // dyes + jewels
					}
				}
				else
				{
					writer.Write((short)0);
				}
				
                return stream.ToArray();
            }
        }
    }
}
