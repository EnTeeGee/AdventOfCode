using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            var visited = new HashSet<Point>();
            var currentPos = new Point();
            visited.Add(currentPos);

            foreach (var direction in input)
            {
                var newPoint = GetNextPoint(direction, currentPos);

                if (!visited.Contains(newPoint))
                    visited.Add(newPoint);

                currentPos = newPoint;
            }

            return visited.Count();
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var visited = new HashSet<Point>();
            var currentPoints = new List<Point> { new Point(), new Point() };
            visited.Add(new Point());

            foreach(var direction in input)
            {
                var newPoint = GetNextPoint(direction, currentPoints[0]);

                if (!visited.Contains(newPoint))
                    visited.Add(newPoint);

                currentPoints.RemoveAt(0);
                currentPoints.Add(newPoint);
            }

            return visited.Count();
        }

        private Point GetNextPoint(char direction, Point currentPoint)
        {
            switch (direction)
            {
                case '^':
                    return currentPoint.MoveNorth();
                case '>':
                    return currentPoint.MoveEast();
                case '<':
                    return currentPoint.MoveEast(-1);
                case 'v':
                    return currentPoint.MoveNorth(-1);
            }

            return currentPoint;
        }
    }
}
