using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2016
{
    class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var points = new HashSet<Point>();
            var limit = new Point(50, 6);
            var lines = Parser.ToArrayOfString(input);

            foreach (var item in lines)
                ApplyCommand(item, points, limit);

            return points.Count();
        }

        [Solution(8, 2)]
        public string Solution2(string input)
        {
            var points = new HashSet<Point>();
            var limit = new Point(50, 6);
            var lines = Parser.ToArrayOfString(input);

            foreach (var item in lines)
                ApplyCommand(item, points, limit);

            var output = new StringBuilder();
            for(var i = 0; i < limit.Y; i++)
            {
                for(var j = 0; j < limit.X; j++)
                {
                    if (points.Contains(new Point(j, i)))
                        output.Append("#");
                    else
                        output.Append(".");
                }
                output.AppendLine();
            }

            return output.ToString();
        }

        private void ApplyCommand(string command, HashSet<Point> points, Point limit)
        {
            var chunks = Parser.SplitOnSpace(command);
            if(chunks[0] == "rect")
            {
                var dimensions = Parser.SplitOn(chunks[1], 'x').Select(it => int.Parse(it)).ToArray();
                for(var i = 0; i < dimensions[0]; i++)
                {
                    for(var j = 0; j < dimensions[1]; j++)
                    {
                        if (!points.Contains(new Point(i, j)))
                            points.Add(new Point(i, j));
                    }
                }
            }
            else
            {
                var index = int.Parse(Parser.SplitOn(chunks[2], '=')[1]);
                var dist = int.Parse(chunks[4]);
                Point[] moved = null;

                if(chunks[1] == "row")
                {
                    var existing = points.Where(it => it.Y == index).ToArray();
                    moved = existing.Select(it => new Point((it.X + dist) % limit.X, it.Y)).ToArray();
                    points.RemoveWhere(it => it.Y == index);
                }
                else
                {
                    var existing = points.Where(it => it.X == index).ToArray();
                    moved = existing.Select(it => new Point(it.X, (it.Y + dist) % limit.Y)).ToArray();
                    points.RemoveWhere(it => it.X == index);
                }

                foreach (var item in moved)
                    points.Add(item);
            }
        }
    }
}
