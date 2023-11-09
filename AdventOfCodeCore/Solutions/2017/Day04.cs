using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it)
                    .GroupBy(x => x)
                    .Select(x => x.Count())
                    .All(x => x == 1))
                .Count(it => it);
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it)
                    .Select(x => new string(x.OrderBy(y => y).ToArray()))
                    .GroupBy(x => x)
                    .Select(x => x.Count())
                    .All(x => x == 1))
                .Count(it => it);
        }
    }
}
