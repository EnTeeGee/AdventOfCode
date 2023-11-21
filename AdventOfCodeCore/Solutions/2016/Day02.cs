using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day02
    {
        private static Dictionary<char, Func<Point, Point>> directionMapper = new Dictionary<char, Func<Point, Point>>
            {
                { 'U', it => it.MoveNorth(-1) },
                { 'R', it => it.MoveEast() },
                { 'D', it => it.MoveNorth() },
                { 'L', it => it.MoveEast(-1) }
            };

        [Solution(2, 1)]
        public string Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var buttonMapping = new Dictionary<Point, char>
            {
                { new Point(-1, 1), '1' },
                { new Point(0, 1), '2' },
                { new Point(1, 1), '3' },
                { new Point(-1, 0), '4' },
                { new Point(0, 0), '5' },
                { new Point(1, 0), '6' },
                { new Point(-1, -1), '7' },
                { new Point(0, -1), '8' },
                { new Point(1, -1), '9' }
            };

            var currentLocation = Point.Origin;
            var output = new List<char>();

            foreach(var line in lines)
            {
                foreach(var item in line)
                {
                    var newPoint = directionMapper[item](currentLocation);
                    if (buttonMapping.ContainsKey(newPoint))
                        currentLocation = newPoint;
                }

                output.Add(buttonMapping[currentLocation]);
            }

            return new string(output.ToArray());
        }

        [Solution(2, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var buttonMapping = new Dictionary<Point, char>
            {
                { new Point(0, 2), '1' },
                { new Point(-1, 1), '2' },
                { new Point(0, 1), '3' },
                { new Point(1, 1), '4' },
                { new Point(-2, 0), '5' },
                { new Point(-1, 0), '6' },
                { new Point(0, 0), '7' },
                { new Point(1, 0), '8' },
                { new Point(2, 0), '9' },
                { new Point(-1, -1), 'A' },
                { new Point(0, -1), 'B' },
                { new Point(1, -1), 'C' },
                { new Point(0, -2), 'D' }
            };

            var currentLocation = new Point(-2, 0);
            var output = new List<char>();

            foreach (var line in lines)
            {
                foreach (var item in line)
                {
                    var newPoint = directionMapper[item](currentLocation);
                    if (buttonMapping.ContainsKey(newPoint))
                        currentLocation = newPoint;
                }

                output.Add(buttonMapping[currentLocation]);
            }

            return new string(output.ToArray());
        }
    }
}
