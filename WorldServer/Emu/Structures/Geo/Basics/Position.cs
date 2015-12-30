using System;
using SharpDX;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Structures.Geo.Basics
{
    public class Position
    {
        public float Cosinus;
        public float Sinus;

        public short Heading;

        public Vector3 Point;

        public Position(Vector3 point)
        {
            Point = point;           
        }

        public double Distance(Position target)
        {
            return Point.Distance(new Vector3(target.Point.X, target.Point.Y, target.Point.Z));
        }

        public void UpdateHeading(Position target)
        {
            Heading = (short)(Math.Atan2(target.Point.Y - Point.Y, target.Point.X - Point.X) * 32768 / Math.PI);       
        }
    }
}
