using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var map = new Map(input);
            var state = new State();
            var seenBounds = new HashSet<Point>();

            var boundary = new Queue<(Point pos, Orientation dir)>();
            boundary.Enqueue((Point.Origin, Orientation.East));
            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var newSteps = IterateMap(current.pos, current.dir, map, state, seenBounds);
                foreach(var item in newSteps)
                    boundary.Enqueue(item);
            }

            return state.VertBeams.Concat(state.HoriBeams).Distinct().Count();
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            var map = new Map(input);
            var max = 0;

            var startingStates = Enumerable.Range(0, (int)map.Bounds.X).Select(it => (new Point(it, 0), Orientation.South))
                .Concat(Enumerable.Range(0, (int)map.Bounds.X).Select(it => (new Point(it, map.Bounds.Y - 1), Orientation.South)))
                .Concat(Enumerable.Range(0, (int)map.Bounds.Y).Select(it => (new Point(0, it), Orientation.East)))
                .Concat(Enumerable.Range(0, (int)map.Bounds.X).Select(it => (new Point(map.Bounds.X - 1, it), Orientation.West)))
                .ToArray();
            var seenBounds = new HashSet<Point>();

            foreach(var item in startingStates)
            {
                var outer = item.Item1.MoveOrient(item.Item2, -1);
                if (seenBounds.Contains(outer))
                    continue;
                seenBounds.Add(outer);

                var state = new State();
                var boundary = new Queue<(Point pos, Orientation dir)>();
                boundary.Enqueue(item);
                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    var newSteps = IterateMap(current.pos, current.dir, map, state, seenBounds);
                    foreach (var step in newSteps)
                        boundary.Enqueue(step);
                }
                max = Math.Max(max, state.VertBeams.Concat(state.HoriBeams).Distinct().Count());
            }

            return max;
        }

        private (Point pos, Orientation dir)[] IterateMap(Point pos, Orientation dir, Map map, State state, HashSet<Point> bounds)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= map.Bounds.X || pos.Y >= map.Bounds.Y)
            {
                if(!bounds.Contains(pos))
                    bounds.Add(pos);

                return Array.Empty<(Point, Orientation)>();
            }

            var covered = (dir.IsVert() && state.VertBeams.Contains(pos)) || (dir.IsHori() && state.HoriBeams.Contains(pos));
            if(!covered)
                (dir.IsVert() ? state.VertBeams : state.HoriBeams).Add(pos);

            if (map.ForwardMirrors.Contains(pos))
            {
                var newDir = (dir == Orientation.North ? Orientation.East :
                    dir == Orientation.West ? Orientation.South :
                    dir == Orientation.South ? Orientation.West :
                    Orientation.North);

                return new[] { (pos.MoveOrient(newDir), newDir) }; 
            }
            else if (map.BackMirrors.Contains(pos))
            {
                var newDir = (dir == Orientation.North ? Orientation.West :
                    dir == Orientation.West ? Orientation.North :
                    dir == Orientation.South ? Orientation.East :
                    Orientation.South);

                return new[] { (pos.MoveOrient(newDir), newDir) }; 
            }
            else if (map.VertSplitters.Contains(pos) && dir.IsHori())
                return new[] { (pos.MoveNorth(), Orientation.North), (pos.MoveNorth(-1), Orientation.South) }; 
            else if (map.HoriSplitters.Contains(pos) && dir.IsVert())
                return new[] { (pos.MoveEast(), Orientation.East), (pos.MoveEast(-1), Orientation.West) }; 
            else if(!covered)
                return new[] { (pos.MoveOrient(dir), dir) };

            return Array.Empty<(Point, Orientation)>();
        }

        private class Map
        {
            public HashSet<Point> ForwardMirrors { get; }
            public HashSet<Point> BackMirrors { get; }
            public HashSet<Point> VertSplitters { get; }
            public HashSet<Point> HoriSplitters { get; }
            public Point Bounds { get; }

            public Map(string input)
            {
                ForwardMirrors = new HashSet<Point>();
                BackMirrors = new HashSet<Point>();
                VertSplitters = new HashSet<Point>();
                HoriSplitters = new HashSet<Point>();

                var lines = Parser.ToArrayOfString(input);
                for(var y = 0; y < lines.Length; y++)
                {
                    var line = lines[y];
                    for(var x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '/')
                            ForwardMirrors.Add(new Point(x, y));
                        else if (line[x] == '\\')
                            BackMirrors.Add(new Point(x, y));
                        else if (line[x] == '|')
                            VertSplitters.Add(new Point(x, y));
                        else if (line[x] == '-')
                            HoriSplitters.Add(new Point(x, y));
                    }
                }
                Bounds = new Point(lines.Last().Length, lines.Length);
            }
        }

        private class State
        {
            public HashSet<Point> VertBeams { get; }
            public HashSet<Point> HoriBeams { get; }

            public State()
            {
                VertBeams = new HashSet<Point>();
                HoriBeams = new HashSet<Point>();
            }

            public void Clear()
            {
                VertBeams.Clear();
                HoriBeams.Clear();
            }
        }
    }
}
