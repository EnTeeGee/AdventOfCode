using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day02
    {
        [Solution(2, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Select(it => int.Parse(it)).ToArray())
                .Count(it => IsSafe(it));
        }

        [Solution(2, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Select(it => int.Parse(it)).ToArray())
                .Select(it => Enumerable
                    .Range(0, it.Length + 1)
                    .Select(it2 => it.Take(it2).Concat(it.Skip(it2 + 1)).ToArray())
                    .Any(it2 => IsSafe(it2)))
                .Count(it => it);
        }

        private bool IsSafe(int[] input)
        {
            var diffs = input.Zip(input.Skip(1), (a, b) => a - b).ToArray();

            return (diffs.All(it => it > 0) || diffs.All(it => it < 0)) 
                && diffs.Select(it => Math.Abs(it)).All(it => it > 0 && it <= 3);
        }
    }
}
