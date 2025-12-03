using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => int.Parse(new string(GetBestJoltage(it, 2))));
        }

        [Solution(3, 2)]
        public long Solution2(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => long.Parse(new string(GetBestJoltage(it, 12))));
        }

        private char[] GetBestJoltage(string input, int remaining)
        {
            var result = input.Take(input.Length - remaining + 1).Max(it => it);
            if (remaining == 1)
                return [result];

            return [result, .. GetBestJoltage(input.Substring(input.IndexOf(result) + 1), remaining - 1)];
        }
    }
}
