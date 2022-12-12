using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var start = new Point();
            var end = new Point();
            var heightMap = new int[lines[0].Length, lines.Length];

            for(var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for(var x = 0; x < line.Length; x++)
                {
                    var item = line[x];
                    if (item == 'S')
                    {
                        start = new Point(x, y);
                        heightMap[x, y] = 0;
                    }
                    else if (item == 'E')
                    {
                        end = new Point(x, y);
                        heightMap[x, y] = 25;
                    }
                    else
                        heightMap[x, y] = item - 'a';
                }
            }

            var seen = new int?[lines[0].Length, lines.Length];
            seen[start.X, start.Y] = 0;
            var boundary = new Queue<Point>(new[] { start });

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var currentHeight = heightMap[current.X, current.Y];
                var currentDist = seen[current.X, current.Y].Value;

                var toTarget = current.GetSurrounding4()
                    .Where(it => it.WithinBounds(0, lines[0].Length - 1, 0, lines.Length - 1))
                    .Where(it => seen[it.X, it.Y] == null)
                    .Where(it => heightMap[it.X, it.Y] <= currentHeight + 1)
                    .ToArray();

                if (toTarget.Any(it => it.Equals(end)))
                    return currentDist + 1;

                foreach(var item in toTarget)
                {
                    seen[item.X, item.Y] = seen[current.X, current.Y] + 1;
                    boundary.Enqueue(item);
                }
            }

            throw new Exception("Unable to find path");
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var start = new Point();
            var heightMap = new int[lines[0].Length, lines.Length];

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var item = line[x];
                    if (item == 'S')
                        heightMap[x, y] = 0;
                    else if (item == 'E')
                    {
                        start = new Point(x, y);
                        heightMap[x, y] = 25;
                    }
                    else
                        heightMap[x, y] = item - 'a';
                }
            }

            var seen = new int?[lines[0].Length, lines.Length];
            seen[start.X, start.Y] = 0;
            var boundary = new Queue<Point>(new[] { start });

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var currentHeight = heightMap[current.X, current.Y];
                var currentDist = seen[current.X, current.Y].Value;

                var toTarget = current.GetSurrounding4()
                    .Where(it => it.WithinBounds(0, lines[0].Length - 1, 0, lines.Length - 1))
                    .Where(it => seen[it.X, it.Y] == null)
                    .Where(it => heightMap[it.X, it.Y] >= currentHeight - 1)
                    .ToArray();

                if (toTarget.Any(it => heightMap[it.X, it.Y] == 0))
                    return currentDist + 1;

                foreach (var item in toTarget)
                {
                    seen[item.X, item.Y] = seen[current.X, current.Y] + 1;
                    boundary.Enqueue(item);
                }
            }

            throw new Exception("Unable to find path");
        }
    }
}
