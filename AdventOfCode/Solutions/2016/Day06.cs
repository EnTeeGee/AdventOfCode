using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2016
{
    class Day06
    {
        [Solution(6, 1)]
        public string Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return new string(Enumerable.Range(0, lines[0].Length).Select(it => lines.Select(l => l[it]).GroupBy(c => c).OrderByDescending(c => c.Count()).First().Key).ToArray());
        }

        [Solution(6, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return new string(Enumerable.Range(0, lines[0].Length).Select(it => lines.Select(l => l[it]).GroupBy(c => c).OrderBy(c => c.Count()).First().Key).ToArray());
        }
    }
}
