using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var nodes = GetAntennas(lines);

            var antinodes = new HashSet<Point>();
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);

            return nodes
                .SelectMany(it => Permutations.GetAllPossiblePairs(it.Value.ToArray()).SelectMany(it2 => GetAntinodes(it2)))
                .Distinct()
                .Count(it => it.WithinBounds(0, bounds.X, 0, bounds.Y));
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var nodes = GetAntennas(lines);

            var antinodes = new HashSet<Point>();
            var bounds = new Point(lines[0].Length - 1, lines.Length - 1);

            return nodes
                .SelectMany(it => Permutations
                    .GetAllPossiblePairs(it.Value.ToArray())
                    .SelectMany(it2 => new[] { it2, it2.Reverse().ToArray() })
                    .SelectMany(it2 => GetHarmonicAntinodes(it2).TakeWhile(it3 => it3.WithinBounds(0, bounds.X, 0, bounds.Y)).ToArray()))
                .Distinct()
                .Count();
        }

        private Dictionary<char, List<Point>> GetAntennas(string[] lines)
        {
            var nodes = new Dictionary<char, List<Point>>();

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var item = lines[y][x];

                    if (item != '.')
                    {
                        if (nodes.ContainsKey(item))
                            nodes[item].Add(new Point(x, y));
                        else
                            nodes.Add(item, new List<Point> { new Point(x, y) });
                    }
                }
            }

            return nodes;
        }

        private Point[] GetAntinodes(Point[] points)
        {
            return new[]
            {
                points[0] - (points[1] - points[0]),
                points[1] - (points[0] - points[1])
            };
        }

        private IEnumerable<Point> GetHarmonicAntinodes(Point[] points)
        {
            var diff = points[1] - points[0];
            var current = points[0];

            while(true)
            {
                yield return current;
                current += diff;
            }    
        }
    }
}
