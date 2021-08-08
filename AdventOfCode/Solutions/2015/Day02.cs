using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day02
    {
        [Solution(2, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return lines.Select(it => it.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(it => GetWrappingArea(int.Parse(it[0]), int.Parse(it[1]), int.Parse(it[2])))
                .Sum();
        }

        [Solution(2, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return lines.Select(it => it.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(it => it.Select(n => int.Parse(n)).ToArray())
                .Select(it => GetBowLength(it))
                .Sum();
        }

        private int GetWrappingArea(int x, int y, int z)
        {
            var area1 = x * y;
            var area2 = x * z;
            var area3 = y * z;

            return (2 * area1) +
                (2 * area2) +
                (2 * area3) +
                Math.Min(Math.Min(area1, area2), area3);
        }

        private int GetBowLength(int[] values)
        {
            var lower2 = values.OrderBy(it => it).Take(2).ToArray();

            return (lower2[0] * 2) + (lower2[1] * 2) + (values[0] * values[1] * values[2]);
        }
    }
}
