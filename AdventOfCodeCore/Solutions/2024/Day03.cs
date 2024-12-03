using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text.RegularExpressions;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            return Regex.Matches(input, "mul\\(\\d+,\\d+\\)")
                .Select(it => Parser.SplitOn(it.Value, '(', ')', ','))
                .Sum(it => int.Parse(it[1]) * int.Parse(it[2]));
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var dos = new[] { 0 }.Concat(Regex.Matches(input, "do\\(\\)").Select(it => it.Index)).ToArray();
            var donts = Regex.Matches(input, "don\\'t\\(\\)").Select(it => it.Index).Concat(new[] { input.Length }).ToArray();

            var ranges = dos
                .Select(it => new { start = it, end = donts.FirstOrDefault(it2 => it2 > it) })
                .ToArray();

            return Regex.Matches(input, "mul\\(\\d+,\\d+\\)")
                .Where(it => ranges.Any(r => r.start < it.Index && r.end > it.Index))
                .Select(it => Parser.SplitOn(it.Value, '(', ')', ','))
                .Sum(it => int.Parse(it[1]) * int.Parse(it[2]));
        }
    }
}
