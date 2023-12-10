using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var info = Setup(lines);
            var prior = info.start;
            var current = info.current;
            var count = 1;

            while(!current.Equals(info.start))
            {
                var newNext = GetNext(current, prior, info.pipes[current]);
                prior = current;
                current = newNext!.Value;
                count++;
            }

            return count / 2;
        }

        [Solution(10, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var info = Setup(lines);

            var route = new HashSet<Point>();
            var left = new HashSet<Point>();
            var prior = info.start;
            var current = info.current;
            route.Add(prior);
            route.Add(current);
            left.Add(GetLeft(prior, current)[0]);

            while (true)
            {
                var newNext = GetNext(current, prior, info.pipes[current]);
                prior = current;
                current = newNext!.Value;
                var leftItems = GetLeft(prior, current);
                left.Add(leftItems[0]);
                left.Add(leftItems[1]);
                if (route.Contains(current))
                    break;
                route.Add(current);
            }

            var boundaryPoints = left.Where(it => !route.Contains(it)).Distinct().ToHashSet();
            var boundary = new Queue<Point>(boundaryPoints);

            while (boundary.Any())
            {
                var currentPoint = boundary.Dequeue();
                var surrounding = currentPoint
                    .GetSurrounding4()
                    .Where(it => !boundaryPoints.Contains(it) && !route.Contains(it) && it.WithinBounds(-1, lines[0].Length, -1, lines.Length))
                    .ToArray();

                foreach(var item in surrounding)
                {
                    boundaryPoints.Add(item);
                    boundary.Enqueue(item);
                }
            }

            if (boundaryPoints.Contains(new Point(-1, -1)))
                return ((lines[0].Length + 2) * (lines.Length + 2)) - route.Count() - boundaryPoints.Count();

            return boundaryPoints.Count();
        }

        private (Point start, Point current, Dictionary<Point, char> pipes) Setup(string[] lines)
        {
            var pipes = new Dictionary<Point, char>();
            var start = Point.Origin;

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'S')
                        start = new Point(x, y);
                    else if (line[x] != '.')
                        pipes.Add(new Point(x, y), line[x]);
                }
            }

            var current = start.GetSurrounding4()
                .Where(it => pipes.ContainsKey(it))
                .Select(it => new { orig = it, surr = GetSurrounding(it, pipes[it]) })
                .Where(it => it.surr.Contains(start))
                .Select(it => it.orig)
                .First();

            return (start, current, pipes);
        }

        private Point? GetNext(Point current, Point prior, char pipe)
        {
            return GetSurrounding(current, pipe).Cast<Point?>().FirstOrDefault(it => !it.Equals(prior));
        }

        private Point[] GetSurrounding(Point point, char pipe)
        {
            if (pipe == '|')
                return new[] { point.MoveNorth(), point.MoveNorth(-1) };
            if (pipe == '-')
                return new[] { point.MoveEast(), point.MoveEast(-1) };
            if (pipe == 'L')
                return new[] { point.MoveNorth(), point.MoveEast() };
            if (pipe == 'J')
                return new[] { point.MoveNorth(), point.MoveEast(-1) };
            if (pipe == '7')
                return new[] { point.MoveNorth(-1), point.MoveEast(-1) };
            if (pipe == 'F')
                return new[] { point.MoveNorth(-1), point.MoveEast(1) };

            return Array.Empty<Point>(); 
        }

        private Point[] GetLeft(Point prior, Point current)
        {
            if (current.Equals(prior.MoveNorth()))
                return new[] { prior.MoveEast(-1), current.MoveEast(-1) };
            if (current.Equals(prior.MoveEast()))
                return new[] { prior.MoveNorth(), current.MoveNorth() };
            if (current.Equals(prior.MoveNorth(-1)))
                return new[] { prior.MoveEast(), current.MoveEast() };
            if (current.Equals(prior.MoveEast(-1)))
                return new[] { prior.MoveNorth(-1), current.MoveNorth(-1) };

            throw new Exception("Points are not adjacent");
        }
    }
}
