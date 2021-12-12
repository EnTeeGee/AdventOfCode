using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day11
    {
        [Solution(11, 1)]
        public int Solution1(string input)
        {
            var points = ToDictionary(input);
            var totalFlashes = 0;

            for(var i = 0; i < 100; i++)
            {
                totalFlashes += IterateGrid(points);
            }

            return totalFlashes;
        }

        [Solution(11, 2)]
        public int Solution2(string input)
        {
            var points = ToDictionary(input);
            var step = 1;

            while(true)
            {
                var result = IterateGrid(points);
                if (result == points.Count())
                    return step;

                step++;
            }
        }

        private Dictionary<Point, int> ToDictionary(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            return Enumerable
                .Range(0, lines[0].Length)
                .SelectMany(it => Enumerable.Range(0, lines.Length).Select(y => new Point(it, y)))
                .ToDictionary(it => it, it => int.Parse(lines[it.Y][(int)it.X].ToString()));
        }

        private int IterateGrid(Dictionary<Point, int> points)
        {
            var pointArray = points.Keys.ToArray();
            foreach (var item in pointArray)
                points[item]++;

            var flashing = new Queue<Point>();
            var flashed = points.Where(it => it.Value > 9).Select(it => it.Key).ToHashSet();
            foreach (var item in flashed)
                flashing.Enqueue(item);

            while (flashing.Any())
            {
                var octo = flashing.Dequeue();
                var surrounding = octo.GetSurrounding8().Where(it => points.ContainsKey(it) && !flashed.Contains(it));
                foreach (var item in surrounding)
                {
                    points[item] += 1;
                    if (points[item] > 9)
                    {
                        flashing.Enqueue(item);
                        flashed.Add(item);
                    }
                }
            }

            foreach (var item in flashed)
                points[item] = 0;

            return flashed.Count();
        }
    }
}
