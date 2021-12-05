using AdventOfCode.Common;
using ImageMagick;
using System;
using System.Linq;

namespace AdventOfCode.Renderer._2021
{
    class Day05
    {
        [Renderable(5)]
        public MagickImage GenerateImage(string input)
        {
            var pointPairs = Parser.ToArrayOf(input, it => new PointPair(it));
            var pointList = pointPairs.SelectMany(it => it.All).ToArray();
            var crossings = pointList.GroupBy(it => it).Where(it => it.Count() > 1).Select(it => it.Key).ToArray();

            var offsetX = pointList.Min(it => it.X) - 10;
            var offsetY = pointList.Min(it => it.Y) - 10;
            var width = pointList.Max(it => it.X) - offsetX + 20;
            var height = pointList.Max(it => it.Y) - offsetY + 20;

            var output = new MagickImage(MagickColors.Blue, (int)width, (int)height);

            foreach(var line in pointPairs)
            {
                new Drawables()
                    .FillColor(MagickColors.DarkBlue)
                    .Line(line.Start.X - offsetX, line.Start.Y - offsetY, line.End.X - offsetX, line.End.Y - offsetY)
                    .Draw(output);
            }

            foreach(var item in crossings)
            {
                new Drawables()
                    .FillColor(MagickColors.Black)
                    .Point(item.X - offsetX, item.Y - offsetY)
                    .Draw(output);
            }

            return output;
        }

        private class PointPair
        {
            public Point Start { get; }
            public Point End { get; }

            public Point[] All { get; }
            public bool IsLevel { get; }

            public PointPair(string input)
            {
                var splits = Parser.SplitOn(input, ' ', '-', '>', ',');
                Start = new Point(int.Parse(splits[0]), int.Parse(splits[1]));
                End = new Point(int.Parse(splits[2]), int.Parse(splits[3]));

                if (Start.X == End.X)
                {
                    IsLevel = true;
                    All = Enumerable.Range((int)Math.Min(Start.Y, End.Y), (int)Math.Abs(Start.Y - End.Y) + 1).Select(it => new Point(Start.X, it)).ToArray();
                }
                else if (Start.Y == End.Y)
                {
                    IsLevel = true;
                    All = Enumerable.Range((int)Math.Min(Start.X, End.X), (int)Math.Abs(Start.X - End.X) + 1).Select(it => new Point(it, Start.Y)).ToArray();
                }
                else
                {
                    var diff = (int)Math.Abs(Start.X - End.X);
                    var stepX = Start.X < End.X ? 1 : -1;
                    var stepY = Start.Y < End.Y ? 1 : -1;
                    All = Enumerable.Range(0, diff + 1).Select(it => new Point(Start.X + (it * stepX), Start.Y + (it * stepY))).ToArray();
                }
            }
        }
    }
}
