using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var (start, splitters) = Parse(lines);

            var beams = new[] { start };
            var output = 0;
            while (beams[0].Y < lines.Length)
            {
                output += beams.Count(it => splitters.Contains(it));
                beams = beams
                    .SelectMany(it =>
                        splitters.Contains(it) ?
                            [new Point(it.X - 1, it.Y + 2), new Point(it.X + 1, it.Y + 2)] :
                            new [] { new Point(it.X, it.Y + 2) })
                    .Distinct()
                    .ToArray();
            }

            return output;
        }

        [Solution(7, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var (start, splitters) = Parse(lines);

            return TimelinesFrom(start, lines.Length, splitters, new Dictionary<Point, long>());
        }

        private (Point start, HashSet<Point> splitters) Parse(string[] input)
        {
            var start = Point.Origin;
            var splitters = new HashSet<Point>();
            for (var y = 0; y < input.Length; y += 2)
            {
                var line = input[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'S')
                        start = (x, y);
                    else if (line[x] == '^')
                        splitters.Add((x, y));
                }
            }

            return (start, splitters);
        }

        private long TimelinesFrom(Point pos, int limit, HashSet<Point> splitters, Dictionary<Point, long> cache)
        {
            if (pos.Y >= limit)
                return 1;

            if(cache.ContainsKey(pos))
                return cache[pos];

            var result = 0L;
            if (splitters.Contains(pos))
                result = TimelinesFrom((pos.X - 1, pos.Y + 2), limit, splitters, cache)
                    + TimelinesFrom((pos.X + 1, pos.Y + 2), limit, splitters, cache);
            else
                result = TimelinesFrom((pos.X, pos.Y + 2), limit, splitters, cache);

            cache.Add(pos, result);

            return result;

        }

    }
}
