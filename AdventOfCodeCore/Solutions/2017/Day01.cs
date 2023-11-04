using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day01
    {
        [Solution(1, 1)]
        public static int Solution1(string input)
        {
            return input
                .Skip(1)
                .Concat(new[] { input[0] })
                .Zip(input, (a, b) => a == b ? int.Parse(a.ToString()) : 0)
                .Sum();
        }

        [Solution(1, 2)]
        public static int Solution2(string input)
        {
            return input
                .Skip(input.Length / 2)
                .Concat(input[..(input.Length / 2)])
                .Zip(input, (a, b) => a == b ? int.Parse(a.ToString()) : 0)
                .Sum();
        }
    }
}
