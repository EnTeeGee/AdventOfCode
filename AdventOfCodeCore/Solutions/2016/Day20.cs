using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day20
    {
        [Solution(20, 1)]
        public uint Solution1(string input)
        {
            var ranges = Parser.ToArrayOf(input, it => Parser.SplitOn(it, '-'))
                .Select(it => new { start = uint.Parse(it[0]), end = uint.Parse(it[1]) })
                .OrderBy(it => it.start)
                .ToArray();

            var lowest = 0U;
            foreach(var item in ranges)
            {
                if (item.start <= lowest)
                    lowest = Math.Max(lowest, item.end + 1);
                else
                    return lowest;
            }

            return lowest;
        }

        [Solution(20, 2)]
        public ulong Solution2(string input)
        {
            var ranges = Parser.ToArrayOf(input, it => Parser.SplitOn(it, '-'))
                .Select(it => new { start = uint.Parse(it[0]), end = uint.Parse(it[1]) })
                .OrderBy(it => it.start)
                .ToArray();

            var lowest = 0UL;
            var sum = 0UL;
            foreach(var item in ranges)
            {
                if(item.start > lowest)
                    sum += (item.start - lowest);

                lowest = Math.Max(lowest, item.end + 1UL);   
            }
            lowest--;

            return sum + (uint.MaxValue - lowest);
        }
    }
}
