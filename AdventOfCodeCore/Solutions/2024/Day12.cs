using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var regions = ToRegions(input);

            return regions.Sum(it => it.Count * it.Sum(it2 => it2.GetSurrounding4().Count(it3 => !it.Contains(it3))));
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var regions = ToRegions(input);

            var output = 0;

            foreach(var region in regions)
            {
                var boundary = region
                    .SelectMany(it => ((Orientation[])Enum.GetValues(typeof(Orientation))).Select(it2 => new { pos = it, dir = it2 }))
                    .Where(it => !region.Contains(it.pos.MoveOrient(it.dir)))
                    .ToHashSet();

                var sides = 0;
                while (boundary.Any())
                {
                    var start = boundary.Where(it => it.dir == Orientation.North).OrderBy(it => it.pos.X).First();
                    var current = start;

                    do
                    {
                        var next = new { pos = current.pos.MoveOrient(current.dir.RotateClockwise()), current.dir };
                        if (boundary.Contains(next))
                        {
                            current = next;
                            boundary.Remove(current);
                            continue;
                        }

                        next = new { pos = current.pos.MoveOrient(current.dir).MoveOrient(current.dir.RotateClockwise()), dir = current.dir.RotateAntiClock() };
                        if (boundary.Contains(next))
                        {
                            current = next;
                            boundary.Remove(current);
                            sides++;
                            continue;
                        }

                        next = new { current.pos, dir = current.dir.RotateClockwise() };
                        if (boundary.Contains(next))
                        {
                            current = next;
                            boundary.Remove(current);
                            sides++;
                        }
                        else
                            throw new Exception("Failed to find next point");
                    } while (!current.Equals(start));

                    boundary.Remove(current);
                }

                output += (region.Count * sides);
            }

            return output;
        }

        private List<HashSet<Point>> ToRegions(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var ungroupedPoints = new Dictionary<Point, char>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                    ungroupedPoints.Add(new Point(x, y), line[x]);
            }

            var output = new List<HashSet<Point>>();

            while (ungroupedPoints.Any())
            {
                var startPoint = ungroupedPoints.First();
                ungroupedPoints.Remove(startPoint.Key);
                var boundary = new Queue<Point>();
                boundary.Enqueue(startPoint.Key);
                var region = new HashSet<Point>();

                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    region.Add(current);
                    var surrounding = current.GetSurrounding4().Where(it => ungroupedPoints.ContainsKey(it) && ungroupedPoints[it] == startPoint.Value);

                    foreach (var item in surrounding)
                    {
                        ungroupedPoints.Remove(item);
                        boundary.Enqueue(item);
                    }
                }

                output.Add(region);
            }

            return output;
        }
    }
}
