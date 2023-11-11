using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using AdventOfCodeCore.Solutions._2017.Common;

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
            return new KnotHash(input).AsHexidecimal();
        }
    }
}
