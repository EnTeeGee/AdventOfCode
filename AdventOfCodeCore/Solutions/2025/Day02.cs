using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day02
    {

        [Solution(2, 1)]
        public long Solution1(string input)
        {
            var ranges = Parser.ToArrayOfString(input)
                .SelectMany(it => Parser.SplitOn(it, ',').Select(it2 => new Range(it2)))
                .ToArray();

            var endStr = ranges.Max(it => it.End).ToString();
            var limit = int.Parse(endStr.Substring(0, (int)Math.Ceiling(endStr.Length / 2.0)));

            return Enumerable.Range(1, limit)
                .Select(it => long.Parse($"{it.ToString()}{it.ToString()}"))
                .Where(it => ranges.Any(it2 => it2.Contains(it)))
                .Sum();
        }

        [Solution(2, 2)]
        public long Solution2(string input)
        {
            var ranges = Parser.ToArrayOfString(input)
                .SelectMany(it => Parser.SplitOn(it, ',').Select(it2 => new Range(it2)))
                .ToArray();

            var endStr = ranges.Max(it => it.End).ToString();
            var output = new HashSet<long>();

            for(var i = 2; i <= endStr.Length; i++)
            {
                var limit = int.Parse(endStr.Substring(0, (int)Math.Ceiling(endStr.Length / (double)i)));
                output.UnionWith(Enumerable.Range(1, limit)
                    .Select(it => long.Parse(string.Join(string.Empty, Enumerable.Repeat(it.ToString(), i))))
                    .Where(it => ranges.Any(it2 => it2.Contains(it))));
            }

            return output.Sum();
        }

        private struct Range
        {
            public long Start { get; }
            public long End { get; }
            public Range(string input)
            {
                var chunks = Parser.SplitOn(input, '-');
                Start = long.Parse(chunks[0]);
                End = long.Parse(chunks[1]);
            }

            public bool Contains(long value)
            {
                return Start <= value && End >= value;
            }
        }
    }
}
