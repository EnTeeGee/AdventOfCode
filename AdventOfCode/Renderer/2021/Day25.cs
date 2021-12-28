using AdventOfCode.Common;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Renderer._2021
{
    class Day25
    {
        [Renderable(25)]
        public MagickImageCollection GenerateImage(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var eastFacing = new HashSet<Point>();
            var southFacing = new HashSet<Point>();
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[0].Length; j++)
                {
                    var item = lines[i][j];
                    if (item == '>')
                        eastFacing.Add(new Point(j, i));
                    else if (item == 'v')
                        southFacing.Add(new Point(j, i));
                }
            }

            var limit = new Point(lines[0].Length, lines.Length);
            var collection = new MagickImageCollection();
            collection.Add(Draw(eastFacing, southFacing, limit));
            collection.Last().AnimationDelay = 20;
            var step = 1;

            while (true)
            {
                var movingEast = new Dictionary<Point, Point>();
                var movingSouth = new Dictionary<Point, Point>();

                foreach (var item in eastFacing)
                {
                    var target = GetNext(item, limit, true);
                    if (!eastFacing.Contains(target) && !southFacing.Contains(target))
                        movingEast.Add(item, target);
                }

                foreach (var item in movingEast)
                {
                    eastFacing.Remove(item.Key);
                    eastFacing.Add(item.Value);
                }

                foreach (var item in southFacing)
                {
                    var target = GetNext(item, limit, false);
                    if (!eastFacing.Contains(target) && !southFacing.Contains(target))
                        movingSouth.Add(item, target);
                }

                foreach (var item in movingSouth)
                {
                    southFacing.Remove(item.Key);
                    southFacing.Add(item.Value);
                }

                if (!movingEast.Any() && !movingSouth.Any())
                {
                    collection.Last().AnimationDelay = 1000;

                    return collection;
                }

                collection.Add(Draw(eastFacing, southFacing, limit));
                collection.Last().AnimationDelay = 20;
                Console.WriteLine($"Drawn step {step++}");
            }
        }

        private Point GetNext(Point source, Point limit, bool isEast)
        {
            if (isEast)
                return new Point(source.X + 1 >= limit.X ? 0 : source.X + 1, source.Y);
            else
                return new Point(source.X, source.Y + 1 >= limit.Y ? 0 : source.Y + 1);
        }

        private MagickImage Draw(HashSet<Point> east, HashSet<Point> south, Point limit)
        {
            var output = new MagickImage(MagickColors.Black, (int)limit.X, (int)limit.Y);

            for(var i = 0; i < limit.Y; i++)
            {
                for(var j = 0; j < limit.X; j++)
                {
                    var point = new Point(j, i);
                    if(east.Contains(point))
                    {
                        new Drawables()
                            .StrokeColor(MagickColors.DarkRed)
                            .Point(point.X, point.Y)
                            .Draw(output);
                    }
                    else if (south.Contains(point))
                    {
                        new Drawables()
                            .StrokeColor(MagickColors.DarkBlue)
                            .Point(point.X, point.Y)
                            .Draw(output);
                    }
                }
            }

            return output;
        }
    }
}
