using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it).Select(n => int.Parse(n)).OrderByDescending(n => n).ToArray()).Where(it => it[0] < it[1] + it[2]).Count();
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it).Select(n => int.Parse(n)).ToArray()).ToArray();
            var output = 0;

            for(var i = 0; i < lines.Length; i+= 3)
            {
                var section = lines.Skip(i).Take(3).ToArray();
                var arranged = Enumerable.Range(0, 3).Select(it => section.Select(s => s[it]).OrderByDescending(n => n).ToArray()).ToList();
                output += arranged.Where(it => it[0] < it[1] + it[2]).Count();
            }

            return output;
        }
    }
}
