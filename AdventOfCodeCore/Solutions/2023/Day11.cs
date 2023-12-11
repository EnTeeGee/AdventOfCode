using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day11
    {
        [Solution(11, 1)]
        public long Solution1(string input)
        {
            var info = ParseInfo(input);

            return info.pairs.Sum(it => CaculateShortest(it, info.xGaps, info.yGaps));
        }

        [Solution(11, 2)]
        public long Solution2(string input)
        {
            var info = ParseInfo(input);
            var multiple = 1.Million();

            return info.pairs.Sum(it => CaculateShortest(it, info.xGaps, info.yGaps, multiple));
        }

        private (Point[][] pairs, int[] xGaps, int[] yGaps) ParseInfo(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = new HashSet<Point>();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        points.Add(new Point(x, y));
                }
            }

            var maxX = (int)points.Max(it => it.X);
            var maxY = (int)points.Max(it => it.Y);
            var xGaps = Enumerable.Range(0, maxX).Where(it => !points.Any(p => p.X == it)).ToArray();
            var yGaps = Enumerable.Range(0, maxY).Where(it => !points.Any(p => p.Y == it)).ToArray();
            var pairs = Permutations.GetAllPossiblePairs(points.ToArray());

            return (pairs, xGaps, yGaps);
        }

        private long CaculateShortest(Point[] points, int[] xGaps, int[] yGaps, int multiple = 2)
        {
            var x = (xGaps.Count(it => it < points.Max(p => p.X) && it > points.Min(p => p.X)) * (multiple - 1)) + Math.Abs(points[0].X - points[1].X);
            var y = (yGaps.Count(it => it < points.Max(p => p.Y) && it > points.Min(p => p.Y)) * (multiple - 1)) + Math.Abs(points[0].Y - points[1].Y);

            return x + y;
        }
    }
}
