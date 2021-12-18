using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day17
    {
        [Solution(17, 1)]
        public int Solution1(string input)
        {
            var items = Parser.SplitOn(input, '.', '=');
            var maxY = int.Parse(items[3]);

            var maxVelocity = -maxY - 1;

            return (maxVelocity * (maxVelocity + 1)) / 2;
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var items = Parser.SplitOn(input, '.', '=', ',');
            var topLeft = new Point(int.Parse(items[1]), int.Parse(items[5]));
            var bottomRight = new Point(int.Parse(items[2]), int.Parse(items[4]));

            var maxYVelocity = -bottomRight.Y - 1;

            return Enumerable.Range(1, (int)bottomRight.X)
                .SelectMany(it => Enumerable.Range((int)bottomRight.Y, (int)(-bottomRight.Y + maxYVelocity + 1)).Select(y => new Point(it, y)))
                .Count(it => HitsTarget(it, topLeft, bottomRight));
        }

        private bool HitsTarget(Point velocity, Point topLeft, Point bottomRight)
        {
            var position = Point.Origin;
            while(position.X <= bottomRight.X && position.Y >= bottomRight.Y)
            {
                position = new Point(position.X + velocity.X, position.Y + velocity.Y);
                velocity = new Point(Math.Max(velocity.X - 1, 0), velocity.Y - 1);

                if (position.X >= topLeft.X && position.X <= bottomRight.X && position.Y <= topLeft.Y && position.Y >= bottomRight.Y)
                    return true;
            }

            return false;
        }
    }
}
