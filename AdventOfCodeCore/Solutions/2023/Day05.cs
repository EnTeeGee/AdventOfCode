using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day05
    {
        [Solution(5, 1)]
        public long Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var seeds = Parser.SplitOnSpace(chunks[0]).Skip(1).Select(it => long.Parse(it)).ToArray();
            var maps = chunks
                .Skip(1)
                .Select(it => Parser.ToArrayOfString(it)
                    .Skip(1)
                    .Select(it2 => Parser.SplitOnSpace(it2).Select(it3 => long.Parse(it3)).ToArray())
                    .Select(it2 => new { Source = it2[1], Dest = it2[0], Length = it2[2] })
                    .ToArray())
                .ToArray();

            var min = long.MaxValue;
            foreach(var item in seeds)
            {
                var current = item;
                foreach(var map in maps)
                {
                    var match = map.FirstOrDefault(it => current >= it.Source && current < (it.Source + it.Length));
                    if (match != null)
                        current += (match.Dest - match.Source);
                }

                if (current < min)
                    min = current;
            }

            return min;
        }

        [Solution(5, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var seeds = Parser.SplitOnSpace(chunks[0])
                .Skip(1)
                .Select((it, i) => new { Index = i / 2, Val = long.Parse(it) })
                .GroupBy(it => it.Index, it => it.Val)
                .Select(it => new Range(it.First(), it.First() + it.Last() - 1))
                .ToArray();
            var maps = chunks
                .Skip(1)
                .Select(it => Parser.ToArrayOfString(it)
                    .Skip(1)
                    .Select(it2 => Parser.SplitOnSpace(it2).Select(it3 => long.Parse(it3)).ToArray())
                    .Select(it2 => new MapRange(it2[1], it2[0], it2[2]))
                    .ToArray())
                .ToArray();

            foreach(var item in maps)
            {
                var updatedSeeds = new List<Range>();
                foreach(var seed in seeds)
                {
                    updatedSeeds.AddRange(UpdateSeedRanges(seed, item));
                }

                seeds = updatedSeeds.ToArray();
            }

            return seeds.Min(it => it.Start);
        }

        private Range[] UpdateSeedRanges(Range seed, MapRange[] mapRanges)
        {
            var output = new List<Range>();
            foreach(var item in mapRanges)
            {
                if(item.Start <= seed.Start && item.End >= seed.End)
                {
                    output.Add(new Range(seed.Start + item.Offset, seed.End + item.Offset));
                    seed = new Range(0, 0);
                    break;
                }
                if(item.Start <= seed.Start && item.End >= seed.End && item.End < seed.End)
                {
                    output.Add(new Range(seed.Start + item.Offset, item.End + item.Offset));
                    seed = new Range(item.End + 1, seed.End);
                }
                else if (item.Start > seed.Start && item.Start <= seed.End && item.End >= seed.End)
                {
                    output.Add(new Range(item.Start + item.Offset, seed.End + item.Offset));
                    seed = new Range(seed.Start, item.Start - 1);
                }
                else if(item.Start > seed.Start && item.End < seed.End)
                {
                    output.Add(new Range(item.Start + item.Offset, item.End + item.Offset));
                    var front = new Range(seed.Start, item.Start - 1);
                    output.AddRange(UpdateSeedRanges(front, mapRanges.Where(it => it != item).ToArray()));
                    var back = new Range(item.End + 1, seed.End);
                    output.AddRange(UpdateSeedRanges(back, mapRanges.Where(it => it != item).ToArray()));
                    seed = new Range(0, 0);
                    break;
                }
            }

            if(seed.Start != seed.End)
                output.Add(seed);

            return output.ToArray();
        }

        private class Range
        {
            public long Start { get; }
            public long End { get; }

            public Range(long start, long end)
            {
                Start = start;
                End = end;
            }
        }

        private class MapRange
        {
            public long Start { get; }
            public long End { get; }
            public long Offset { get; }

            public MapRange(long source, long dest, long length)
            {
                Start = source;
                End = source + length - 1;
                Offset = dest - source;
            }
        }
    }
}
