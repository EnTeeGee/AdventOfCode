using System.Diagnostics.CodeAnalysis;

namespace AdventOfCodeCore.Common
{
    internal struct Point
    {
        public static Point Origin { get { return new Point(0, 0); } }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long X { get; }

        public long Y { get; set; }

        public Point MoveNorth(long distance = 1)
        {
            return new Point(X, Y - distance);
        }

        public Point MoveEast(long distance = 1)
        {
            return new Point(X + distance, Y);
        }

        public Point MoveOrient(Orientation orient, long distance = 1)
        {
            switch (orient)
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

        public Point[] GetSurrounding4()
        {
            return new[]
            {
                new Point(X, Y - 1),
                new Point(X + 1, Y),
                new Point(X, Y + 1),
                new Point(X - 1, Y)
            };
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

        public Point RotateClockwise()
        {
            return new Point(-Y, X);
        }

        public Point RotateCounterClock()
        {
            return new Point(Y, -X);
        }

        public long GetTaxiCabDistanceTo(Point target)
        {
            return Math.Abs(X - target.X) + Math.Abs(Y - target.Y);
        }

        public bool WithinBounds(long xMin, long xMax, long yMin, long yMax)
        {
            return X >= xMin && X <= xMax && Y >= yMin && Y <= yMax;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Point))
                return false;

            var cast = (Point)obj;

            return cast.X == X && cast.Y == Y;
        }

        public override int GetHashCode()
        {
            return (int)(X ^ Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static bool operator == (Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator != (Point a, Point b)
        {
            return !(a == b);
        }
    }
}
