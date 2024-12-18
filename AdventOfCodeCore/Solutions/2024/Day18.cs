using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day18
    {
        [Solution(18, 1)]
        public int Solution1(string input)
        {
            var points = Parser
                .ToArrayOf(input, it => Parser
                    .SplitOn(it, ',')
                    .Select(it => int.Parse(it))
                    .ToArray())
                .Select(it => new Point(it[0], it[1]))
                .Take(1024)
                .ToHashSet();

            return RunFor(points)!.Value;
        }

        [Solution(18, 2)]
        public string Solution2(string input)
        {
            var points = Parser
                .ToArrayOf(input, it => Parser
                    .SplitOn(it, ',')
                    .Select(it => int.Parse(it))
                    .ToArray())
                .Select(it => new Point(it[0], it[1]))
                .ToArray();

            var maxValid = 1024;
            var minInvalid = points.Length;

            while(maxValid + 1 !=  minInvalid)
            {
                var midPoint = (maxValid + minInvalid) / 2;
                var result = RunFor(points.Take(midPoint).ToHashSet());

                if(result != null)
                    maxValid = midPoint;
                else
                    minInvalid = midPoint;
            }

            var output = points[maxValid];

            return $"{output.X},{output.Y}";
        }

        private int? RunFor(HashSet<Point> points)
        {
            var target = new Point(70, 70);
            var seen = new HashSet<Point>();
            var boundary = new Queue<(Point pos, int dist)>();
            seen.Add(Point.Origin);
            boundary.Enqueue((Point.Origin, 0));

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                if (current.pos == target)
                    return current.dist;

                var next = current.pos.GetSurrounding4()
                    .Where(it => !points.Contains(it) && !seen.Contains(it) && it.WithinBounds(0, target.X, 0, target.Y))
                    .ToArray();
                foreach (var item in next)
                {
                    boundary.Enqueue((item, current.dist + 1));
                    seen.Add(item);
                }
            }

            return null;
        }
    }
}
