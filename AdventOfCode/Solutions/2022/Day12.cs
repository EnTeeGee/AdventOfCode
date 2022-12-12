using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var (start, end, heightMap) = GenerateHeightMap(lines);

            return SearchMap(start, heightMap, (height, currentHeight) => height <= currentHeight + 1, (it) => it.Equals(end));
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var (_, start, heightMap) = GenerateHeightMap(lines);

            return SearchMap(start, heightMap, (height, currentHeight) => height >= currentHeight - 1, (it) => heightMap[it.X, it.Y] == 0);
        }

        private (Point start, Point end, int[,] heightMap) GenerateHeightMap(string[] input)
        {
            var start = new Point();
            var end = new Point();
            var heightMap = new int[input[0].Length, input.Length];

            for (var y = 0; y < input.Length; y++)
            {
                var line = input[y];
                for (var x = 0; x < line.Length; x++)
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

            return (start, end, heightMap);
        }

        private int SearchMap(Point start, int[,] heightMap, Func< int, int, bool> validCheck, Func<Point, bool> completeCheck)
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
            var seen = new int?[width, height];
            seen[start.X, start.Y] = 0;
            var boundary = new Queue<Point>(new[] { start });

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var currentHeight = heightMap[current.X, current.Y];
                var currentDist = seen[current.X, current.Y].Value;

                var toTarget = current.GetSurrounding4()
                    .Where(it => it.WithinBounds(0, width - 1, 0, height - 1))
                    .Where(it => seen[it.X, it.Y] == null)
                    .Where(it => validCheck.Invoke(heightMap[it.X, it.Y], currentHeight))
                    .ToArray();

                if (toTarget.Any(it => completeCheck.Invoke(it)))
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
