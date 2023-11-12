using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var scanners = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ':').Select(x => int.Parse(x)).ToArray());

            return scanners.Aggregate(0, (acc, it) => IsHit(it[0], it[1]) ? acc + (it[0] * it[1]) : acc);
        }

        [Solution(13, 2)]
        public int Solution2(string input)
        {
            var scanners = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ':').Select(x => int.Parse(x)).ToArray());
            var delay = 0;
            while (true)
            {
                if (scanners.All(it => !IsHit(it[0] + delay, it[1])))
                    return delay;
                delay++;
            }
        }

        private bool IsHit(int step, int scannerWidth)
        {
            var freq = (scannerWidth - 1) * 2;

            return step % freq == 0;
        }
    }
}
