using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day18
    {
        [Solution(18,1)]
        public int Solution1(string input)
        {
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ',').Select(n => int.Parse(n)).ToArray())
                .Select(it => new Voxel(it[0], it[1], it[2]))
                .ToHashSet();

            return points.Sum(it => it.GetSurrounding6().Count(p => !points.Contains(p)));
        }

        [Solution(18, 2)]
        public int Solution2(string input)
        {
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ',').Select(n => int.Parse(n)).ToArray())
                .Select(it => new Voxel(it[0], it[1], it[2]))
                .ToHashSet();

            var min = points.Min(it => Math.Min(it.X, Math.Min(it.Y, it.Z))) - 1;
            var max = points.Max(it => Math.Max(it.X, Math.Max(it.Y, it.Z))) + 1;
            var surface = 0;
            var seen = new HashSet<Voxel>();
            var bounday = new Queue<Voxel>();
            bounday.Enqueue(new Voxel(min, min, min));
            seen.Add(new Voxel(min, min, min));

            while (bounday.Any())
            {
                var item = bounday.Dequeue();
                var surrounding = item.GetSurrounding6().Where(it => it.WithinBounds(min, max, min, max, min, max) && !seen.Contains(it)).ToArray();
                var intersecting = surrounding.Where(it => points.Contains(it)).ToArray();
                surface += intersecting.Length;
                foreach(var toAdd in surrounding.Where(it => !intersecting.Contains(it)))
                {
                    seen.Add(toAdd);
                    bounday.Enqueue(toAdd);
                }
            }

            return surface;
        }
    }
}
