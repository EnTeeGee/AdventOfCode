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
            var steps = new List<long>();

            foreach(var item in current)
            {
                var currentItem = item;
                var step = 0L;
                while (true)
                {
                    if (currentItem.Name.EndsWith('Z'))
                    {
                        steps.Add(step);
                        break;
                    }

                    currentItem = moves[(int)(step % moves.Length)] == 'L' ? currentItem.Left : currentItem.Right;
                    step++;
                }
            }

            var lcm = steps[0];

            foreach(var item in steps.Skip(1))
            {
                var newLcm = lcm;
                while (true)
                {
                    if (newLcm % item == 0)
                        break;
                    newLcm += lcm;
                }
                lcm = newLcm;
            }

            return lcm;
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
    }
}
