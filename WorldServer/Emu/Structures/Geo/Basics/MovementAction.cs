namespace WorldServer.Emu.Structures.Geo.Basics
{
    public class MovementAction
    {
        public Position StartPosition, EndPosition;
        public short Heading;      
        public short MovementType;
        public short Speed;

        public MovementAction(Position startPosition, Position endPosition, short heading, short speed, short moveType)
        {
            Heading = heading;
            StartPosition = startPosition;
            EndPosition = endPosition;
            MovementType = moveType;
            Speed = speed;
        }
    }
}
