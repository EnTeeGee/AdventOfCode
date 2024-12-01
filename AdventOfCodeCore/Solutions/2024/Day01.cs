using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            var columns = Parser.ToArrayOfString(input)
                .SelectMany(it => Parser.SplitOnSpace(it))
                .Select((it, index) => new { val = int.Parse(it), col = index % 2 })
                .GroupBy(it => it.col, it => it.val);

            return columns.First().OrderBy(it => it).Zip(columns.Last().OrderBy(it => it), (a, b) => Math.Abs(a - b)).Sum();
        }

        [Solution(1, 2)]
        public long Solution2(string input)
        {
            var columns = Parser.ToArrayOfString(input)
                .SelectMany(it => Parser.SplitOnSpace(it))
                .Select((it, index) => new { val = long.Parse(it), col = index % 2 })
                .GroupBy(it => it.col, it => it.val);

            var checkDict = columns.Last().GroupBy(it => it).ToDictionary(it => it.Key, it => it.Count());

            return columns.First().Sum(it => checkDict.ContainsKey(it) ? it * checkDict[it] : 0);
        }
    }
}
