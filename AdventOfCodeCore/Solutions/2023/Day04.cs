using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ": ", " | ")
                    .Skip(1)
                    .Select(it2 => Parser.SplitOnSpace(it2).Select(it3 => int.Parse(it3)).ToArray())
                    .ToArray())
                .Select(it => it[1].Where(it2 => it[0].Contains(it2)).Count())
                .Select(it => it == 0 ? 0 : (int)Math.Pow(2, it - 1))
                .Sum();
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var results = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ": ", " | ")
                    .Skip(1)
                    .Select(it2 => Parser.SplitOnSpace(it2).Select(it3 => int.Parse(it3)).ToArray())
                    .ToArray())
                .Select(it => it[1].Where(it2 => it[0].Contains(it2)).Count())
                .ToArray();
            var counts = Enumerable.Repeat(1, results.Length).ToArray();
            for(var i = 0; i < results.Length; i++)
            {
                for (var j = 0; j < results[i]; j++)
                    counts[i + j + 1] += counts[i];
            }

            return counts.Sum();
        }
    }
}
