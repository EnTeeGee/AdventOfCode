using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            var points = GetPoints(input);

            return points[0].Sum(it => GetPossibleRuns(it)
                .Count(it2 => points[1].Contains(it2[0]) && points[2].Contains(it2[1]) && points[3].Contains(it2[2])));
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var points = GetPoints(input);

            return points[2].Sum(it => GetPossibleRunsV2(it)
                .Count(it2 => points[1].Contains(it2[0]) && points[3].Contains(it2[1]) && points[1].Contains(it2[2]) && points[3].Contains(it2[3])));
        }

        private HashSet<Point>[] GetPoints(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = new[] { new HashSet<Point>(), new HashSet<Point>(), new HashSet<Point>(), new HashSet<Point>() };

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == 'X')
                        points[0].Add(new Point(x, y));
                    else if (lines[y][x] == 'M')
                        points[1].Add(new Point(x, y));
                    else if (lines[y][x] == 'A')
                        points[2].Add(new Point(x, y));
                    else
                        points[3].Add(new Point(x, y));
                }
            }

            return points;
        }

        private Point[][] GetPossibleRuns(Point origin)
        {
            return new[]
            {
                new Point(-1, -1),
                new Point(0, -1),
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),
                new Point(-1, 1),
                new Point(-1, 0),
            }.Select(it => new[] { 1, 2, 3 }.Select(it2 => new Point(origin.X + (it.X * it2), origin.Y + (it.Y * it2))).ToArray()).ToArray();
        }

        private Point[][] GetPossibleRunsV2(Point origin)
        {
            var points = new[]
            {
                new Point(-1, -1),
                new Point(1, 1),
                new Point(-1, 1),
                new Point(1, -1),
            };

            return new[] {
                points.Select(it => new Point(it.X + origin.X, it.Y + origin.Y)).ToArray(),
                points.Select(it => it.RotateClockwise()).Select(it => new Point(it.X + origin.X, it.Y + origin.Y)).ToArray(),
                points.Select(it => it.RotateClockwise().RotateClockwise()).Select(it => new Point(it.X + origin.X, it.Y + origin.Y)).ToArray(),
                points.Select(it => it.RotateCounterClock()).Select(it => new Point(it.X + origin.X, it.Y + origin.Y)).ToArray()
            };
        }
    }
}
