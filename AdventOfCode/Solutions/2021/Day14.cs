using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day14
    {
        [Solution(14, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOf(chunks[1], it => Parser.SplitOnSpace(it)).ToDictionary(it => it[0], it => it[2]);

            var groups = Enumerable.Range(0, 10).Aggregate(chunks[0], (acc, _) => Expand(acc, rules)).GroupBy(it => it).OrderBy(it => it.Count()).ToArray();

            return groups.Last().Count() - groups.First().Count();
        }

        [Solution(14, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOf(chunks[1], it => Parser.SplitOnSpace(it)).ToDictionary(it => it[0], it => new[] { $"{it[0][0]}{it[2]}", $"{it[2]}{it[0][1]}" });

            var currentPairs = rules.Keys.ToDictionary(it => it, it => 0L);
            foreach (var item in chunks[0].Zip(chunks[0].Skip(1), (a, b) => $"{a}{b}"))
                currentPairs[item]++;

            var currentCounts = chunks[0].GroupBy(it => it).ToDictionary(it => it.Key, it => (long)it.Count());

            for(var i = 0; i < 40; i++)
            {
                var newPairs = currentPairs.ToDictionary(it => it.Key, it => 0L);
                foreach(var item in currentPairs)
                {
                    var match = rules[item.Key];
                    newPairs[match[0]] += item.Value;
                    newPairs[match[1]] += item.Value;
                    if (!currentCounts.ContainsKey(match[0][1]))
                        currentCounts.Add(match[0][1], 0);
                    currentCounts[match[0][1]] += item.Value;
                }
                currentPairs = newPairs;
            }

            var orderedCounts = currentCounts.Values.OrderBy(it => it).ToArray();

            return orderedCounts.Last() - orderedCounts.First();
        }

        private string Expand(string polymer, Dictionary<string, string> mappings)
        {
            return string.Join(
                string.Empty,
                polymer.Zip(polymer.Skip(1), (a, b) => a + mappings[$"{a}{b}"]).Concat(new[] { polymer.Last().ToString() }).ToArray());
        }
    }
}
