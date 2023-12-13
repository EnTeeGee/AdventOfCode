using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfGroups(input)
                .Select(it => Parser.ToArrayOfString(it))
                .Select(it => FindReflection(it) ?? (100 * FindReflection(RotateMap(it))))
                .Sum()
                !.Value;
        }

        [Solution(13, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfGroups(input)
                .Select(it => Parser.ToArrayOfString(it))
                .Select(it => FindSmudged(it) ?? (100 * FindSmudged(RotateMap(it))))
                .Sum()
                !.Value;
        }

        private string[] RotateMap(string[] input)
        {
            return Enumerable.Range(0, input[0].Length)
                .Select(it => new string(input.Select(l => l[it]).ToArray()))
                .Reverse()
                .ToArray();
        }

        private int? FindReflection(string[] map)
        {
            return Enumerable.Range(1, map[0].Length - 1)
                .Cast<int?>()
                .FirstOrDefault(it => map
                    .Select(l => l[..it!.Value].Reverse()
                        .Zip(l[it.Value..], (a, b) => a == b)
                        .All(c => c))
                    .All(l => l));
        }

        private int? FindSmudged(string[] map)
        {
            return Enumerable.Range(1, map[0].Length - 1)
                .Cast<int?>()
                .FirstOrDefault(it => map
                    .Select(l => l[..it!.Value].Reverse()
                        .Zip(l[it.Value..], (a, b) => a == b ? 0 : 1)
                        .Sum())
                    .Sum() == 1);
        }
    }
}
