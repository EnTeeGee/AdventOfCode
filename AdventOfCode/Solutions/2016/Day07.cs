using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2016
{
    class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Count(it => CheckString(it));
        }

        [Solution(7, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input).Count(it => CheckString2(it));
        }

        private (string[] Outer, string[] Inner) GetSections(string input)
        {
            var outerSections = new List<string>();
            var innerSections = new List<string>();

            var startIndex = input.IndexOf('[');
            while (startIndex > 0)
            {
                var endIndex = input.IndexOf(']');
                outerSections.Add(input.Substring(0, startIndex));
                innerSections.Add(input.Substring(startIndex + 1, endIndex - startIndex - 1));
                input = input.Substring(endIndex + 1);
                startIndex = input.IndexOf('[');
            }

            outerSections.Add(input);

            return (outerSections.ToArray(), innerSections.ToArray());
        }

        private bool CheckString(string input)
        {
            var sections = GetSections(input);

            return sections.Outer.Any(it => ContainsSubPattern(it)) && sections.Inner.All(it => !ContainsSubPattern(it));
        }

        private bool CheckString2(string input)
        {
            var sections = GetSections(input);
            var abas = sections.Outer.SelectMany(it => Get3CharSubPatterns(it)).ToArray();
            var babs = sections.Inner.SelectMany(it => Get3CharSubPatterns(it)).Select(it => $"{it[1]}{it[0]}{it[1]}").ToArray();

            return abas.Any(it => babs.Contains(it));
        }

        private bool ContainsSubPattern(string input)
        {
            var matchOuter = input.Zip(input.Skip(3), (a, b) => a == b).ToArray();
            var matchInner = input.Skip(1).Zip(input.Skip(2), (a, b) => a == b).ToArray();
            var notMatch = input.Zip(input.Skip(1), (a, b) => a != b).ToArray();

            var result = matchOuter.Zip(matchInner, (a, b) => a && b).Zip(notMatch, (a, b) => a && b).Any(it => it);

            return result;
        }

        private string[] Get3CharSubPatterns(string input)
        {
            var match = input.Zip(input.Skip(2), (a, b) => a == b);
            var notMatch = input.Zip(input.Skip(1), (a, b) => a != b);
            var indexes = match.Zip(notMatch, (a, b) => a && b).ToArray();

            return Enumerable
                .Range(0, indexes.Length)
                .Select(it => indexes[it] ? $"{input[it]}{input[it + 1]}{input[it]}" : null)
                .Where(it => it != null)
                .ToArray();
        }
    }
}
