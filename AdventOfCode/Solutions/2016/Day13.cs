using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2016
{
    class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var number = int.Parse(input);
            var boundary = new Queue<State>();
            var seen = new HashSet<Point>();
            boundary.Enqueue(new State(0, new Point(1, 1)));
            var goal = new Point(31, 39);
            //var goal = new Point(7, 4);

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var targets = current.Pos.GetSurrounding4().Where(it => !seen.Contains(it) && IsOpen(number, it)).ToArray();

                if (targets.Any(it => it.Equals(goal)))
                    return current.Steps + 1;

                foreach (var item in targets)
                {
                    boundary.Enqueue(new State(current.Steps + 1, item));
                    seen.Add(item);
                }
            }

            throw new Exception("No path found");
        }

        [Solution(13, 2)]
        public int Solution2(string input)
        {
            var number = int.Parse(input);
            var boundary = new Queue<State>();
            var seen = new HashSet<Point>();
            boundary.Enqueue(new State(0, new Point(1, 1)));

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                if (current.Steps >= 50)
                    continue;

                var targets = current.Pos.GetSurrounding4().Where(it => !seen.Contains(it) && IsOpen(number, it)).ToArray();

                foreach (var item in targets)
                {
                    boundary.Enqueue(new State(current.Steps + 1, item));
                    seen.Add(item);
                }
            }

            return seen.Count();
        }

        private bool IsOpen(int number, Point point)
        {
            if (point.X < 0 || point.Y < 0)
                return false;

            var result = (point.X * point.X) + (3 * point.X) + (2 * point.X * point.Y) + point.Y + (point.Y * point.Y) + number;

            return Convert.ToString(result, 2).Count(it => it == '1') % 2 == 0;
        }

        private class State
        {
            public int Steps { get; }
            public Point Pos { get; }

            public State(int steps, Point pos)
            {
                Steps = steps;
                Pos = pos;
            }
        }
    }
}
