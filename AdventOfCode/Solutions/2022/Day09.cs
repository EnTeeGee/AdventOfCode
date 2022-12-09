using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2022
{
    class Day09
    {
        [Solution(9, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .ToArray();

            var head = new Point();
            var tail = new Point();
            var visited = new HashSet<Point>();
            visited.Add(tail);

            foreach (var item in lines)
            {
                var steps = int.Parse(item[1]);

                for(var i = 0; i < steps; i++)
                {
                    var next = new Point();
                    if (item[0] == "U")
                        next = new Point(head.X, head.Y - 1);
                    else if (item[0] == "R")
                        next = new Point(head.X + 1, head.Y);
                    else if (item[0] == "D")
                        next = new Point(head.X, head.Y + 1);
                    else
                        next = new Point(head.X - 1, head.Y);

                    if (!(tail.Equals(next) || tail.GetSurrounding8().Contains(next)))
                        tail = head;
                    head = next;

                    visited.Add(tail);
                }
            }

            return visited.Count();
        }

        [Solution(9, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .ToArray();

            var rope = new Point[10];
            var visited = new HashSet<Point>();
            visited.Add(rope[0]);

            foreach (var item in lines)
            {
                var steps = int.Parse(item[1]);

                for (var i = 0; i < steps; i++)
                {
                    var next = new Point();
                    if (item[0] == "U")
                        next = new Point(rope[0].X, rope[0].Y - 1);
                    else if (item[0] == "R")
                        next = new Point(rope[0].X + 1, rope[0].Y);
                    else if (item[0] == "D")
                        next = new Point(rope[0].X, rope[0].Y + 1);
                    else
                        next = new Point(rope[0].X - 1, rope[0].Y);
                    var prev = rope[0];
                    rope[0] = next;

                    for(var j = 1; j < rope.Length; j++)
                    {
                        if (rope[j].Equals(rope[j - 1]) || rope[j].GetSurrounding8().Contains(rope[j - 1]))
                            break;

                        var validTargets = rope[j - 1].GetSurrounding4().Where(it => rope[j].GetSurrounding4().Contains(it)).ToArray();
                        if (validTargets.Any())
                            rope[j] = validTargets.First();
                        else
                        {
                            validTargets = rope[j].GetSurrounding8()
                                .Where(it => (!rope[j].GetSurrounding4().Contains(it)) && rope[j - 1].GetSurrounding8().Contains(it))
                                .ToArray();
                            rope[j] = validTargets.First();
                        }
                    }

                    visited.Add(rope[9]);
                }
            }

            return visited.Count();
        }

        private void Draw(Point[] points, HashSet<Point> seen)
        {
            for (var y = -10; y < 10; y++)
            {
                var line = new StringBuilder();

                for (var x = -10; x < 10; x++)
                {
                    var point = new Point(x, y);
                    var existing = Array.IndexOf(points, point);


                    if (existing == 0)
                        line.Append("H");
                    else if (existing > 0)
                        line.Append(existing);
                    else if (point.Equals(Point.Origin))
                        line.Append("s");
                    else if (seen.Contains(point))
                        line.Append("#");
                    else
                        line.Append('.');
                }

                Console.WriteLine(line.ToString());
            }

            Console.WriteLine();
        }
    }
}
