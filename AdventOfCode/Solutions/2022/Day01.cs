using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfGroups(input)
                .Select(it => Parser.ToArrayOfInt(it).Sum())
                .Max();
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfGroups(input)
                .Select(it => Parser.ToArrayOfInt(it).Sum())
                .OrderByDescending(it => it)
                .Take(3)
                .Sum();
        }
    }
}
