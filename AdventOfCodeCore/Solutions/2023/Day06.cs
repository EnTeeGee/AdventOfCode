using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Skip(1).Select(it => int.Parse(it)).ToArray());
            var pairs = lines[0].Zip(lines[1], (a, b) => new { Time = a, Dist = b }).ToArray();

            return pairs.Select(it => Enumerable
                    .Range(1, it.Time - 2)
                    .Select(it2 => it2 * (it.Time - it2))
                    .Count(it2 => it2 > it.Dist))
                .Aggregate(1, (it, acc) => it * acc);
        }

        [Solution(6, 2)]
        public long Solution2(string input)
        {
            var values = Parser.ToArrayOf(input, it => long.Parse(new string(it.Skip(11).Where(it => it != ' ').ToArray())));
            var invalid = Enumerable.Range(0, int.MaxValue)
                .Select(it => it * (values[0] - it))
                .TakeWhile(it => it <= values[1])
                .Count() * 2;

            return (values[0] + 1) - invalid;
        }
    }
}
