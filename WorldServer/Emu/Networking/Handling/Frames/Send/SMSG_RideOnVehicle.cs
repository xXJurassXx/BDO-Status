/*
   Author: RBW
*/
using Commons.Utils;
using System.IO;
using WorldServer.Emu.Models.Creature.Player;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_RideOnVehicle : APacketProcessor
    {
		private readonly Player _character;

		public SMSG_RideOnVehicle(Player character)
		{
			_character = character;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((int)_character.GameSessionId);
				writer.Write((float)_character.Position.Point.X);
				writer.Write((float)_character.Position.Point.Y);
				writer.Write((float)_character.Position.Point.Z);
				writer.Write("00000000000000000000000000FCFFFF00".ToBytes());

				return stream.ToArray();
			}
		}
    }
}
