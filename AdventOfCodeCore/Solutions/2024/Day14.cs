using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day14
    {
        [Solution(14, 1)]
        public int Solution1(string input)
        {
            //var bounds = new Point(11, 7);
            var bounds = new Point(101, 103);
            var steps = 100;

            var points = Parser.ToArrayOf(input, it => new Robot(it, bounds).PositionAfter(steps));
            var midpoint = new Point(bounds.X / 2, bounds.Y / 2);

            return points.Count(it => it.X < midpoint.X && it.Y < midpoint.Y)
                * points.Count(it => it.X > midpoint.X && it.Y < midpoint.Y)
                * points.Count(it => it.X < midpoint.X && it.Y > midpoint.Y)
                * points.Count(it => it.X > midpoint.X && it.Y > midpoint.Y);
        }

        [Solution(14, 2)]
        public int Solution2(string input)
        {
            var bounds = new Point(101, 103);
            var points = Parser.ToArrayOf(input, it => new Robot(it, bounds));
            var steps = 0;

            while (true)
            {
                var map = points.Select(it => it.Pos).ToHashSet();
                if(points.Count(it => it.Pos.GetSurrounding8().Any(it2 => map.Contains(it2))) > points.Length / 1.5)
                {
                    Console.WriteLine($"Found potential image at {steps}");
                    for(var y = 0; y < bounds.Y; y++)
                    {
                        var output = new StringBuilder();
                        for(var x = 0; x < bounds.X; x++)
                        {
                            if (map.Contains(new Point(x, y)))
                                output.Append('#');
                            else
                                output.Append(',');
                        }
                        Console.WriteLine(output.ToString());
                    }

                    return steps;
                }

                foreach (var item in points)
                    item.Step();
                steps++;
            }
        }

        private class Robot
        {
            public Point Pos { get; private set; }

            private Point vel;
            private Point bounds;

            public Robot(string input, Point bounds)
            {
                var chunks = Parser.SplitOn(input, '=', ',', ' ');
                Pos = new Point(int.Parse(chunks[1]), int.Parse(chunks[2]));

                vel = new Point(int.Parse(chunks[4]), int.Parse(chunks[5]));
                this.bounds = bounds;
            }

            public Point PositionAfter(int steps)
            {
                var newX = (Pos.X + (vel.X * steps)) % bounds.X;
                var newY = (Pos.Y + (vel.Y * steps)) % bounds.Y;
                if (newX < 0)
                    newX += bounds.X;
                if (newY < 0)
                    newY += bounds.Y;

                return new Point(newX, newY);
            }

            public void Step()
            {
                Pos = PositionAfter(1);
            }

        }
    }
}
