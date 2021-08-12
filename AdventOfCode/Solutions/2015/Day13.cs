using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var mappings = new Dictionary<string, Dictionary<string, int>>();

            foreach(var item in lines)
            {
                var tokens = Parser.SplitOnSpace(item);

                if (!mappings.ContainsKey(tokens[0]))
                    mappings.Add(tokens[0], new Dictionary<string, int>());

                mappings[tokens[0]].Add(tokens.Last().TrimEnd('.'), int.Parse(tokens[3]) * (tokens[2] == "gain" ? 1 : -1));
            }

            var remaining = mappings.Keys.ToArray();
            var start = remaining[0];
            remaining = remaining.Skip(1).ToArray();

            var perms = GetPermutations(remaining);

            perms = perms.Select(it => new List<string> { start }.Concat(it).ToArray()).ToList();

            var sums = perms.Select(it => GetTotalHappiness(it, mappings)).ToList();

            return perms.Max(it => GetTotalHappiness(it, mappings));
        }

        [Solution(13, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var mappings = new Dictionary<string, Dictionary<string, int>>();

            foreach (var item in lines)
            {
                var tokens = Parser.SplitOnSpace(item);

                if (!mappings.ContainsKey(tokens[0]))
                    mappings.Add(tokens[0], new Dictionary<string, int>());

                mappings[tokens[0]].Add(tokens.Last().TrimEnd('.'), int.Parse(tokens[3]) * (tokens[2] == "gain" ? 1 : -1));
            }

            foreach (var item in mappings)
                item.Value.Add("you", 0);

            mappings.Add("you", new Dictionary<string, int>());

            foreach (var item in mappings)
                mappings["you"].Add(item.Key, 0);

            var remaining = mappings.Keys.ToArray();
            var start = remaining[0];
            remaining = remaining.Skip(1).ToArray();

            var perms = GetPermutations(remaining);

            perms = perms.Select(it => new List<string> { start }.Concat(it).ToArray()).ToList();

            var sums = perms.Select(it => GetTotalHappiness(it, mappings)).ToList();

            return perms.Max(it => GetTotalHappiness(it, mappings));
        }

        private List<string[]> GetPermutations(string[] remaining)
        {
            if (remaining.Length <= 1)
                return new List<string[]> { remaining };

            var output = new List<string[]>();

            foreach(var item in remaining)
            {
                var newRemaining = remaining.Where(it => it != item).ToArray();
                var perms = GetPermutations(newRemaining);
                output.AddRange(perms.Select(it => new List<string> { item }.Concat(it).ToArray()));
            }

            return output;
        }

        private int GetTotalHappiness(string[] loop, Dictionary<string, Dictionary<string, int>> mappings)
        {
            return loop.Zip(loop.Skip(1).Append(loop[0]), (a, b) => mappings[a][b] + mappings[b][a]).Sum();
        }
    }
}
