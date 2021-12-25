using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2021
{
    class Day25
    {
        [Solution(25, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var eastFacing = new HashSet<Point>();
            var southFacing = new HashSet<Point>();
            for(var i = 0; i < lines.Length; i++)
            {
                for(var j = 0; j < lines[0].Length; j++)
                {
                    var item = lines[i][j];
                    if (item == '>')
                        eastFacing.Add(new Point(j, i));
                    else if (item == 'v')
                        southFacing.Add(new Point(j, i));
                }
            }

            var limit = new Point(lines[0].Length, lines.Length);
            var steps = 1;

            while(true)
            {
                var movingEast = new Dictionary<Point, Point>();
                var movingSouth = new Dictionary<Point, Point>();

                foreach(var item in eastFacing)
                {
                    var target = GetNext(item, limit, true);
                    if (!eastFacing.Contains(target) && !southFacing.Contains(target))
                        movingEast.Add(item, target);
                }

                foreach(var item in movingEast)
                {
                    eastFacing.Remove(item.Key);
                    eastFacing.Add(item.Value);
                }

                foreach(var item in southFacing)
                {
                    var target = GetNext(item, limit, false);
                    if (!eastFacing.Contains(target) && !southFacing.Contains(target))
                        movingSouth.Add(item, target);
                }

                foreach(var item in movingSouth)
                {
                    southFacing.Remove(item.Key);
                    southFacing.Add(item.Value);
                }

                if (!movingEast.Any() && !movingSouth.Any())
                {
                    //Draw(eastFacing, southFacing, limit);

                    return steps;
                }

                steps++;
            }
        }

        private Point GetNext(Point source, Point limit, bool isEast)
        {
            if (isEast)
                return new Point(source.X + 1 >= limit.X ? 0 : source.X + 1, source.Y);
            else
                return new Point(source.X, source.Y + 1 >= limit.Y ? 0 : source.Y + 1);
        }

        private void Draw(HashSet<Point> east, HashSet<Point> south, Point limit)
        {
            for(var i = 0; i < limit.Y; i++)
            {
                var builder = new StringBuilder();
                for(var j = 0; j < limit.X; j++)
                {
                    var point = new Point(j, i);
                    if (east.Contains(point))
                        builder.Append(">");
                    else if (south.Contains(point))
                        builder.Append("v");
                    else
                        builder.Append(".");
                }

                Console.WriteLine(builder.ToString());
            }
        }
    }
}
