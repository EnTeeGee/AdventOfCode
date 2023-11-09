using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var knotHash = Enumerable.Range(0, 256).ToArray();
            var currentPos = 0;
            var skipSize = 0;
            var lengths = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();

            foreach(var length in lengths)
            {
                var section = knotHash
                    .Skip(currentPos)
                    .Take(length)
                    .Concat(knotHash.Take(currentPos - knotHash.Length + length))
                    .Reverse()
                    .ToArray();
                for(var i = 0; i < section.Length; i++)
                    knotHash[(currentPos + i) % knotHash.Length] = section[i];

                currentPos = (currentPos + skipSize + length) % knotHash.Length;
                skipSize++;
            }

            return knotHash[0] * knotHash[1];
        }

        [Solution(10, 2)]
        public string Solution2(string input)
        {
            var knotHash = Enumerable.Range(0, 256).ToArray();
            var currentPos = 0;
            var skipSize = 0;
            var lengths = input.Select(it => (byte)it).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();
            for(var i = 0; i < 64; i++)
            {
                foreach (var length in lengths)
                {
                    var section = knotHash
                        .Skip(currentPos)
                        .Take(length)
                        .Concat(knotHash.Take(currentPos - knotHash.Length + length))
                        .Reverse()
                        .ToArray();
                    for (var j = 0; j < section.Length; j++)
                        knotHash[(currentPos + j) % knotHash.Length] = section[j];

                    currentPos = (currentPos + skipSize + length) % knotHash.Length;
                    skipSize++;
                }
            }

            return string.Join("", Enumerable.Range(0, 16).Select(it => knotHash.Skip(it * 16).Take(16).Aggregate((acc, it) => acc ^ it).ToString("x2")));
        }
    }
}
