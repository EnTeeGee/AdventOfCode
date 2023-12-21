using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var rocks = new HashSet<Point>();
            var start = Point.Origin;
            for(var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for(var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        rocks.Add(new Point(x, y));
                    else if (line[x] == 'S')
                        start = new Point(x, y);
                }
            }
            var bounds = new Point(lines.Last().Length - 1, lines.Length - 1);
            var boundary = new HashSet<Point>() { start };
            for(var i = 0; i < 64; i++)
            {
                var current = boundary.ToArray();
                boundary.Clear();

                foreach(var point in current)
                {
                    var next = point.GetSurrounding4()
                        .Where(it => !rocks.Contains(it) && !boundary.Contains(it) && it.WithinBounds(0, bounds.X, 0, bounds.Y))
                        .ToArray();
                    foreach (var nextPoint in next)
                        boundary.Add(nextPoint);
                }
            }

            return boundary.Count;
        }

        [Solution(21, 2)]
        public long Solution2(string input)
        {
            var targetSteps = 26501365;
            var lines = Parser.ToArrayOfString(input);
            var rocks = new HashSet<Point>();
            var start = Point.Origin;
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        rocks.Add(new Point(x, y));
                    else if (line[x] == 'S')
                        start = new Point(x, y);
                }
            }
            var bounds = new Point(lines.Last().Length - 1, lines.Length - 1);

            var inaccessable = rocks.SelectMany(it => it.GetSurrounding4())
                .Distinct()
                .Where(it => !rocks.Contains(it))
                .Where(it => it.GetSurrounding4().All(r => rocks.Contains(r)))
                .ToArray();

            var coveredOdd = ((lines[0].Length * lines.Length) / 2) + 1 - rocks.Count(it => (it.X + it.Y) % 2 == 0) - inaccessable.Count(it => (it.X + it.Y) % 2 == 0);
            var coveredEven = ((lines[0].Length * lines.Length) / 2) - rocks.Count(it => (it.X + it.Y) % 2 == 1) - inaccessable.Count(it => (it.X + it.Y) % 2 == 1);

            var stepSize = lines[0].Length;
            var startingStep = start.X;

            var remaining = (int)((targetSteps - startingStep) % stepSize);
            if (remaining == 0)
                remaining = stepSize;

            var cornerSteps = new[] { Orientation.North, Orientation.East, Orientation.South, Orientation.West }
            .Select(it => start.MoveOrient(it, startingStep))
            .Select(it => GetCoveredSteps(rocks, it, remaining, bounds))
            .Sum();

            var corners = new[] { new Point(0, 0), new Point(0, stepSize - 1), new Point(stepSize - 1, stepSize - 1), new Point(stepSize - 1, 0) };
            var smallCornerSteps = corners.Select(it => GetCoveredSteps(rocks, it, 65, bounds)).Sum();
            var largeCornerSteps = corners.Select(it => GetCoveredSteps(rocks, it, 131 + 65, bounds)).Sum();

            var covered = ((targetSteps - startingStep) / stepSize) - 1;
            var type1 = 0L;
            var type2 = 0L;
            if(covered % 2 != 0)
            {
                covered++;
                type1 = (covered * covered) / 4; // next to origin
                type2 = ((covered * (covered + 1)) / 2) - type1 - covered;
                covered--;
            }
            else
            {
                type1 = (covered * covered) / 4; // next to origin
                type2 = ((covered * (covered + 1)) / 2) - type1;
            }

            var total = 0L;
            if(targetSteps % 2 == 1) // origin has coveredEven
            {
                total += coveredEven;
                total += (long)coveredOdd * type1 * 4;
                total += (long)coveredEven * type2 * 4;
            }
            else
            {
                total += coveredOdd;
                total += (long)coveredEven * type1 * 4;
                total += (long)coveredOdd * type2 * 4;
            }

            total += cornerSteps;
            total += (long)smallCornerSteps * (covered + 1);
            total += (long)largeCornerSteps * covered;

            return total;
        }

        private int GetCoveredSteps(HashSet<Point> rocks, Point start, int steps, Point bounds)
        {
            var current = new HashSet<Point> { start };

            for(var i = 1; i < steps; i++)
            {
                var next = current
                    .SelectMany(it => it.GetSurrounding4())
                    .Distinct()
                    .Where(it => !rocks.Contains(it) && it.WithinBounds(0, bounds.X, 0, bounds.Y))
                    .ToHashSet();
                current = next;
            }

            return current.Count;
        }
    }
}
