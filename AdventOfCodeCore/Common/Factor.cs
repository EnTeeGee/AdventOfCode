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

        public static long InternalGCD(long first, long second)
        {
            var max = Math.Max(first, second);
            var min = Math.Min(first, second);
            var remainder = max % min;
            if (remainder == 0)
                return min;

            return InternalGCD(min, remainder);
        }

        public static long GCD(params long[] values)
        {
            var currentGCD = Math.Abs(values[0]);
            foreach (var item in values.Skip(1))
                currentGCD = InternalGCD(currentGCD, Math.Abs(item));

            return currentGCD;
        }

        private static long InternalLCM(long first, long second)
        {
            return (first * second) / GCD(first, second);
        }

        public static long LCM(params long[] values)
        {
            var currentLCM = 1L;
            foreach(var item in values)
            {
                currentLCM = InternalLCM(currentLCM, item);
            }

            return currentLCM;
        }
    }
}
