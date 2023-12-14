using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day14
    {
        [Solution(14, 1)]
        public long Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var ( spheres, walls ) = ParseData(lines);
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);

            for(var x = 0; x < lines[0].Length; x++)
            {
                for(var y = lines.Length - 1; y >= 0; y--)
                {
                    var point = new Point(x, y);
                    if (spheres.Contains(point))
                        TryRoll(point, spheres, walls, bounds);
                }
            }

            return spheres.Sum(it => lines.Length - it.Y);
        }

        [Solution(14, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var (spheres, walls) = ParseData(lines);
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);
            var seen = new Dictionary<string, int>();
            var toEnd = false;

            for(var i = 0; i < 1.Billion(); i++)
            {
                //North
                for (var x = 0; x < lines[0].Length; x++)
                {
                    for (var y = lines.Length - 1; y >= 0; y--)
                    {
                        var point = new Point(x, y);
                        if (spheres.Contains(point))
                            TryRoll(point, spheres, walls, bounds);
                    }
                }

                // West
                for(var y = 0; y < lines.Length; y++)
                {
                    for(var x = lines[y].Length - 1; x >= 0; x--)
                    {
                        var point = new Point(x, y);
                        if (spheres.Contains(point))
                            TryRoll(point, spheres, walls, bounds, Orientation.West);
                    }
                }

                // South
                for (var x = 0; x < lines[0].Length; x++)
                {
                    for (var y = 0; y < lines.Length; y++)
                    {
                        var point = new Point(x, y);
                        if (spheres.Contains(point))
                            TryRoll(point, spheres, walls, bounds, Orientation.South);
                    }
                }

                // East
                for (var y = 0; y < lines.Length; y++)
                {
                    for (var x = 0; x < lines[0].Length; x++)
                    {
                        var point = new Point(x, y);
                        if (spheres.Contains(point))
                            TryRoll(point, spheres, walls, bounds, Orientation.East);
                    }
                }

                if (toEnd)
                    continue;

                var result = string.Join(string.Empty, spheres.OrderBy(it => it.X).ThenBy(it => it.Y));
                if (seen.ContainsKey(result))
                {
                    var start = seen[result];
                    var remaining = 1.Billion() - i;
                    var loops = remaining / (i - seen[result]);
                    i += (int)(loops * (i - seen[result]));
                    toEnd = true;
                }
                else
                    seen.Add(result, i);
            }

            return spheres.Sum(it => lines.Length - it.Y);
        }

        private (HashSet<Point> spheres, HashSet<Point> walls) ParseData(string[] lines)
        {
            var spheres = new HashSet<Point>();
            var walls = new HashSet<Point>();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'O')
                        spheres.Add(new Point(x, y));
                    else if (line[x] == '#')
                        walls.Add(new Point(x, y));
                }
            }

            return (spheres, walls);
        }

        private bool TryRoll(Point point, HashSet<Point> spheres, HashSet<Point> walls, Point bounds, Orientation dir = Orientation.North)
        {
            var target = point.MoveOrient(dir);
            if (!target.WithinBounds(0, bounds.X, 0, bounds.Y))
                return false;
            if (walls.Contains(target))
                return false;
            if (spheres.Contains(target) && !TryRoll(target, spheres, walls, bounds, dir))
                return false;

            spheres.Remove(point);
            spheres.Add(target);

            return true;
        }
    }
}
