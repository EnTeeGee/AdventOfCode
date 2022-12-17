using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day17
    {
        [Solution(17, 1)]
        public long Solution1(string input)
        {
            var directions = input.Trim().Select(it => it == '<' ? Jet.Left : Jet.Right).ToArray();
            var shapes = new Func<Point, Shape>[] { 
                it => Shape.GetHori(it), 
                it => Shape.GetCross(it), 
                it => Shape.GetCurve(it), 
                it => Shape.GetVert(it), 
                it => Shape.GetBox(it) };
            var directionIndex = 0;
            var landed = new HashSet<Point>();
            var peak = 0L;

            for(var i = 0; i < 2022; i++)
            {
                var shape = shapes[i % shapes.Length].Invoke(new Point(2, peak + 4));
                while (true)
                {
                    shape.TryMoveHori(directions[directionIndex % directions.Length], landed);
                    directionIndex += 1;
                    var moving = shape.TryMoveDown(landed);
                    if (!moving)
                    {
                        peak = Math.Max(shape.Points.Max(it => it.Y), peak);
                        foreach (var item in shape.Points)
                            landed.Add(item);

                        break;
                    }
                }
            }

            return peak;
        }

        [Solution(17, 2)]
        public long Solution2(string input)
        {
            var directions = input.Trim().Select(it => it == '<' ? Jet.Left : Jet.Right).ToArray();
            var shapes = new Func<Point, Shape>[] {
                it => Shape.GetHori(it),
                it => Shape.GetCross(it),
                it => Shape.GetCurve(it),
                it => Shape.GetVert(it),
                it => Shape.GetBox(it) };
            var directionIndex = 0L;
            var shapeIndex = 0L;
            var landed = new HashSet<Point>();
            var peak = 0L;

            var target = 1000000000000;
            var seenStates = new HashSet<State>();

            while (shapeIndex < target)
            {
                var state = new State(
                    GetSurfaceMap(landed, new Point(2, peak + 1)),
                    (int)(shapeIndex % shapes.Length),
                    (int)(directionIndex % directions.Length),
                    peak,
                    shapeIndex,
                    directionIndex);
                if (seenStates != null && seenStates.Contains(state))
                {
                    seenStates.TryGetValue(state, out var prior);
                    var peakDist = peak - prior.PeakAt;
                    var shapeDist = shapeIndex - prior.ShapeAt;
                    var steps = (target - shapeIndex) / shapeDist;

                    seenStates = null;
                    landed = GetSurfaceMap(landed, new Point(2, peak + 1), false).Select(it => it.MoveNorth(peakDist * steps)).ToHashSet();

                    shapeIndex += (shapeDist * steps);
                    peak += (peakDist * steps);
                }

                seenStates?.Add(state);

                var shape = shapes[shapeIndex % shapes.Length].Invoke(new Point(2, peak + 4));
                shapeIndex += 1;
                while (true)
                {
                    shape.TryMoveHori(directions[directionIndex % directions.Length], landed);
                    directionIndex += 1;
                    var moving = shape.TryMoveDown(landed);
                    if (!moving)
                    {
                        peak = Math.Max(shape.Points.Max(it => it.Y), peak);
                        foreach (var item in shape.Points)
                            landed.Add(item);

                        break;
                    }
                }
            }

            return peak;
        }

        private Point[] GetSurfaceMap(HashSet<Point> points, Point source, bool zero = true)
        {
            var boundary = new Queue<Point>();
            var seen = new HashSet<Point>();
            var limits = new List<Point>();
            boundary.Enqueue(source);
            seen.Add(source);

            while (boundary.Any())
            {
                var item = boundary.Dequeue();
                var targets = new[] { item.MoveEast(), item.MoveEast(-1), item.MoveNorth(-1) }.Where(it => it.X >= 0 && it.X <= 6 && !seen.Contains(it)).ToArray();
                var toAdd = targets.Where(it => points.Contains(it) || it.Y == 0).ToArray();
                limits.AddRange(toAdd);
                foreach (var it in targets)
                {
                    seen.Add(it);
                    if (!toAdd.Contains(it))
                        boundary.Enqueue(it);
                }
            }

            var peakY = limits.Max(it => it.Y);

            return limits.Select(it => zero ? it.MoveNorth(-peakY) : it).OrderBy(it => it.Y).ThenBy(it => it.X).ToArray();
        }

        private void Draw(HashSet<Point> points)
        {
            for(var i = points.Max(it => it.Y); i > 0; i--)
            {
                var builder = new StringBuilder();

                for (var j = 0; j < 7; j++)
                {
                    if (points.Contains(new Point(j, i)))
                        builder.Append('#');
                    else
                        builder.Append('.');
                }

                Console.WriteLine(builder.ToString());
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private enum Jet { Left, Right }

        private class Shape
        {
            public Point[] Points { get; private set; }

            private Shape(Point[] points)
            {
                Points = points;
            }

            public void TryMoveHori(Jet direction, HashSet<Point> landed)
            {
                if (direction == Jet.Left && Points.Any(it => it.X == 0))
                    return;

                if (direction == Jet.Right && Points.Any(it => it.X == 6))
                    return;

                var newPoints = Points.Select(it => it.MoveEast(direction == Jet.Left ? -1 : 1)).ToArray();

                if (newPoints.Any(it => landed.Contains(it)))
                    return;

                Points = newPoints;
            }

            public bool TryMoveDown(HashSet<Point> landed)
            {
                if (Points.Any(it => it.Y == 1))
                    return false;

                var newPoints = Points.Select(it => it.MoveNorth(-1)).ToArray();
                if (newPoints.Any(it => landed.Contains(it)))
                    return false;

                Points = newPoints;

                return true;
            }

            public static Shape GetHori(Point origin)
            {
                return new Shape(new[] {
                    origin,
                    origin.MoveEast(),
                    origin.MoveEast(2),
                    origin.MoveEast(3)});
            }

            public static Shape GetCross(Point origin)
            {
                return new Shape(new[] {
                    origin.MoveEast(),
                    origin.MoveNorth(),
                    new Point(origin.X + 1, origin.Y + 1),
                    new Point(origin.X + 1, origin.Y + 2),
                    new Point(origin.X + 2, origin.Y + 1)});
            }

            public static Shape GetCurve(Point origin)
            {
                return new Shape(new[] {
                    origin,
                    origin.MoveEast(),
                    origin.MoveEast(2),
                    new Point(origin.X + 2, origin.Y + 1),
                    new Point(origin.X + 2, origin.Y + 2)});
            }

            public static Shape GetVert(Point origin)
            {
                return new Shape(new[] {
                    origin,
                    origin.MoveNorth(),
                    origin.MoveNorth(2),
                    origin.MoveNorth(3)});
            }

            public static Shape GetBox(Point origin)
            {
                return new Shape(new[] {
                    origin,
                    origin.MoveEast(),
                    origin.MoveNorth(),
                    new Point(origin.X + 1, origin.Y + 1)});
            }
        }

        private class State
        {
            public Point[] Surface { get; }
            public int ShapeIndex { get; }
            public int DirectionIndex { get; }

            public long PeakAt { get; }
            public long ShapeAt { get; }
            public long DirectionAt { get; }

            private int hash;

            public State(Point[] surface, int shapeIndex, int directionIndex, long peakAt, long shapeAt, long directionAt)
            {
                Surface = surface;
                ShapeIndex = shapeIndex;
                DirectionIndex = directionIndex;
                PeakAt = peakAt;
                ShapeAt = shapeAt;
                DirectionAt = directionAt;

                hash = string.Join(",", surface.Select(it => $"{it.X},{it.Y}").ToArray()).GetHashCode() ^ ShapeIndex ^ DirectionIndex;
            }

            public override int GetHashCode()
            {
                return hash;
            }

            public override bool Equals(object obj)
            {
                var cast = obj as State;
                if (cast == null)
                    return false;

                return cast.ShapeIndex == ShapeIndex
                    && cast.DirectionIndex == DirectionIndex
                    && cast.Surface.Length == Surface.Length
                    && cast.Surface.Zip(Surface, (a, b) => a.Equals(b)).All(it => it);
            }
        }
    }
}
