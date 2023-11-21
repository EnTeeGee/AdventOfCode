using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day01
    {
        [Solution(1, 1)]
        public long Solution1(string input)
        {
            var orders = Parser.SplitOn(input, ' ', ',');
            var orient = Orientation.North;
            var location = new Point();

            foreach(var item in orders)
            {
                var dir = item[0];
                var dist = int.Parse(new string(item.Skip(1).ToArray()));

                if (dir == 'L')
                    orient = orient.RotateAntiClock();
                else
                    orient = orient.RotateClockwise();

                location = location.MoveOrient(orient, dist);
            }

            return location.GetTaxiCabDistanceTo(new Point());
        }

        [Solution(1, 2)]
        public long Solution2(string input)
        {
            var orders = Parser.SplitOn(input, ' ', ',');
            var orient = Orientation.North;
            var location = new Point();
            var visited = new HashSet<Point>();
            visited.Add(location);

            foreach (var item in orders)
            {
                var dir = item[0];
                var dist = int.Parse(new string(item.Skip(1).ToArray()));

                if (dir == 'L')
                    orient = orient.RotateAntiClock();
                else
                    orient = orient.RotateClockwise();

                for(var i = 0; i < dist; i++)
                {
                    location = location.MoveOrient(orient);
                    if (visited.Contains(location))
                        return location.GetTaxiCabDistanceTo(new Point());

                    visited.Add(location);
                }
            }

            throw new Exception("Failed to find point visited twice");
        }
    }
}
