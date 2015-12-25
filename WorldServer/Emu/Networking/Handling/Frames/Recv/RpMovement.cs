using System;
using System.IO;
using Commons.Utils;
using SharpDX;
using WorldServer.Emu.Structures.Geo.Basics;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class RpMovement : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                int type = reader.ReadInt32();
                reader.ReadInt16();
                float start_x = reader.ReadSingle();
                float start_y = reader.ReadSingle();
                float start_z = reader.ReadSingle();              
                reader.ReadInt64();//zeros

                float x1 = reader.ReadSingle();
                float y1 = reader.ReadSingle();
                float z1 = reader.ReadSingle();

                float cosinus = reader.ReadSingle();
                float unk2 = reader.ReadSingle();
                float sinus = reader.ReadSingle();
                float unk4 = reader.ReadSingle();

                float x2 = reader.ReadSingle();
                float y2 = reader.ReadSingle();
                float z2 = reader.ReadSingle();

                float x3 = reader.ReadSingle();
                float y3 = reader.ReadSingle();
                float z3 = reader.ReadSingle();

                float x4 = reader.ReadSingle();
                float y4 = reader.ReadSingle();
                float z4 = reader.ReadSingle();

                float x5 = reader.ReadSingle();
                float y5 = reader.ReadSingle();
                float z5 = reader.ReadSingle();

                float x6 = reader.ReadSingle();
                float y6 = reader.ReadSingle();
                float z6 = reader.ReadSingle();

                float x7 = reader.ReadSingle();
                float y7 = reader.ReadSingle();
                float z7 = reader.ReadSingle();

                float x8 = reader.ReadSingle();
                float y8 = reader.ReadSingle();
                float z8 = reader.ReadSingle();

                float x9 = reader.ReadSingle();
                float y9 = reader.ReadSingle();
                float z9 = reader.ReadSingle();

                reader.Skip(94); //zeros

                var cHeading = Math.Acos(cosinus);
                var sHeading = Math.Asin(sinus);

                var heading = start_x * cosinus - start_y * sinus; 

                var movementAcion = new MovementAction(
                    new Position(new Vector3(start_x, start_y, start_z))
                {
                    Cosinus = cosinus, Sinus = sinus, Heading = (short) heading
                }, new Position(new Vector3(0,0,0)), (short) heading, 120, 1);

                Core.Act(s => s.WorldProcessor.ObjectMoved(client.ActivePlayer, movementAcion));
                //Log.Debug("\n----------------\n" +
                //          $"Type {type} cHeading {cHeading} sHeading {sHeading} calculated heading {heading}\n" +
                //          $"Cos {cosinus} Sin {sinus}\n" +
                //          $"x-{start_x} y-{start_y} z-{start_z}\n" +
                //          $"x-{x1} y-{y1} z-{z1}\n" +
                //          $"x-{x2} y-{y2} z-{z2}\n" +
                //          $"x-{x3} y-{y3} z-{z3}\n" +
                //          $"x-{x4} y-{y4} z-{z4}\n" +
                //          $"x-{x5} y-{y5} z-{z5}\n" +
                //          $"x-{x6} y-{y6} z-{z6}\n" +
                //          $"x-{x7} y-{y7} z-{z7}\n" +
                //          $"x-{x8} y-{y8} z-{z8}\n" +
                //          $"x-{x9} y-{y9} z-{z9}\n" +
                //          $"----------------");
            }
        }
    }
}
