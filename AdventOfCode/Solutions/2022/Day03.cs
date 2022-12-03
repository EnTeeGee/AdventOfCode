using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => new { first = it.Take(it.Length / 2).ToArray(), second = it.Skip(it.Length / 2).ToArray() })
                .Select(it => it.first.First(c => it.second.Contains(c)))
                .Sum(it => char.IsLower(it) ? it - '`' : it - '&');
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select((it, index) => new { index = index / 3, bag = it.Distinct().ToArray() })
                .GroupBy(it => it.index)
                .Select(it => it.SelectMany(b => b.bag).GroupBy(c => c).First(c => c.Count() == 3).Key)
                .Sum(it => char.IsLower(it) ? it - '`' : it - '&');
        }
    }
}
