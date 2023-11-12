using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ',').Skip(2).Select(x => int.Parse(x)).ToArray());
            var seen = new HashSet<int>();
            var boundary = new Queue<int>();
            boundary.Enqueue(0);
            seen.Add(0);

            while (boundary.Any())
            {
                var current = boundary.Dequeue();
                foreach(var item in lines[current])
                {
                    if (!seen.Contains(item))
                    {
                        boundary.Enqueue(item);
                        seen.Add(item);
                    }
                }
            }

            return seen.Count;
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var lines = Parser
                .ToArrayOf(input, it => Parser.SplitOn(it, ' ', ','))
                .ToDictionary(it => int.Parse(it[0]), it => it.Skip(2).Select(x => int.Parse(x)).ToArray());
            var groupCount = 0;

            while (lines.Any())
            {
                groupCount++;
                var start = lines.First();
                var boundary = new Queue<int>();
                boundary.Enqueue(start.Key);

                while (boundary.Any())
                {
                    var current = boundary.Dequeue();
                    if (!lines.ContainsKey(current))
                        continue;
                    foreach (var item in lines[current])
                    {
                        if (lines.ContainsKey(item) && item != current)
                            boundary.Enqueue(item);
                    }
                    lines.Remove(current);
                }
            }

            return groupCount;
        }
    }
}
