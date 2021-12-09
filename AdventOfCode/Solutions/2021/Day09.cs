using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day09
    {
        [Solution(9, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = lines.SelectMany(it => it.Select(c => int.Parse(c.ToString()))).ToArray();

            return Enumerable.Range(0, points.Length)
                .Where(it => GetSurrounding(it, lines[0].Length, points.Length).All(p => points[p] > points[it]))
                .Sum(it => points[it] + 1);
        }

        [Solution(9, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = lines.SelectMany(it => it.Select(c => int.Parse(c.ToString()))).ToArray();
            var pointMap = Enumerable.Range(0, points.Length).Where(it => points[it] != 9).Select(it => new Point(it % lines[0].Length, it / lines[0].Length)).ToHashSet();
            var basins = new List<int>(); 

            while (pointMap.Any())
            {
                var point = pointMap.First();
                pointMap.Remove(point);
                var basinSize = GetInBasin(point, pointMap) + 1;
                basins.Add(basinSize);
            }

            return basins.OrderByDescending(it => it).Take(3).Aggregate(1, (acc, i) => acc * i);
        }

        private int[] GetSurrounding(int index, int width, int limit)
        {
            return new[] { index - 1, index + 1, }
                .Where(it => it >= 0 && it < limit && (index / width == it / width))
                .Concat(new[] { index - width, index + width }.Where(it => it >= 0 && it < limit))
                .ToArray();
        }

        private int GetInBasin(Point source, HashSet<Point> remaining)
        {
            var matches = source.GetSurrounding4().Where(it => remaining.Contains(it)).ToArray();
            foreach (var item in matches)
                remaining.Remove(item);

            return matches.Length + matches.Sum(it => GetInBasin(it, remaining));

        }
    }
}
