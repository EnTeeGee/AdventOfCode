using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day07
    {
        [Solution(7, 1)]
        public long Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ':').Select(it => long.Parse(it)).ToArray());

            return lines.Where(it => IsValid(it[0], it.Skip(1).ToArray())).Sum(it => it[0]);
        }

        [Solution(7, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ':').Select(it => long.Parse(it)).ToArray());

            return lines.Where(it => IsValid(it[0], it.Skip(1).ToArray(), true)).Sum(it => it[0]);
        }

        private bool IsValid(long target, long[] values, bool canConcat = false)
        {
            var heads = canConcat ? new[]
            {
                values[0] + values[1],
                values[0] * values[1],
                long.Parse(values[0].ToString() + values[1].ToString())
            } : new[]
            {
                values[0] + values[1],
                values[0] * values[1]
            };

            if (heads.All(it => it > target))
                return false;

            if (values.Length == 2)
                return heads.Any(it => it == target);

            return heads.Any(it => IsValid(target, new[] { it }.Concat(values.Skip(2)).ToArray(), canConcat));
        }
    }
}
