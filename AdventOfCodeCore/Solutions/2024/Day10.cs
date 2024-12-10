using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var (points, trailheads) = GetMap(input);

            var output = 0;
            foreach (var item in trailheads)
            {
                var valid = new Point[] { item };

                for(var i = 1; i <= 9; i++)
                {
                    valid = valid.SelectMany(it => it.GetSurrounding4())
                        .Where(it => points.ContainsKey(it) && points[it] == i)
                        .Distinct()
                        .ToArray();
                }

                output += valid.Length;
            }

            return output;
        }

        [Solution(10, 2)]
        public int Solution2(string input)
        {
            var (points, trailheads) = GetMap(input);

            var output = 0;
            foreach (var item in trailheads)
            {
                var valid = new Point[] { item };

                for (var i = 1; i <= 9; i++)
                {
                    valid = valid.SelectMany(it => it.GetSurrounding4())
                        .Where(it => points.ContainsKey(it) && points[it] == i)
                        .ToArray();
                }

                output += valid.Length;
            }

            return output;
        }

        private (Dictionary<Point, int> points, List<Point> trailheads) GetMap(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var points = new Dictionary<Point, int>();
            var trailheads = new List<Point>();

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if ("0123456789".Contains(lines[y][x]))
                    {
                        points.Add(new Point(x, y), int.Parse(lines[y][x].ToString()));

                        if (lines[y][x] == '0')
                            trailheads.Add(new Point(x, y));
                    }
                }
            }

            return (points, trailheads);
        }
    }
}
