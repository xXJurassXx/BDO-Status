using System;

namespace WorldServer.Emu.Structures.Geo.Basics
{
    public class Position
    {
        public float X;
        public float Y;
        public float Z;
        public float Cosinus;
        public float Sinus;

        public short Heading;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Distance(Position target)
        {
            var distance =
                      (X - target.X) * (X - target.X) +
                      (Y - target.Y) * (Y - target.Y) +
                      (Z - target.Z) * (Z - target.Z);

            return Math.Sqrt(distance);
        }

        public void UpdateHeading(Position target)
        {
            Heading = (short)(Math.Atan2(target.Y - Y, target.X - X) * 32768 / Math.PI);
        }
    }
}
