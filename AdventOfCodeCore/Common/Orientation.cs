namespace AdventOfCodeCore.Common
{
    internal enum Orientation
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    static class OrientationExtensions
    {
        public static Orientation RotateClockwise(this Orientation start)
        {
            return (Orientation)((((int)start) + 1) % 4);
        }

        public static Orientation RotateAntiClock(this Orientation start)
        {
            return (Orientation)((((int)start) + 3) % 4);
        }

        public static Orientation Reverse(this Orientation start)
        {
            return (Orientation)((((int)start) + 2) % 4);
        }
    }
}
