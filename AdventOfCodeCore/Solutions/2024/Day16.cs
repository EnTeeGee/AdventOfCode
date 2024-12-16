using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            (var path, var start, var end) = GetMap(input);

            var boundary = new PriorityQueueHeap<(Point pos, Orientation dir)>();
            var seen = new HashSet<(Point, Orientation)>();

            boundary.Add((start, Orientation.East), 0);

            while (boundary.Any())
            {
                var current = boundary.PopMaxWithPriority();
                if (seen.Contains(current.value))
                    continue;

                seen.Add(current.value);

                if (current.value.pos == end)
                    return -current.priority;

                if (path.Contains(current.value.pos.MoveOrient(current.value.dir)))
                    boundary.Add((current.value.pos.MoveOrient(current.value.dir), current.value.dir), current.priority - 1);

                boundary.Add((current.value.pos, current.value.dir.RotateClockwise()), current.priority - 1000);
                boundary.Add((current.value.pos, current.value.dir.RotateAntiClock()), current.priority - 1000);
            }

            throw new Exception("Failed to find path");
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            (var path, var start, var end) = GetMap(input);

            var boundary = new PriorityQueueHeap<(Point pos, Orientation dir, Point[] path)>();
            var seen = new Dictionary<(Point, Orientation), int>();
            var bestPaths = new HashSet<Point>();
            var endScore = (int?)null;

            boundary.Add((start, Orientation.East, new[] { start } ), 0);

            while (boundary.Any())
            {
                var current = boundary.PopMaxWithPriority();
                var currentKey = (current.value.pos, current.value.dir);
                if (endScore != null && endScore > current.priority)
                    break;

                if (seen.ContainsKey(currentKey) && seen[currentKey] > current.priority)
                    continue;

                if(!seen.ContainsKey(currentKey))
                    seen.Add(currentKey, current.priority);

                if (current.value.pos == end)
                {
                    endScore = current.priority;
                    foreach (var item in current.value.path)
                        bestPaths.Add(item);

                    continue;
                }

                if (path.Contains(current.value.pos.MoveOrient(current.value.dir)))
                    boundary.Add(
                            (current.value.pos.MoveOrient(current.value.dir),
                            current.value.dir,
                            current.value.path.Concat(new []{ current.value.pos.MoveOrient(current.value.dir) }).ToArray()),
                        current.priority - 1);

                boundary.Add((current.value.pos, current.value.dir.RotateClockwise(), current.value.path), current.priority - 1000);
                boundary.Add((current.value.pos, current.value.dir.RotateAntiClock(), current.value.path), current.priority - 1000);
            }

            return bestPaths.Count;
        }

        private (HashSet<Point>, Point, Point) GetMap(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var path = new HashSet<Point>();
            var start = Point.Origin;
            var end = Point.Origin;

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

            return (path, start, end);
        }
    }
}
