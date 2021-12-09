using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var matchingLengths = new[] { 2, 3, 4, 7 };

            return Parser
                .ToArrayOfString(input)
                .SelectMany(it => Parser.SplitOn(it, ' ', '|').Skip(10))
                .Count(it => matchingLengths.Contains(it.Length));
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => DecodeValues(it));
        }

        private int DecodeValues(string input)
        {
            var numberMapping = new string[10];
            var digits = Parser.SplitOn(input, ' ', '|').Select(it => new string(it.OrderBy(c => c).ToArray())).ToArray();
            var source = digits.Take(10).ToArray();
            var target = digits.Skip(10).ToArray();
            numberMapping[1] = source.First(it => it.Length == 2);
            numberMapping[7] = source.First(it => it.Length == 3);
            numberMapping[4] = source.First(it => it.Length == 4);
            numberMapping[8] = source.First(it => it.Length == 7);
            numberMapping[6] = source.First(it => it.Length == 6 && numberMapping[1].Any(d => !it.Contains(d)));
            numberMapping[9] = source.First(it => it.Length == 6 && numberMapping[4].All(d => it.Contains(d)));
            numberMapping[0] = source.First(it => it.Length == 6 && it != numberMapping[6] && it != numberMapping[9]);
            numberMapping[3] = source.First(it => it.Length == 5 && numberMapping[1].All(d => it.Contains(d)));
            numberMapping[5] = source.First(it => it.Length == 5 && it != numberMapping[3] && it.All(d => numberMapping[9].Contains(d)));
            numberMapping[2] = source.First(it => it.Length == 5 && it != numberMapping[3] && it != numberMapping[5]);

            return int.Parse(string.Concat(target.Select(it => Array.IndexOf(numberMapping, it).ToString())));
        }
    }
}
