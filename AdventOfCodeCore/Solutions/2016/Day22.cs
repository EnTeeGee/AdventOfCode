using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day22
    {
        [Solution(22, 1)]
        public int Solution1(string input)
        {
            var nodes = Parser.ToArrayOfString(input).Skip(2).Select(it => new Node(it)).ToArray();

            return nodes.Sum(it => nodes.Count(n => n != it && n.Used != 0 && n.Used <= it.Avail));
        }

        [Solution(22, 2)]
        public int Solution2(string input)
        {
            var nodes = Parser.ToArrayOfString(input).Skip(2).Select(it => new Node(it, true)).ToArray();
            var source = nodes.Where(it => it.Pos.Y == 0).OrderByDescending(it => it.Pos.X).First();
            source.IsTarget = true;

            var moves = new PriorityQueueHeap<State>();
            var initialState = new State(nodes.ToDictionary(it => it.Pos), 0);
            moves.Add(initialState, -(initialState.Moves + initialState.DistToEnd));
            var seen = new HashSet<string>();

            while (moves.Any())
            {
                var current = moves.PopMax()!;
                var state = current.GetState();
                if (seen.Contains(state))
                    continue;

                seen.Add(state);
                var options = new List<State>();
                var openNode = current.Nodes.Values.First(it => it.Used == 0);

                var validSteps = openNode.Pos.GetSurrounding4().Where(it => current.Nodes.ContainsKey(it) && current.Nodes[it].Used <= openNode.Avail).ToArray();
                foreach (var step in validSteps)
                {
                    var newNodes = current.Nodes.Values.Select(it => it.Clone()).ToDictionary(it => it.Pos);
                    newNodes[openNode.Pos].Used = newNodes[step].Used;
                    newNodes[step].Used = 0;
                    if (newNodes[step].IsTarget)
                    {
                        if (step == Point.Origin)
                            return current.Moves;

                        newNodes[openNode.Pos].IsTarget = true;
                        newNodes[step].IsTarget = false;
                    }

                    var newState = new State(newNodes, current.Moves + 1);
                    moves.Add(newState, -(newState.Moves + newState.DistToEnd));
                }
            }

            throw new Exception("Found no path");
        }

        private class Node
        {
            public Point Pos { get; }
            public int Size { get; }
            public int Used { get; set; }
            public int Avail { get { return Size - Used; } }
            public bool IsTarget { get; set; }

            public Node(string input, bool simplify = false)
            {
                var chunks = Parser.SplitOn(input, '-', ' ');
                Pos = new Point(int.Parse(chunks[1].TrimStart('x')), int.Parse(chunks[2].TrimStart('y')));

                if(simplify)
                {
                    if (chunks[3].Length == 4  || chunks[3] == "32T")
                    {
                        Size = 2;
                        Used = 2;
                    }
                    else
                    {
                        Size = 1;
                        Used = chunks[4] == "0T" ? 0 : 1;
                    }
                }
                else
                {
                    Size = int.Parse(chunks[3].TrimEnd('T'));
                    Used = int.Parse(chunks[4].TrimEnd('T'));

                    if (Avail != int.Parse(chunks[5].TrimEnd('T')))
                        throw new Exception("Invalid values");
                }
            }

            private Node(Point pos, int size, int used, bool isTarget)
            {
                Pos = pos;
                Size = size;
                Used = used;
                IsTarget = isTarget;
            }

            public Node Clone()
            {
                return new Node(Pos, Size, Used, IsTarget);
            }

            public string GetState()
            {
                return IsTarget ? Used.ToString() : $"[{Used}]";
            }
        }

        private class State
        {
            public Dictionary<Point, Node> Nodes { get; }
            public int Moves { get; }
            public int DistToEnd { get; }

            public State(Dictionary<Point, Node> nodes, int moves)
            {
                Nodes = nodes;
                Moves = moves;
                DistToEnd = (int)Nodes.Values.First(it => it.IsTarget).Pos.GetTaxiCabDistanceTo(Point.Origin);
            }

            public string GetState()
            {
                return Nodes.Values.First(it => it.IsTarget).Pos.ToString() + " " + Nodes.Values.First(it => it.Used == 0).Pos.ToString();
            }
        }
    }
}
