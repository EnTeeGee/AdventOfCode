using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day02
    {
        static readonly Dictionary<char, char> wins = new Dictionary<char, char> { { 'A', 'Y' }, { 'B', 'Z' }, { 'C', 'X' } };
        static readonly Dictionary<char, char> losses = new Dictionary<char, char> { { 'A', 'Z' }, { 'B', 'X' }, { 'C', 'Y' } };

        [Solution(2, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => new { elf = it[0], you = it[2] })
                .Sum(it => (wins[it.elf] == it.you ? 6 : losses[it.elf] == it.you ? 0 : 3) + it.you - 'W');
        }

        [Solution(2, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => new { elf = it[0], you = it[2] })
                .Sum(it => (it.you == 'Z' ? wins[it.elf] : it.you == 'X' ? losses[it.elf] : (it.elf + 23)) - 'W' + ((it.you - 'X') * 3));
        }
    }
}
