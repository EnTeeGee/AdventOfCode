using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day09
    {
        [Solution(9, 1)]
        public long Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Select(it2 => long.Parse(it2)).ToArray());

            return lines.Sum(it => GetNextValue(it));
        }

        [Solution(9, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it).Select(it2 => long.Parse(it2)).ToArray());

            return lines.Sum(it => GetPreviousValue(it));
        }

        private long GetNextValue(long[] values)
        {
            var difs = values.Zip(values.Skip(1), (a, b) => b - a).ToArray();

            if (difs.Distinct().Count() == 1)
                return values.Last() + difs[0];

            return values.Last() + GetNextValue(difs);
        }

        private long GetPreviousValue(long[] values)
        {
            var difs = values.Zip(values.Skip(1), (a, b) => b - a).ToArray();

            if (difs.Distinct().Count() == 1)
                return values[0] - difs[0];

            return values[0] - GetPreviousValue(difs);
        }
    }
}
