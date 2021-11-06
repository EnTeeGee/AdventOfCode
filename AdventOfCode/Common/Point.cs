using System;

namespace AdventOfCode.Common
{
    class Point
    {
        public static Point Origin { get { return new Point(0, 0); } }

        public Point() { }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X { get; }

        public long Y { get; set; }

        public Point MoveNorth(long distance = 1)
        {
            return new Point(X, Y + distance);
        }

        public Point MoveEast(long distance = 1)
        {
            return new Point(X + distance, Y);
        }

        public Point MoveOrient(Orientation orient, long distance = 1)
        {
            switch(orient)
            {
                case Orientation.North:
                    return MoveNorth(distance);
                case Orientation.East:
                    return MoveEast(distance);
                case Orientation.South:
                    return MoveNorth(-distance);
                case Orientation.West:
                    return MoveEast(-distance);
            }

            return this;
        }

        public Point[] GetSurrounding8()
        {
            return new[]
            {
                new Point(X - 1, Y - 1),
                new Point(X, Y - 1),
                new Point(X + 1, Y - 1),
                new Point(X + 1, Y),
                new Point(X + 1, Y + 1),
                new Point(X, Y + 1),
                new Point(X - 1, Y + 1),
                new Point(X - 1, Y)
            };
        }

        public long GetTaxiCabDistanceTo(Point target)
        {
            return Math.Abs(X - target.X) + Math.Abs(Y - target.Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            var cast = obj as Point;

            return cast.X == X && cast.Y == Y;
        }

        public override int GetHashCode()
        {
            return (int)(X ^ Y);
        }
    }
}
