namespace AdventOfCodeCore.Common
{
    internal static class Factor
    {
        public static int[] GetFactors(int value)
        {
            if (value == 1)
                return new[] { 1 };

            var output = new List<int>();
            var target = (int)Math.Sqrt(value);

            for (var i = 1; i <= target; i++)
            {
                if (value % i == 0)
                {
                    output.Add(i);
                    var inverse = value / i;
                    if (inverse != target)
                        output.Add(inverse);
                }

            }

            return output.ToArray();
        }

        public static long GCD(long first, long second)
        {
            var max = Math.Max(first, second);
            var min = Math.Min(first, second);
            var remainder = max % min;
            if (remainder == 0)
                return min;

            return GCD(min, remainder);
        }

        public static long LCM(int first, int second)
        {
            return (first * second) / GCD(first, second);
        }
    }
}
