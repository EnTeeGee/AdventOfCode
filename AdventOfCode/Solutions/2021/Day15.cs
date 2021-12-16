using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = Enumerable.Range(0, lines[0].Length).SelectMany(it => Enumerable.Range(0, lines.Length).Select(x => new Point(x, it))).ToHashSet();
            var pointValues = points.ToDictionary(it => it, it => int.Parse(lines[it.Y][(int)it.X].ToString()));
            var start = new Point(0, 0);
            var end = new Point(lines[0].Length - 1, lines.Length - 1);
            var seen = new[] { start }.ToHashSet();
            var progress = new PriorityQueue<(Point Point, int Dist)>((a, b) => a.Dist - b.Dist);
            progress.Insert((start, 0));

            while(progress.Any())
            {
                var current = progress.Pop();
                var next = current.Point.GetSurrounding4().Where(it => points.Contains(it) && !seen.Contains(it)).ToArray();
                if (next.Any(it => it.Equals(end)))
                    return current.Dist + pointValues[end];
                foreach(var item in next)
                {
                    seen.Add(item);
                    progress.Insert((item, current.Dist + pointValues[item]));
                }
            }

            throw new Exception("Found no path");
        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = new HashSet<Point>();
            var start = new Point(0, 0);
            var end = new Point((lines[0].Length * 5) - 1, (lines.Length * 5) - 1);
            var seen = new[] { start }.ToHashSet();
            var progress = new PriorityQueue<(Point Point, int Dist)>((a, b) => a.Dist - b.Dist);
            progress.Insert((start, 0));

            while (progress.Any())
            {
                var current = progress.Pop();
                var next = current.Point.GetSurrounding4().Where(it => InGrid(it, end) && !seen.Contains(it)).ToArray();
                if (next.Any(it => it.Equals(end)))
                    return current.Dist + GetPointValue(end, lines);

                foreach(var item in next)
                {
                    seen.Add(item);
                    progress.Insert((item, current.Dist + GetPointValue(item, lines)));
                }
            }

            throw new Exception("Found no path");
        }

        private int GetPointValue(Point point, string[] lines)
        {
            var baseValue = lines[point.Y % lines.Length][(int)point.X % lines[0].Length];
            var steps = (point.Y / lines.Length) + (point.X / lines[0].Length);

            return ((int.Parse(baseValue.ToString()) + (int)steps - 1) % 9) + 1;
        }

        private bool InGrid(Point point, Point end)
        {
            return point.X >= 0 && point.X <= end.X && point.Y >= 0 && point.Y <= end.Y;
        }
    }
}
