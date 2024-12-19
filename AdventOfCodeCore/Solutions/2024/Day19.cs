using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day19
    {
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var towels = Parser.SplitOn(chunks[0], ", ");
            var patterns = Parser.ToArrayOfString(chunks[1]);

            return patterns.Count(it => MakesPatterns(it, towels, new Dictionary<string, long>()) > 0);
        }

        [Solution(19, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var towels = Parser.SplitOn(chunks[0], ", ");
            var patterns = Parser.ToArrayOfString(chunks[1]);

            return patterns.Sum(it => MakesPatterns(it, towels, new Dictionary<string, long>()));
        }

        private long MakesPatterns(string pattern, string[] towels, Dictionary<string, long> seen)
        {
            if (pattern.Length == 0) return 1;

            if (seen.ContainsKey(pattern))
                return seen[pattern];

            var matches = towels.Where(it => it.Length <= pattern.Length && it == pattern[..it.Length]);
            if (!matches.Any())
                return 0;

            var output = matches.Sum(it => MakesPatterns(pattern[it.Length..], towels, seen));
            seen.Add(pattern, output);

            return output;
        }
    }
}
