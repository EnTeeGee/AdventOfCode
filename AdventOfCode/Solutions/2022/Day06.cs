using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            return GetDistinctRange(input, 4);
        }

        [Solution(6, 2)]
        public int Solution2(string input)
        {
            return GetDistinctRange(input, 14);
        }

        private int GetDistinctRange(string input, int range)
        {
            return Enumerable.Range(0, input.Length - range)
                .Select(it => new { index = it + range, distinct = input.Skip(it).Take(range).Distinct().Count() })
                .First(it => it.distinct == range)
                .index;
        }
    }
}
