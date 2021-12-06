using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day06
    {
        [Solution(6, 1)]
        public long Solution1(string input)
        {
            return RunFor(input, 80);
        }

        [Solution(6, 2)]
        public long Solution2(string input)
        {
            return RunFor(input, 256);
        }

        private long RunFor(string input, int days)
        {
            var fish = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).Aggregate(new long[9], (acc, i) => { acc[i] += 1; return acc; });

            for (var i = 0; i < days; i++)
            {
                var newFish = fish.Skip(1).Concat(new long[] { fish[0] }).ToArray();
                newFish[6] += fish[0];
                fish = newFish;
            }

            return fish.Sum();
        }
    }
}
