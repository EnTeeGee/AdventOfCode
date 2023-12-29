using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day17
    {
        private static Dictionary<Orientation, string> DirMapping = new Dictionary<Orientation, string> {
            { Orientation.North, "U" },
            { Orientation.South, "D" },
            { Orientation.West, "L" },
            { Orientation.East, "R" }
        };

        private static Orientation[] OriMap = new[]
        {
            Orientation.North,
            Orientation.South,
            Orientation.West,
            Orientation.East
        };

        [Solution(17, 1)]
        public string Solution1(string input)
        {
            var paths = new Queue<(string route, Point pos)>();
            paths.Enqueue(new(string.Empty, Point.Origin));
            var bound = new Point(3, 3);

            while (paths.Any())
            {
                var current = paths.Dequeue();
                if (current.pos == bound)
                    return current.route;
                var hash = string.Join(
                    string.Empty,
                    MD5.Create()
                        .ComputeHash(ASCIIEncoding.ASCII.GetBytes(input + current.route))
                        .Take(2)
                        .Select(it => it.ToString("X2")));

                var valid = hash
                    .Zip(OriMap, (a, b) => new { letter = a, dir = b })
                    .Where(it => it.letter > 'A')
                    .Where(it => current.pos.MoveOrient(it.dir).WithinBounds(0, bound.X, 0, bound.Y))
                    .ToArray();

                foreach(var item in valid)
                    paths.Enqueue(new(current.route + DirMapping[item.dir], current.pos.MoveOrient(item.dir)));
            }

            throw new Exception("No path found");
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var paths = new Queue<(string route, Point pos)>();
            paths.Enqueue(new(string.Empty, Point.Origin));
            var bound = new Point(3, 3);
            var max = 0;

            while (paths.Any())
            {
                var current = paths.Dequeue();
                if (current.pos == bound)
                {
                    if (current.route.Length > max)
                        max = current.route.Length;

                    continue;
                }
                var hash = string.Join(
                    string.Empty,
                    MD5.Create()
                        .ComputeHash(ASCIIEncoding.ASCII.GetBytes(input + current.route))
                        .Take(2)
                        .Select(it => it.ToString("X2")));

                var valid = hash
                    .Zip(OriMap, (a, b) => new { letter = a, dir = b })
                    .Where(it => it.letter > 'A')
                    .Where(it => current.pos.MoveOrient(it.dir).WithinBounds(0, bound.X, 0, bound.Y))
                    .ToArray();

                foreach (var item in valid)
                    paths.Enqueue(new(current.route + DirMapping[item.dir], current.pos.MoveOrient(item.dir)));
            }

            return max;
        }
    }
}
