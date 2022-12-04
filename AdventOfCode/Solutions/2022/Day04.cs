using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, '-', ',').Select(l => int.Parse(l)).ToArray())
                .Count(it => ((it[0] >= it[2]) && (it[1] <= it[3])) || ((it[0] <= it[2]) && (it[1] >= it[3])));
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, '-', ',').Select(l => int.Parse(l)).ToArray())
                .Count(it => !((it[1] < it[2]) || (it[0] > it[3])));
                //.Count(it => ((it[0] <= it[2]) && (it[1] >= it[2])) || ((it[0] <= it[3]) && (it[1] >= it[3])) || ((it[0] >= it[2]) && (it[1] <= it[3])));
        }
    }
}
