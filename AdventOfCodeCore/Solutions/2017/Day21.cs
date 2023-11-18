using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => new Mapping(it));

            return Run(5, lines, StartingPoints()).Count();
        }

        [Solution(21, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOf(input, it => new Mapping(it));

            var startingArray = StartingPoints().ToArray();
            var matching = lines.First(it => it.Matches(startingArray));

            return matching.GetForDepth(18, lines);
        }

        private static HashSet<Point> Run(int iterations, Mapping[] lines, HashSet<Point> startingPoints)
        {
            var twoMaps = lines.Where(it => it.Size == 2).ToArray();
            var threeMaps = lines.Where(it => it.Size == 3).ToArray();
            var points = startingPoints;
            var dimensions = 3;

            for (var i = 0; i < iterations; i++)
            {
                if (dimensions % 2 == 0)
                {
                    var steps = Enumerable.Range(0, dimensions / 2).Select(it => it * 2).ToArray();
                    var options = GetIndexes(steps);
                    var newPoints = new HashSet<Point>();

                    foreach (var item in options)
                    {
                        var result = MapPoints(points, twoMaps, item[0], item[1], 2);
                        foreach (var point in result)
                            newPoints.Add(point);
                    }

                    points = newPoints;
                    dimensions = (dimensions / 2) * 3;
                }
                else
                {
                    var steps = Enumerable.Range(0, dimensions / 3).Select(it => it * 3).ToArray();
                    var options = GetIndexes(steps);
                    var newPoints = new HashSet<Point>();

                    foreach (var item in options)
                    {
                        var result = MapPoints(points, threeMaps, item[0], item[1], 3);
                        foreach (var point in result)
                            newPoints.Add(point);
                    }

                    points = newPoints;
                    dimensions = (dimensions / 3) * 4;
                }
            }

            return points;
        }

        private static int[][] GetIndexes(int[] items)
        {
            return items.SelectMany(it => items.Select(x => new[] { it, x })).ToArray();
        }

        private static Point[] MapPoints(HashSet<Point> points, Mapping[] maps, int x, int y, int size)
        {
            var targetPoints = points.Where(it => it.WithinBounds(x, x + size - 1, y, y + size - 1)).Select(it => new Point(it.X - x, it.Y - y)).ToArray();
            var map = maps.First(it => it.Matches(targetPoints));
            
            return map.Result.Select(it => new Point(((x / size) * (size + 1)) + it.X, ((y / size) * (size + 1)) + it.Y)).ToArray();
        }

        private static HashSet<Point> StartingPoints()
        {
            return new HashSet<Point>
            {
                new Point(1, 0),
                new Point(2, 1),
                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2)
            };
        }

        private class Mapping
        {
            public Point[][] Sources { get; }
            public Point[] Result { get; }
            public int Size { get; }
            public Dictionary<int, int> PointsAtDepth { get; }

            public Mapping(string input)
            {
                var chunks = Parser.SplitOn(input, " => ");
                Size = chunks[0].Length == 5 ? 2 : 3;

                var resultChunks = Parser.SplitOn(chunks[1], '/');
                var resultAcc = new List<Point>();
                for(var i = 0; i < resultChunks.Length; i++)
                {
                    for(var j = 0; j < resultChunks[i].Length; j++)
                    {
                        if (resultChunks[i][j] == '#')
                            resultAcc.Add(new Point(j, i));
                    }
                }
                Result = resultAcc.ToArray();

                var sourceChunks = Parser.SplitOn(chunks[0], '/');
                var sourceAcc = new List<Point>();
                for (var i = 0; i < sourceChunks.Length; i++)
                {
                    for (var j = 0; j < sourceChunks[i].Length; j++)
                    {
                        if (sourceChunks[i][j] == '#')
                            sourceAcc.Add(new Point(j, i));
                    }
                }

                var sourceVari = new[]
                {
                    sourceAcc.ToArray(),
                    sourceAcc.Select(it => new Point(it.Y, it.X)).ToArray()
                };

                var sub1 = Size - 1;
                Sources = sourceVari.SelectMany(it => new[]
                {
                    it,
                    it.Select(p => new Point(sub1 - p.Y, p.X)).ToArray(),
                    it.Select(p => new Point(sub1 - p.X, sub1 - p.Y)).ToArray(),
                    it.Select(p => new Point(p.Y, sub1 - p.X)).ToArray()
                }).Select(it => it.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray()).Distinct(ArraysMatch).ToArray();

                PointsAtDepth = new Dictionary<int, int> { { 0, sourceAcc.Count() } };
            }

            public int GetForDepth(int depth, Mapping[] mappings)
            {
                if (PointsAtDepth.ContainsKey(depth))
                    return PointsAtDepth[depth];

                var stepPoints = Day21.Run(3, mappings, Sources[0].ToHashSet());

                if(depth == 3)
                {
                    PointsAtDepth.Add(3, stepPoints.Count());

                    return stepPoints.Count();
                }

                var subGroups = SplitIntoSubgroups(stepPoints);
                var total = 0;
                foreach(var group in subGroups)
                {
                    var matching = mappings.First(it => it.Matches(group));
                    var result = matching.GetForDepth(depth - 3, mappings);
                    total += result;
                }

                PointsAtDepth.Add(depth, total);

                return total;
            }

            public bool Matches(Point[] input)
            {
                input = input.OrderBy(it => it.X).ThenBy(it => it.Y).ToArray();

                return Sources.Any(it => ArraysMatch(input, it));
            }

            private bool ArraysMatch(Point[] a, Point[] b)
            {
                return a.Length == b.Length && a.Zip(b, (it1, it2) => it1.Equals(it2)).All(it => it);
            }

            private Point[][] SplitIntoSubgroups(HashSet<Point> points)
            {
                List<Point[]> output = new List<Point[]>();
                for(var y = 0; y < 3; y++)
                {
                    for(var x = 0; x < 3; x++)
                    {
                        var matching = points
                            .Where(it => it.WithinBounds(x * 3, (x * 3) + 2, y * 3, (y * 3) + 2))
                            .Select(it => new Point(it.X - (x * 3), it.Y - (y * 3)))
                            .ToArray();
                        output.Add(matching);
                    }
                }

                return output.ToArray();
            }
        }
    }
}
