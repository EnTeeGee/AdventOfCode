using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var blocks = new HashSet<Point>();
            var pos = Point.Origin;
            var orient = Orientation.North;
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);

            for(var y = 0; y < lines.Length; y++)
            {
                for(var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                        blocks.Add(new Point(x, y));
                    else if (lines[y][x] == '^')
                        pos = new Point(x, y);
                }
            }

            var seen = new HashSet<Point>();
            while(pos.WithinBounds(0, bounds.X, 0, bounds.Y))
            {
                if (!seen.Contains(pos))
                    seen.Add(pos);

                var next = pos.MoveOrient(orient);
                if (!blocks.Contains(next))
                    pos = next;
                else
                    orient = orient.RotateClockwise();
            }

            return seen.Count;
        }

        [Solution(6, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var blocks = new HashSet<Point>();
            var pos = Point.Origin;
            var orient = Orientation.North;
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                        blocks.Add(new Point(x, y));
                    else if (lines[y][x] == '^')
                        pos = new Point(x, y);
                }
            }

            var seen = new HashSet<Point>();
            var validBlocks = new HashSet<Point>();
            while(true)
            {
                if (!seen.Contains(pos))
                    seen.Add(pos);

                var next = pos.MoveOrient(orient);
                if (!next.WithinBounds(0, bounds.X, 0, bounds.Y))
                    return validBlocks.Count;

                if (!blocks.Contains(next))
                {
                    if (!seen.Contains(next))
                    {
                        var testCase = blocks.ToHashSet();
                        testCase.Add(next);
                        if (!validBlocks.Contains(next) && LeadsToLoop(testCase, pos, orient, bounds))
                            validBlocks.Add(next);
                    }

                    pos = next;
                }
                else
                    orient = orient.RotateClockwise();
            }
        }

        private bool LeadsToLoop(HashSet<Point> blocks, Point pos, Orientation orient, Point bounds)
        {
            var seen = new Dictionary<Orientation, HashSet<Point>>()
            {
                { Orientation.North, new HashSet<Point>() },
                { Orientation.East, new HashSet<Point>() },
                { Orientation.South, new HashSet<Point>() },
                { Orientation.West, new HashSet<Point>() },
            };

            while (true)
            {
                if (seen[orient].Contains(pos))
                    return true;

                seen[orient].Add(pos);

                var next = pos.MoveOrient(orient);
                if (!next.WithinBounds(0, bounds.X, 0, bounds.Y))
                    return false;

                if (!blocks.Contains(next))
                    pos = next;
                else
                    orient = orient.RotateClockwise();
            }
        }
    }
}
