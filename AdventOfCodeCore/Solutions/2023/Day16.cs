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

            var boundary = new Queue<Step>();
            boundary.Enqueue(new Step(Point.Origin, Orientation.East));
            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var newSteps = IterateMap(current.Pos, current.Dir, map, state);
                foreach(var item in newSteps)
                    boundary.Enqueue(item);
            }

            return state.VertBeams.Concat(state.HoriBeams).Distinct().Count();
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            var map = new Map(input);
            var state = new State();
            var max = 0;

            for(var y = 0; y < map.Bounds.Y; y++)
            {
                var boundary = new Queue<Step>();
                boundary.Enqueue(new Step(new Point(0, y), Orientation.East));
                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    var newSteps = IterateMap(current.Pos, current.Dir, map, state);
                    foreach (var item in newSteps)
                        boundary.Enqueue(item);
                }
                max = Math.Max(max, state.VertBeams.Concat(state.HoriBeams).Distinct().Count());
                state.Clear();

                boundary.Enqueue(new Step(new Point(map.Bounds.X - 1, y), Orientation.West));
                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    var newSteps = IterateMap(current.Pos, current.Dir, map, state);
                    foreach (var item in newSteps)
                        boundary.Enqueue(item);
                }
                max = Math.Max(max, state.VertBeams.Concat(state.HoriBeams).Distinct().Count());
                state.Clear();
            }

            for(var x = 0; x < map.Bounds.X; x++)
            {
                var boundary = new Queue<Step>();
                boundary.Enqueue(new Step(new Point(x, 0), Orientation.South));
                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    var newSteps = IterateMap(current.Pos, current.Dir, map, state);
                    foreach (var item in newSteps)
                        boundary.Enqueue(item);
                }
                max = Math.Max(max, state.VertBeams.Concat(state.HoriBeams).Distinct().Count());
                state.Clear();

                state = new State();
                boundary.Enqueue(new Step(new Point(x, map.Bounds.Y - 1), Orientation.North));
                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    var newSteps = IterateMap(current.Pos, current.Dir, map, state);
                    foreach (var item in newSteps)
                        boundary.Enqueue(item);
                }
                max = Math.Max(max, state.VertBeams.Concat(state.HoriBeams).Distinct().Count());
                state.Clear();
            }

            return max;
        }

        private Step[] IterateMap(Point pos, Orientation dir, Map map, State state)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X >= map.Bounds.X || pos.Y >= map.Bounds.Y)
                return Array.Empty<Step>();

            var covered = (dir.IsVert() && state.VertBeams.Contains(pos)) || (dir.IsHori() && state.HoriBeams.Contains(pos));
            if(!covered)
                (dir.IsVert() ? state.VertBeams : state.HoriBeams).Add(pos);

            if (map.ForwardMirrors.Contains(pos))
            {
                var newDir = (dir == Orientation.North ? Orientation.East :
                    dir == Orientation.West ? Orientation.South :
                    dir == Orientation.South ? Orientation.West :
                    Orientation.North);

                return new[] { new Step(pos.MoveOrient(newDir), newDir) }; 
            }
            else if (map.BackMirrors.Contains(pos))
            {
                var newDir = (dir == Orientation.North ? Orientation.West :
                    dir == Orientation.West ? Orientation.North :
                    dir == Orientation.South ? Orientation.East :
                    Orientation.South);

                return new[] { new Step(pos.MoveOrient(newDir), newDir) }; 
            }
            else if (map.VertSplitters.Contains(pos) && dir.IsHori())
                return new[] { new Step(pos.MoveNorth(), Orientation.North), new Step(pos.MoveNorth(-1), Orientation.South) }; 
            else if (map.HoriSplitters.Contains(pos) && dir.IsVert())
                return new[] { new Step(pos.MoveEast(), Orientation.East), new Step(pos.MoveEast(-1), Orientation.West) }; 
            else if(!covered)
                return new[] { new Step(pos.MoveOrient(dir), dir) };

            return Array.Empty<Step>();
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
        
        private struct Step
        {
            public Point Pos { get; }
            public Orientation Dir { get; }

            public Step(Point pos, Orientation dir)
            {
                Pos = pos;
                Dir = dir;
            }
        }
    }
}
