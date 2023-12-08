using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var moves = chunks[0];
            var nodes = Parser.ToArrayOf(chunks[1], it => new Node(it)).ToDictionary(it => it.Name);
            foreach (var item in nodes)
                item.Value.AssignNodes(nodes);

            var current = nodes["AAA"];
            var step = 0;

            while(current.Name != "ZZZ")
            {
                current = moves[step % moves.Length] == 'L' ? current.Left : current.Right;
                step++;
            }

            return step;
        }

        [Solution(8, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var moves = chunks[0];
            var nodes = Parser.ToArrayOf(chunks[1], it => new Node(it)).ToDictionary(it => it.Name);
            foreach (var item in nodes)
                item.Value.AssignNodes(nodes);

            var current = nodes.Values.Where(it => it.Name.EndsWith('A')).ToArray();
            var finishedSteps = new List<long>();
            var cycles = new List<Cycle>();

            foreach(var item in current)
            {
                var currentItem = item;
                var step = 0L;
                var lead = 0L;
                while (true)
                {
                    if (currentItem.Name.EndsWith('Z'))
                    {
                        if (lead == 0)
                            lead = step;
                        else
                            break;
                    }

                    currentItem = moves[(int)(step % moves.Length)] == 'L' ? currentItem.Left : currentItem.Right;
                    step++;
                }

                var loops = (step / 2) / moves.Length;
                var test = (step / 2) % moves.LongCount();

                cycles.Add(new Cycle(item.Name, loops));
            }

            var lcm = cycles[0].Loops;
            var test2 = cycles.Select(it => Primes.IsPrime(it.Loops)).ToArray();

            foreach (var item in cycles.Skip(1))
            {
                var newLcm = lcm;
                while (true)
                {
                    if(newLcm % item.Loops == 0)
                        break;

                    newLcm += lcm;
                }
                lcm = newLcm;
            }

            return lcm * moves.Length;
        }

        private class Node
        {
            public string Name { get; }
            public string LeftName { get; }
            public string RightName { get; }
            public Node Left { get; private set; }
            public Node Right { get; private set; }

            public Node(string input)
            {
                var chunks = Parser.SplitOn(input, ' ', '(', ')', ',');
                Name = chunks[0];
                LeftName = chunks[2];
                RightName = chunks[3];
                Left = this;
                Right = this;
            }

            public void AssignNodes(Dictionary<string, Node> nodes)
            {
                Left = nodes[LeftName];
                Right = nodes[RightName];
            }
        }

        private class State
        {
            public string Node { get; }
            public int Step { get; }
            public long TotalSteps { get; }

            public State(string node, int step,  long totalSteps)
            {
                this.Node = node;
                this.Step = step;
                this.TotalSteps = totalSteps;
            }

            public bool Equals(State other)
            {
                return Node == other.Node && Step == other.Step && TotalSteps == other.TotalSteps;
            }
        }

        private class Cycle
        {
            public string StartingNode { get; }
            public long Loops { get; }

            public Cycle(string startingNode,  long loops)
            {
                StartingNode = startingNode;
                Loops = loops;
            }
        }
    }
}
