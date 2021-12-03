using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var gammaStr = Enumerable.Range(0, lines[0].Length).Select(it => lines.Select(l => l[it]).Count(c => c == '0') > lines.Length / 2 ? '0' : '1').ToArray();
            var epsilonStr = gammaStr.Select(it => it == '0' ? '1' : '0').ToArray();

            return Convert.ToInt32(new string(gammaStr), 2) * Convert.ToInt32(new string(epsilonStr), 2);
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var remainingOxy = lines;
            var remainingCo2 = lines;

            for(var i = 0; i < lines[0].Length && remainingOxy.Length > 1; i++)
            {
                var oxyTarget = remainingOxy.Select(it => it[i]).Count(it => it == '1') < Math.Ceiling(remainingOxy.Length / 2f) ? '0' : '1';
                remainingOxy = remainingOxy.Where(it => it[i] == oxyTarget).ToArray();
            }

            for(var i = 0; i < lines[0].Length && remainingCo2.Length > 1; i++)
            {
                var co2Target = remainingCo2.Select(it => it[i]).Count(it => it == '0') <= remainingCo2.Length / 2 ? '0' : '1';
                remainingCo2 = remainingCo2.Where(it => it[i] == co2Target).ToArray();
            }

            return Convert.ToInt32(remainingOxy[0], 2) * Convert.ToInt32(remainingCo2[0], 2);
        }

        [Solution(3, 2, "linq")]
        public int Solution2Linq(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return Convert.ToInt32(
                Enumerable
                    .Range(0, lines[0].Length)
                    .Aggregate(lines, (acc, i) => acc.GroupBy(it => it[i]).OrderByDescending(it => it.Count()).ThenByDescending(it => it.Key).First().ToArray())
                    .First(), 2)
                *
                Convert.ToInt32(
                    Enumerable
                    .Range(0, lines[0].Length)
                    .Aggregate(lines, (acc, i) => acc.GroupBy(it => it[i]).OrderBy(it => it.Count()).ThenBy(it => it.Key).First().ToArray())
                    .First(), 2);
        }
    }
}
