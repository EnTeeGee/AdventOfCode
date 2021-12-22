using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day20
    {
        [Solution(20, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var mapping = new string(chunks[0].Where(it => it == '.' || it == '#').ToArray());
            var points = new HashSet<Point>();
            var lines = Parser.ToArrayOfString(chunks[1]);
            for(var i = 0; i < lines.Length; i++)
                for(var j = 0; j < lines[0].Length; j++)
                    if (lines[i][j] == '#')
                        points.Add(new Point(j, i));

            return Enhance(mapping, points, 2);
        }

        [Solution(20, 2)]
        public int Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var mapping = new string(chunks[0].Where(it => it == '.' || it == '#').ToArray());
            var points = new HashSet<Point>();
            var lines = Parser.ToArrayOfString(chunks[1]);
            for (var i = 0; i < lines.Length; i++)
                for (var j = 0; j < lines[0].Length; j++)
                    if (lines[i][j] == '#')
                        points.Add(new Point(j, i));

            return Enhance(mapping, points, 50);
        }

        private int Enhance(string mapping, HashSet<Point> points, int steps)
        {
            var outOfBoundsValue = '0';

            for (var step = 0; step < steps; step++)
            {
                var topLeft = new Point(points.Min(it => it.X), points.Min(it => it.Y));
                var bottomRight = new Point(points.Max(it => it.X), points.Max(it => it.Y));
                var newPoints = new HashSet<Point>();

                for (var i = topLeft.X - 1; i <= bottomRight.X + 1; i++)
                {
                    for (var j = topLeft.Y - 1; j <= bottomRight.Y + 1; j++)
                    {
                        var point = new Point(j, i);
                        var values = new[] { point }
                            .Concat(point.GetSurrounding8())
                            .OrderBy(it => it.Y)
                            .ThenBy(it => it.X)
                            .Select(it => IsOutOfBounds(it, topLeft, bottomRight) ? outOfBoundsValue : points.Contains(it) ? '1' : '0')
                            .ToArray();
                        var index = Convert.ToInt32(new string(values), 2);
                        if (mapping[index] == '#')
                            newPoints.Add(point);
                    }
                }

                var newOutOfBounds = outOfBoundsValue == '0' ? mapping[0] : mapping[511];
                outOfBoundsValue = newOutOfBounds == '#' ? '1' : '0';
                points = newPoints;
            }

            return points.Count();
        }

        private bool IsOutOfBounds(Point point, Point topLeft, Point bottomRight)
        {
            return point.X < topLeft.X || point.X > bottomRight.X || point.Y < topLeft.Y || point.Y > bottomRight.Y;
        }
    }
}
