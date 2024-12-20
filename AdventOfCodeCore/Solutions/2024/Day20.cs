using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day20
    {
        [Solution(20, 1)]
        public int Solution1(string input)
        {
            var (start, end, steps) = GetMap(input);

            var cheatMap = ((Orientation[])Enum.GetValues(typeof(Orientation))).Select(it => Point.Origin.MoveOrient(it, 2)).ToArray();

            return steps.Keys
                .SelectMany(it => GetCheats(it, steps, cheatMap))
                .GroupBy(it => it)
                .Where(it => it.Key >= 100)
                .Sum(it => it.Count());
        }

        [Solution(20, 2)]
        public int Solution2(string input)
        {
            var (start, end, steps) = GetMap(input);

            return steps.Keys
                .SelectMany(it => GetCheatsV2(it, steps, 20, start, end))
                .GroupBy(it => it)
                .Where(it => it.Key >= 100)
                .Sum(it => it.Count());
        }

        private (Point, Point, Dictionary<Point, int>) GetMap(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var start = Point.Origin;
            var end = Point.Origin;
            var path = new HashSet<Point>();

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var symbol = lines[y][x];
                    if (symbol == '#')
                        continue;
                    path.Add(new Point(x, y));
                    if (symbol == 'S')
                        start = new Point(x, y);
                    else if (symbol == 'E')
                        end = new Point(x, y);
                }
            }

            var steps = new Dictionary<Point, int> { { start, 0 } };
            var current = start;
            while (current != end)
            {
                var next = current.GetSurrounding4().Where(it => path.Contains(it) && !steps.ContainsKey(it)).First();
                steps.Add(next, steps[current] + 1);
                current = next;
            }

            return (start, end, steps);
        }

        private int[] GetCheats(Point pos, Dictionary<Point, int> steps, Point[] cheatMap)
        {
            return cheatMap
                .Select(it => it + pos)
                .Where(it => steps.ContainsKey(it) && steps[it] > steps[pos] + it.GetTaxiCabDistanceTo(pos))
                .Select(it => steps[it] - steps[pos] - (int)it.GetTaxiCabDistanceTo(pos))
                .ToArray();
        }

        private int[] GetCheatsV2(Point pos, Dictionary<Point, int> steps, int cheatSteps, Point start, Point end)
        {
            var valid = new Dictionary<Point, int> { { pos, 0 } };
            for (var i = 0; i < cheatSteps - 1; i++)
            {
                var step = valid.Keys.SelectMany(it => it.GetSurrounding4()).Where(it => !valid.ContainsKey(it)).Distinct().ToArray();
                foreach (var item in step)
                    valid.Add(item, i + 1);
            }
            valid.Remove(pos);

            return valid.Keys
                .Where(it => it != end && it != start)
                .SelectMany(it => it.GetSurrounding4().Select(it2 => new { from = it, to = it2 }))
                .DistinctBy(it => it.to)
                .Where(it => steps.ContainsKey(it.to) && steps[it.to] - 49 > steps[pos] - (valid[it.from] + 1))
                .Select(it => steps[it.to] - steps[pos] - (valid[it.from] + 1))
                .ToArray();
        }
    }
}
