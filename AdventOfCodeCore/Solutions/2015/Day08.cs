using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text.RegularExpressions;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return lines.Select(it => it.Length - GetCountForString(it)).Sum();
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            return lines.Select(it => CountSpecial(it)).Sum();
        }

        private int GetCountForString(string input)
        {
            var reduced = input.Replace("\\\\", "-");
            reduced = reduced.Replace("\\\"", "-");
            var hex = new Regex("[^\\\\]\\\\x");
            var hexCount = hex.Matches(reduced).Count;
            var extraCharacters = (input.Length - reduced.Length) + (hexCount * 3) + 2;

            return input.Length - extraCharacters;
        }

        private int CountSpecial(string input)
        {
            return input.Where(it => it == '\"' || it == '\\').Count() + 2;
        }
    }
}
