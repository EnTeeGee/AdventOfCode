using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using AdventOfCodeCore.Solutions._2017.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day14
    {
        [Solution(14, 1)]
        public int Solution1(string input)
        {
            var lines = Enumerable.Range(0, 128).Select(it => new KnotHash($"{input}-{it}").AsBinary());

            return lines.Sum(it => it.Count(x => x == '1'));
        }

        [Solution(14, 2)]
        public int Solution2(string input)
        {
            var lines = Enumerable.Range(0, 128).Select(it => new KnotHash($"{input}-{it}").AsBinary());
            var points = lines
                .SelectMany((it, i) => it.Select((x, j) => x == '1' ? new Point(j, i) : (Point?)null))
                .Where(it => it != null)
                .Cast<Point>()
                .ToHashSet();

            var regions = 0;

            while (points.Any())
            {
                var boundary = new Queue<Point>();
                boundary.Enqueue(points.First());
                points.Remove(boundary.Peek());

                while (boundary.Any())
                {
                    var point = boundary.Dequeue();
                    var next = point.GetSurrounding4().Where(it => points.Contains(it));
                    foreach (var item in next)
                    {
                        boundary.Enqueue(item);
                        points.Remove(item);
                    }
                }

                regions++;
            }

            return regions;
        }
    }
}
