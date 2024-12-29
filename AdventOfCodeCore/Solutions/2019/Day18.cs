using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    internal class Day18
    {
        [Solution(18, 1)]
        public int Solution1(string input)
        {
            var (open, keys, doors, robot) = GetMap(input);

            var paths = new Dictionary<char, (char target, char[] doors, int steps)[]>();
            paths.Add('@', GenerateFor(robot, open, keys, doors));
            foreach (var item in keys)
                paths.Add(item.Value, GenerateFor(item.Key, open, keys, doors));

            var boundary = new PriorityQueueHeap<State>();
            boundary.Add(new State('@', 0, Array.Empty<char>()), 0);
            var seen = new HashSet<string>();

            while (boundary.Any())
            {
                var current = boundary.PopMax();
                var currentId = current!.GetId();
                if (seen.Contains(currentId))
                    continue;

                if (current.Collected.Length == keys.Count)
                    return current.Steps;

                seen.Add(currentId);
                var next = current.GetFurtherMoves(paths[current.Current]);
                foreach (var item in next)
                    boundary.Add(item, -item.Steps);
            }

            throw new Exception("Failed to find valid path");
        }

        [Solution(18, 2)]
        public int Solution2(string input)
        {
            var (open, keys, doors, robot) = GetMap(input);

            var robots = new[]
            {
                new Point(-1, -1),
                new Point(1, -1),
                new Point(1, 1),
                new Point(-1, 1)
            }.Select(it => robot + it).ToArray();

            var paths = new Dictionary<char, (char target, char[] doors, int steps)[]>();

            // Upper left
            var (openSubset, keysSubset, doorsSubset) = GetSubSet(open, keys, doors, (Point pos) => pos.X < robot.X && pos.Y < robot.Y);
            paths.Add('0', GenerateFor(robots[0], openSubset, keysSubset, doorsSubset));
            foreach (var item in keysSubset)
                paths.Add(item.Value, GenerateFor(item.Key, openSubset, keysSubset, doorsSubset));

            // Upper right
            (openSubset, keysSubset, doorsSubset) = GetSubSet(open, keys, doors, (Point pos) => pos.X > robot.X && pos.Y < robot.Y);
            paths.Add('1', GenerateFor(robots[1], openSubset, keysSubset, doorsSubset));
            foreach (var item in keysSubset)
                paths.Add(item.Value, GenerateFor(item.Key, openSubset, keysSubset, doorsSubset));

            // Lower right
            (openSubset, keysSubset, doorsSubset) = GetSubSet(open, keys, doors, (Point pos) => pos.X > robot.X && pos.Y > robot.Y);
            paths.Add('2', GenerateFor(robots[2], openSubset, keysSubset, doorsSubset));
            foreach (var item in keysSubset)
                paths.Add(item.Value, GenerateFor(item.Key, openSubset, keysSubset, doorsSubset));

            // Lower left
            (openSubset, keysSubset, doorsSubset) = GetSubSet(open, keys, doors, (Point pos) => pos.X < robot.X && pos.Y > robot.Y);
            paths.Add('3', GenerateFor(robots[3], openSubset, keysSubset, doorsSubset));
            foreach (var item in keysSubset)
                paths.Add(item.Value, GenerateFor(item.Key, openSubset, keysSubset, doorsSubset));

            var boundary = new PriorityQueueHeap<QuadState>();
            boundary.Add(new QuadState("0123".ToArray(), 0, Array.Empty<char>()), 0);
            var seen = new HashSet<string>();

            while (boundary.Any())
            {
                var current = boundary.PopMax();
                var currentId = current!.GetId();

                if (seen.Contains(currentId))
                    continue;
                seen.Add(currentId);

                if (current.Collected.Length == keys.Count)
                    return current.Steps;
                
                foreach(var bot in current.Current)
                {
                    var next = current.GetFurtherMoves(bot, paths[bot]);
                    foreach(var item in next)
                        boundary.Add(item, -item.Steps);
                }
            }

            throw new Exception("Failed to find valid path");
        }

        private (HashSet<Point> open, Dictionary<Point, char> keys, Dictionary<Point, char> doors, Point robot) GetMap(string input)
        {
            var open = new HashSet<Point>();
            var keys = new Dictionary<Point, char>();
            var doors = new Dictionary<Point, char>();
            var robot = Point.Origin;

            var lines = Parser.ToArrayOfString(input);

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var symbol = lines[y][x];
                    var point = new Point(x, y);

                    if (symbol == '#')
                        continue;
                    open.Add(point);

                    if (symbol == '@')
                        robot = point;
                    else if (char.IsUpper(symbol))
                        doors.Add(point, symbol);
                    else if (char.IsLower(symbol))
                        keys.Add(point, symbol.ToString().ToUpper()[0]);
                }
            }

            return (open, keys, doors, robot);
        }

        private (HashSet<Point> open, Dictionary<Point, char> keys, Dictionary<Point, char> doors) GetSubSet(
            HashSet<Point> open,
            Dictionary<Point, char> keys,
            Dictionary<Point, char> doors,
            Func<Point, bool> filterFunc)
        {
            var openSubset = open.Where(it => filterFunc(it)).ToHashSet();
            var keysSubset = keys.Where(it => filterFunc(it.Key)).ToDictionary(it => it.Key, it => it.Value);
            var doorsSubSet = doors.Where(it => filterFunc(it.Key)).ToDictionary(it => it.Key, it => it.Value);

            return (openSubset, keysSubset, doorsSubSet);
        }

        private (char target, char[] doors, int steps)[] GenerateFor(
            Point source,
            HashSet<Point> open,
            Dictionary<Point, char> keys,
            Dictionary<Point, char> doors)
        {
            var seen = new HashSet<Point>();
            var boundary = new Queue<(Point pos, int steps, char[] doors)>();
            seen.Add(source);
            boundary.Enqueue((source, 0, Array.Empty<char>()));
            var output = new List<(char, char[], int)>();

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                var next = current.pos.GetSurrounding4().Where(it => open.Contains(it) && !seen.Contains(it));

                foreach(var item in next)
                {
                    seen.Add(item);

                    if (doors.ContainsKey(item))
                        boundary.Enqueue((item, current.steps + 1, current.doors.Append(doors[item]).ToArray()));
                    else
                    {
                        if (keys.ContainsKey(item))
                            output.Add((keys[item], current.doors, current.steps + 1));

                        boundary.Enqueue((item, current.steps + 1, current.doors));
                    }
                }
            }

            return output.ToArray();
        }

        private class State
        {
            public char Current { get; }
            public int Steps { get; }
            public char[] Collected { get; }

            public State(char current, int steps, char[] collected)
            {
                Current = current;
                Steps = steps;
                Collected = collected;
            }

            public string GetId()
            {
                return $"{Current}-{string.Join(string.Empty, Collected.OrderBy(it => it))}";
            }

            public State[] GetFurtherMoves((char target, char[] doors, int steps)[] input)
            {
                var valid = input.Where(it => !Collected.Contains(it.target) && !it.doors.Any(it2 => !Collected.Contains(it2)));

                return valid.Select(it => new State(it.target, Steps + it.steps, Collected.Append(it.target).ToArray())).ToArray();
            }
        }

        private class QuadState
        {
            public char[] Current { get; }
            public int Steps { get; }
            public char[] Collected { get; }

            public QuadState(char[] current, int steps, char[] collected)
            {
                Current = current;
                Steps = steps;
                Collected = collected;
            }

            public string GetId()
            {
                return $"{string.Join(string.Empty, Current.OrderBy(it => it))}-{string.Join(string.Empty, Collected.OrderBy(it => it))}";
            }

            public QuadState[] GetFurtherMoves(char source, (char target, char[] doors, int steps)[] input)
            {
                var valid = input.Where(it => !Collected.Contains(it.target) && !it.doors.Any(it2 => !Collected.Contains(it2)));

                return valid.Select(it => new QuadState(
                    Current.Where(it => it != source).Append(it.target).ToArray(),
                    Steps + it.steps,
                    Collected.Append(it.target).ToArray())).ToArray();
            }
        }
    }
}
