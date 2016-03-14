using System.IO;
using Commons.Utils;
/*
   Author: Sagara, InCube, RBW
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Send
{
    // ReSharper disable once InconsistentNaming
    public class SMSG_GetContentServiceInfo : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
				/* c,c,c,d*8,c*32,d*8,c*11,h[ch] */

				writer.Write((byte)4); // gameOption
				writer.Write((byte)4); // characterOpenSlots
				writer.Write((byte)9); // characterMaxSlots

				writer.Write((int)3000000);
				writer.Write((int)0);
				writer.Write((int)300000);
				writer.Write((int)0);
				writer.Write((int)200000);
				writer.Write((int)200000);
				writer.Write((int)0);
				writer.Write((int)60000);

				/* allowed/open classes config - classId = enable else 0x20[32] to disable */
				writer.Write((byte)0); // warrior
				writer.Write((byte)4); // ranger
				writer.Write((byte)8); // sorcerer
				writer.Write((byte)12); // giant
				writer.Write((byte)16); // tamer
				writer.Write((byte)20); // blader
				writer.Write((byte)21); // plum
				writer.Write((byte)24); // valkyrie
				writer.Write((byte)25); // kunoichi
				writer.Write((byte)26); // ninja
				writer.Write((byte)28); // wizard
				writer.Write((byte)31); // witch

				// null - not allowed classes
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);
				writer.Write((byte)32);

				writer.Write((int)56); // max level
				writer.Write((int)1);
				writer.Write((int)86400);
				writer.Write((int)0);
				writer.Write((int)86400);
				writer.Write((int)0);
				writer.Write((int)86400);
				writer.Write((int)0);

				writer.Write((byte)0);
				writer.Write((byte)1);
				writer.Write((byte)0);
				writer.Write((byte)6);
				writer.Write((byte)0);
				writer.Write((byte)0);
				writer.Write((byte)150);
				writer.Write((byte)0);
				writer.Write((byte)0);
				writer.Write((byte)0);
				writer.Write((byte)0);

				writer.Write((short)50); // extra unk loop size related to server list
				// loop content: [c,h]*50
				writer.Write("040100040200040300040400040500040600040700040800040900040B00040C00040D00040E00040F00041000041100041200041300041400041500041600041700041800041900041F00042100042200042400042500042600042700042800042900042A00042B00042C00042D00042E00043D00043E00043F00044000046500046600046700046800046900046A00046B00046C00".ToBytes());

                return stream.ToArray();
            }
        }
    }
}
