using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            var discs = Parser.ToArrayOf(input, it => new Disc(it));
            var drops = Enumerable.Range(0, discs.Length)
                .Select(it => ((discs[it].Size * 2) - discs[it].Offset - it - 1) % discs[it].Size)
                .ToArray();

            while(drops.Distinct().Count() > 1)
            {
                var toBump = Array.IndexOf(drops, drops.Min());
                drops[toBump] += discs[toBump].Size;
            }

            return drops[0];
        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            var discs = Parser.ToArrayOf(input, it => new Disc(it)).Concat(new[] { new Disc(11, 0) }).ToArray();
            var drops = Enumerable.Range(0, discs.Length)
                .Select(it => ((discs[it].Size * 2) - discs[it].Offset - it - 1) % discs[it].Size)
                .ToArray();

            while (drops.Distinct().Count() > 1)
            {
                var toBump = Array.IndexOf(drops, drops.Min());
                drops[toBump] += discs[toBump].Size;
            }

            return drops[0];
        }

        private struct Disc
        {
            public int Size { get; }
            public int Offset { get; }

            public Disc(string input)
            {
                var chunks = Parser.SplitOn(input, ' ', '.');
                Size = int.Parse(chunks[3]);
                Offset = int.Parse(chunks[11]);
            }

            public Disc(int size, int offset)
            {
                Size = size;
                Offset = offset;
            }
        }
    }
}
