using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day05
    {
        [Solution(5, 1)]
        public int Solution1(string input)
        {
            return Parser
                .ToArrayOf(input, it => new PointPair(it))
                .Where(it => !it.IsDiag)
                .SelectMany(it => it.All)
                .GroupBy(it => it)
                .Count(it => it.Count() > 1);
        }

        [Solution(5, 2)]
        public int Solution2(string input)
        {
            return Parser
                .ToArrayOf(input, it => new PointPair(it))
                .SelectMany(it => it.All)
                .GroupBy(it => it)
                .Count(it => it.Count() > 1);
        }

        private class PointPair
        {
            public Point[] All { get; }
            public bool IsDiag { get; }

            public PointPair(string input)
            {
                var splits = Parser.SplitOn(input, ' ', '-', '>', ',');
                var start = new Point(int.Parse(splits[0]), int.Parse(splits[1]));
                var end = new Point(int.Parse(splits[2]), int.Parse(splits[3]));

                var diff = Math.Max((int)Math.Abs(start.X - end.X), (int)Math.Abs(start.Y - end.Y));
                var stepX = start.X < end.X ? 1 : start.X == end.X ? 0 : -1;
                var stepY = start.Y < end.Y ? 1 : start.Y == end.Y ? 0 : -1;
                All = Enumerable.Range(0, diff + 1).Select(it => new Point(start.X + (it * stepX), start.Y + (it * stepY))).ToArray();
                IsDiag = start.X != end.X && start.Y != end.Y;
            }
        }
    }
}
