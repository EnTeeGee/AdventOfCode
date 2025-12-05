using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day05
    {
        [Solution(6, 1)]
        public long Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var ranges = Parser.ToArrayOfString(chunks[0]).Select(it => new Range(it)).ToArray();

            return Parser.ToArrayOf(chunks[1], it => long.Parse(it)).Count(it => ranges.Any(it2 => it2.Contains(it)));
        }

        [Solution(6, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var ranges = Parser.ToArrayOfString(chunks[0]).Select(it => new Range(it)).ToArray();

            var merged = new List<Range> { ranges[0] };
            ranges = ranges.Skip(1).ToArray();
            foreach ( var range in ranges)
            {
                var active = range;
                for(var i = 0; i < merged.Count; i++)
                {
                    if (merged[i].Overlaps(active))
                    {
                        active = merged[i].Merge(active);
                        merged.RemoveAt(i);
                        i--;
                    }
                }
                merged.Add(active);
            }

            return merged.Sum(it => (it.End - it.Start) + 1);
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

            private Range(long start, long end)
            {
                Start = start;
                End = end;
            }

            public bool Contains(long value)
            {
                return Start <= value && End >= value;
            }

            public bool Overlaps(Range other)
            {
                var min = Start < other.Start ? this : other;
                var max = Start >= other.Start ? this : other;

                return min.End >= max.Start - 1;
            }

            public Range Merge(Range other)
            {
                return new Range(Math.Min(Start, other.Start), Math.Max(End, other.End));
            }
        }
    }
}
