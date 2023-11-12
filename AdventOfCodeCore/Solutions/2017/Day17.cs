using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day17
    {
        [Solution(17, 1)]
        public int Solution1(string input)
        {
            var step = int.Parse(input);
            var node = new Node(0);

            for(var i = 1; i < 2018; i++)
            {
                for (var j = 0; j < step; j++)
                    node = node.Next;

                node.AddAfter(new Node(i));
                node = node.Next;
            }

            return node.Next.Value;
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var step = int.Parse(input);
            var index = 0;
            var valAfter = 0;

            for (var i = 1; i <= 50.Million(); i++)
            {
                index = (index + step) % i;
                if (index == 0)
                    valAfter = i;
                index++;
            }

            return valAfter;
        }

        private class Node
        {
            public Node Next { get; set; }
            public Node Prior { get; set; }
            public int Value { get; }

            public Node(int value)
            {
                Value = value;
                Next = this;
                Prior = this;
            }

            public void AddAfter(Node toAdd)
            {
                var swap = Next;
                Next = toAdd;
                toAdd.Prior = this;
                toAdd.Next = swap;
            }
        }
    }
}
