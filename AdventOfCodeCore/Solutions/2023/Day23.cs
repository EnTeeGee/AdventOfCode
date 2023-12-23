using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            return Run(input, true);
        }

        [Solution(23, 2)]
        public int Solution2(string input)
        {
            return Run(input, false);
        }

        public int Run(string input, bool filterSlopes)
        {
            var lines = Parser.ToArrayOfString(input);
            var spaces = new HashSet<Point>();
            var slopes = new Dictionary<Point, Orientation>();
            var slopeDict = new Dictionary<char, Orientation> {
                { '^', Orientation.North },
                { '>', Orientation.East },
                { 'v', Orientation.South },
                { '<', Orientation.West } };
            var start = Point.Origin;
            var end = Point.Origin;

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        continue;

                    if (y == 0)
                        start = new Point(x, y);
                    else if (y == lines.Length - 1)
                        end = new Point(x, y);
                    else if (slopeDict.ContainsKey(line[x]))
                        slopes.Add(new Point(x, y), slopeDict[line[x]]);

                    spaces.Add(new Point(x, y));
                }
            }

            var junctions = slopes.Keys
                .SelectMany(it => it.GetSurrounding4())
                .Distinct()
                .Where(it => spaces.Contains(it))
                .Where(it => it.GetSurrounding4().Where(p => spaces.Contains(p)).All(p => slopes.ContainsKey(p)))
                .ToArray();

            var ends = junctions.Concat(new[] { start, end }).ToHashSet();
            var paths = junctions.Concat(new[] { start })
                .SelectMany(it => RouteFrom(it, ends, spaces, slopes, filterSlopes))
                .ToArray();
            var pathsDict = junctions.Concat(new[] { start })
                .ToDictionary(it => it, it => paths.Where(p => p.Start.Equals(it))
                .ToArray());
            var result = LongestDistanceToEnd(start, 0, end, pathsDict, new HashSet<Point>());

            if (result == null)
                throw new Exception("Found no path");

            return result.Value;
        }

        private Path[] RouteFrom(Point start, HashSet<Point> ends, HashSet<Point> spaces, Dictionary<Point, Orientation> slopes, bool filterSlopes)
        {
            var startingPaths = Array.Empty<Point>();
            if (start.Y == 0)
                startingPaths = new[] { start.MoveNorth(-1) };
            else if (filterSlopes)
            {
                startingPaths = slopes.Values
                .Where(it => slopes.ContainsKey(start.MoveOrient(it)) && slopes[start.MoveOrient(it)] == it)
                .Distinct()
                .Select(it => start.MoveOrient(it)).ToArray();
            }
            else
            {
                startingPaths = start.GetSurrounding4()
                    .Where(it => spaces.Contains(it))
                    .ToArray();
            }
            var output = new List<Path>();

            foreach(var item in startingPaths)
            {
                var covered = new HashSet<Point> { start, item };
                var current = item;

                while (!ends.Contains(current))
                {
                    current = current.GetSurrounding4().Single(it => spaces.Contains(it) && !covered.Contains(it));
                    covered.Add(current);
                }

                output.Add(new Path(start, current, covered.Count - 1));
            }

            return output.ToArray();
        }

        private int? LongestDistanceToEnd(Point start, int distance, Point end, Dictionary<Point, Path[]> paths, HashSet<Point> seen)
        {
            if (start.Equals(end))
                return distance;

            var targets = paths[start].Where(it => !seen.Contains(it.End)).ToArray();
            if (targets.Length == 0)
                return null;

            var nextSeen = new HashSet<Point>(seen.Concat(new[] { start }));
            var results = targets.Select(it => LongestDistanceToEnd(it.End, distance + it.Length, end, paths, nextSeen))
                .Where(it => it != null)
                .ToArray();

            if (results.Length == 0)
                return null;

            return results.Max();
        }

        private class Path
        {
            public Point Start { get; }
            public Point End { get; }
            public int Length { get; }

            public Path(Point start, Point end, int length)
            {
                Start = start;
                End = end;
                Length = length;
            }
        }
    }
}
