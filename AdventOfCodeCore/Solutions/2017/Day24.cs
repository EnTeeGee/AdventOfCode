using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day24
    {
        [Solution(24, 1)]
        public int Solution1(string input)
        {
            var ports = Parser.ToArrayOfString(input).Select((it, i) => new Port(it, i)).ToArray();
            var bridges = new Queue<Port[]>();
            var max = 0;
            var starters = ports.Concat(ports.Select(it => it.Flip())).Where(it => it.FrontPort == 0).ToArray();
            foreach (var item in starters)
                bridges.Enqueue(new[] { item });

            while (bridges.Any())
            {
                var current = bridges.Dequeue();
                var usedIds = current.Select(it => it.Id).ToArray();
                var options = ports
                    .Where(it => !usedIds.Contains(it.Id))
                    .SelectMany(it => new[] { it, it.Flip() })
                    .Where(it => it.FrontPort == current.Last().RearPort)
                    .ToArray();

                if (!options.Any())
                {
                    var sum = current.Sum(it => it.FrontPort + it.RearPort);
                    if (sum > max)
                        max = sum;
                }

                foreach(var item in options)
                    bridges.Enqueue(current.Concat(new[] { item }).ToArray());
            }

            return max;
        }

        [Solution(24, 2)]
        public int Solution2(string input)
        {
            var ports = Parser.ToArrayOfString(input).Select((it, i) => new Port(it, i)).ToArray();
            var bridges = new Queue<Port[]>();
            var max = 0;
            var length = 0;
            var starters = ports.Concat(ports.Select(it => it.Flip())).Where(it => it.FrontPort == 0).ToArray();
            foreach (var item in starters)
                bridges.Enqueue(new[] { item });

            while (bridges.Any())
            {
                var current = bridges.Dequeue();
                var usedIds = current.Select(it => it.Id).ToArray();
                var options = ports
                    .Where(it => !usedIds.Contains(it.Id))
                    .SelectMany(it => new[] { it, it.Flip() })
                    .Where(it => it.FrontPort == current.Last().RearPort)
                    .ToArray();

                if (!options.Any())
                {
                    if (current.Length > length)
                        max = 0;
                    else if (current.Length < length)
                        continue;

                    var sum = current.Sum(it => it.FrontPort + it.RearPort);

                    if (sum > max)
                        max = sum;
                }

                foreach (var item in options)
                    bridges.Enqueue(current.Concat(new[] { item }).ToArray());
            }

            return max;
        }

        private class Port
        {
            public int Id { get; }
            public int FrontPort { get; }
            public int RearPort { get; }

            public Port(string input, int id)
            {
                var items = Parser.SplitOn(input, '/');
                FrontPort = int.Parse(items[0]);
                RearPort = int.Parse(items[1]);
                Id = id;
            }

            private Port(int front, int rear, int id)
            {
                FrontPort = front;
                RearPort = rear;
                Id = id;
            }

            public Port Flip()
            {
                return new Port(RearPort, FrontPort, Id);
            }
        }
    }
}
