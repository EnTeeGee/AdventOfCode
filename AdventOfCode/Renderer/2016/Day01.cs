using AdventOfCode.Common;
using ImageMagick;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Renderer._2016
{
    class Day01
    {
        [Renderable(1)]
        public MagickImage GenerateImage(string input)
        {
            var orders = Parser.SplitOn(input, ' ', ',');
            var orient = Orientation.North;
            var location = new Point();
            var visited = new HashSet<Point>();
            visited.Add(location);
            Point firstIntersect = null;

            foreach (var item in orders)
            {
                var dir = item[0];
                var dist = int.Parse(new string(item.Skip(1).ToArray()));

                if (dir == 'L')
                    orient = orient.RotateAntiClock();
                else
                    orient = orient.RotateClockwise();

                for (var i = 0; i < dist; i++)
                {
                    location = location.MoveOrient(orient);
                    if (visited.Contains(location))
                    {
                        if (firstIntersect == null)
                            firstIntersect = location;
                    }
                    else
                        visited.Add(location);
                }
            }

            var upperLeft = new Point(visited.Min(it => it.X), visited.Min(it => it.Y));
            var lowerRight = new Point(visited.Max(it => it.X), visited.Max(it => it.Y));
            var buffer = 10;

            var output = new MagickImage(MagickColors.Black, (int)((lowerRight.X - upperLeft.X) + (buffer * 2)), (int)((lowerRight.Y - upperLeft.Y) + (buffer * 2)));

            foreach(var item in visited)
            {
                new Drawables()
                    .FillColor(MagickColors.White)
                    .Point(item.X - upperLeft.X + buffer, item.Y - upperLeft.Y + buffer)
                    .Draw(output);
            }

            new Drawables()
                .FillColor(MagickColors.Yellow)
                .Rectangle(-upperLeft.X - 1 + buffer, -upperLeft.Y - 1 + buffer, -upperLeft.X + 1 + buffer, -upperLeft.Y + 1 + buffer)
                .Draw(output);

            new Drawables()
                .FillColor(MagickColors.Red)
                .Rectangle(
                    firstIntersect.X - upperLeft.X - 1 + buffer,
                    firstIntersect.Y - upperLeft.Y - 1 + buffer,
                    firstIntersect.X - upperLeft.X + 1 + buffer,
                    firstIntersect.Y - upperLeft.Y + 1 + buffer)
                .Draw(output);

            output.Scale(new Percentage(200));

            return output;
        }
    }
}
