using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day24
    {
        [Solution(24, 1)]
        public int Solution1(string input)
        {
            return FindDist(input, false);
        }

        [Solution(24, 2)]
        public int Solution2(string input)
        {
            return FindDist(input, true);
        }

        public int FindDist(string input, bool returnToStart)
        {
            var lines = Parser.ToArrayOfString(input);
            var points = new HashSet<Point>();
            var targets = new List<(char symbol, Point pos)>();

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];

                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        continue;

                    points.Add(new Point(x, y));

                    if (line[x] != '.')
                        targets.Add((line[x], new Point(x, y)));
                }
            }

            targets = targets.OrderBy(it => it.symbol).ToList();

            var steps = Permutations.GetAllPossiblePairs(targets.ToArray())
                .Select(it => new { items = $"{it[0].symbol}{it[1].symbol}", dist = DistTo(it[0].pos, it[1].pos, points) })
                .SelectMany(it => new[] { it, new { items = new string(it.items.Reverse().ToArray()), it.dist } })
                .GroupBy(it => it.items[0])
                .ToDictionary(it => it.Key, it => it.ToArray());

            var routes = new PriorityQueueHeap<State>();
            var intitalStates = steps['0'].Select(it => new State(it.dist, it.items));
            foreach (var item in intitalStates)
                routes.Add(item, -item.Dist);

            while (routes.Any())
            {
                var current = routes.PopMax()!;
                if (!returnToStart && current.SeenTargets.Length == targets.Count)
                    return current.Dist;

                if (current.SeenTargets.Length == targets.Count + 1)
                    return current.Dist;

                var currentHead = current.SeenTargets.Last();
                var validTargets = steps[currentHead].Where(it => !current.SeenTargets.Contains(it.items[1])).ToArray();
                if (!validTargets.Any()) // Reached end, need to path back to 0
                    validTargets = steps[currentHead].Where(it => it.items[1] == '0').ToArray();

                foreach (var item in validTargets)
                {
                    var newState = new State(current.Dist + item.dist, $"{current.SeenTargets}{item.items[1]}");
                    routes.Add(newState, -newState.Dist);
                }
            }

            throw new Exception("Failed to find valid path");
        }

        private int DistTo(Point start, Point end, HashSet<Point> points)
        {
            var seen = new HashSet<Point>();
            var boundary = new Queue<(Point head, int dist)>();
            boundary.Enqueue((start, 0));
            seen.Add(start);

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                if(current.head == end)
                    return current.dist;

                var valid = current.head.GetSurrounding4().Where(it => points.Contains(it) && !seen.Contains(it)).ToArray();
                foreach(var item in valid)
                {
                    boundary.Enqueue((item, current.dist + 1));
                    seen.Add(item);
                }
            }

            throw new Exception("Unable to find valid path");
        }

        private class State
        {
            public int Dist { get; }
            public string SeenTargets { get; }

            public State(int dist, string seenTargets)
            {
                this.Dist = dist;
                this.SeenTargets = seenTargets;
            }
        }
    }
}
