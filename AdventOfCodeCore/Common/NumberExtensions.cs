namespace AdventOfCodeCore.Common
{
    internal static class NumberExtensions
    {
        public static int Thousand(this int value)
        {
            return value * 1000;
        }

        public static int Million(this int value)
        {
            return value * 1000000;
        }

        public static long Billion(this int value)
        {
            return value * 1000000000L;
        }

        public static long Trillion(this int value)
        {
            return value * 1000000000000L;
        }
    }
}
