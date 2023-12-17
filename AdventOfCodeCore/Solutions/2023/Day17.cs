using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day17
    {
        [Solution(17, 1)]
        public int Solution1(string input)
        {
            var map = GetMap(input);
            var limit = new Point(map.Keys.Max(it => it.X), map.Keys.Max(it => it.Y));
            var boundary = new PriorityQueue<State>((a, b) => a.GetMetric(limit) - b.GetMetric(limit));
            boundary.Insert(new State(Point.Origin, Orientation.East, 0));
            boundary.Insert(new State(Point.Origin, Orientation.South, 0));
            var seen = new HashSet<State>();

            while (boundary.Any())
            {
                var current = boundary.Pop()!;
                if(seen.Contains(current))
                    continue;
                seen.Add(current);
                if (current.Pos.Equals(limit))
                    return current.HeatLoss;
                var next = GetNextStates(current, map, 1, 3);
                foreach (var item in next)
                    boundary.Insert(item);
            }

            throw new Exception("Ran out of valid paths");
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var map = GetMap(input);
            var limit = new Point(map.Keys.Max(it => it.X), map.Keys.Max(it => it.Y));
            var boundary = new PriorityQueue<State>((a, b) => a.GetMetric(limit) - b.GetMetric(limit));
            boundary.Insert(new State(Point.Origin, Orientation.East, 0));
            boundary.Insert(new State(Point.Origin, Orientation.South, 0));
            var seen = new HashSet<State>();

            while (boundary.Any())
            {
                var current = boundary.Pop()!;
                if (seen.Contains(current))
                    continue;
                seen.Add(current);
                if (current.Pos.Equals(limit))
                    return current.HeatLoss;
                var next = GetNextStates(current, map, 4, 7);
                foreach (var item in next)
                    boundary.Insert(item);
            }

            throw new Exception("Ran out of valid paths");
        }

        private Dictionary<Point, int> GetMap(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var output = new Dictionary<Point, int>();
            for(var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for(var x = 0; x < line.Length; x++)
                    output.Add(new Point(x, y), int.Parse(line[x].ToString()));
            }

            return output;
        }

        private State[] GetNextStates(State state, Dictionary<Point, int> map, int min, int range)
        {
            return new[] { state.Dir.RotateClockwise(), state.Dir.RotateAntiClock() }
                .SelectMany(it => Enumerable.Range(min, range).Select(r => new { dir = it, pos = state.Pos.MoveOrient(it, r), dist = r }))
                .Where(it => map.ContainsKey(it.pos))
                .Select(it => new State(it.pos, it.dir, state.HeatLoss + Enumerable.Range(1, it.dist).Sum(r => map[state.Pos.MoveOrient(it.dir, r)])))
                .ToArray();
        }

        private class State
        {
            public Point Pos { get; }
            public Orientation Dir { get; }
            public int HeatLoss { get; }

            public State(Point pos, Orientation dir, int heatLoss)
            {
                Pos = pos;
                Dir = dir;
                HeatLoss = heatLoss;
            }

            public int GetMetric(Point target)
            {
                return HeatLoss + (int)Pos.GetTaxiCabDistanceTo(target);
            }

            public override bool Equals(object? obj)
            {
                if (obj is not State cast)
                    return false;

                return Pos.Equals(cast.Pos) && Dir == cast.Dir;
            }

            public override int GetHashCode()
            {
                return Pos.GetHashCode() ^ Dir.GetHashCode();
            }
        }
    }
}
