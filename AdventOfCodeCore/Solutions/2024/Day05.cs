using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day05
    {
        [Solution(5, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOfString(chunks[0])
                .Select(it => Parser.SplitOn(it, '|').Select(it2 => int.Parse(it2)).ToArray())
                .GroupBy(it => it[0])
                .ToDictionary(it => it.Key, it => it.Select(it2 => it2[1]).ToArray());

            var lists = Parser.ToArrayOf(chunks[1], it => Parser.SplitOn(it, ',').Select(it2 => int.Parse(it2)).ToArray());

            return lists.Where(it => IsListValid(it, rules))
                .Sum(it => it[it.Length / 2]);
        }

        [Solution(5, 2)]
        public int Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOfString(chunks[0])
                .Select(it => Parser.SplitOn(it, '|').Select(it2 => int.Parse(it2)).ToArray())
                .GroupBy(it => it[0])
                .ToDictionary(it => it.Key, it => it.Select(it2 => it2[1]).ToArray());

            var lists = Parser.ToArrayOf(chunks[1], it => Parser.SplitOn(it, ',').Select(it2 => int.Parse(it2)).ToArray());

            var comparer = new PageComparer(rules);
            return lists.Where(it => !IsListValid(it, rules))
                .Select(it => it.OrderBy(it => it, comparer).ToArray())
                .Sum(it => it[it.Length / 2]);
        }

        private bool IsListValid(int[] list, Dictionary<int, int[]> rules)
        {
            return list.Select((it, index) => new { val = it, index })
                .Where(it => rules.ContainsKey(it.val))
                .All(it => !list.Take(it.index).Any(it2 => rules[it.val].Contains(it2)));
        }

        private class PageComparer : IComparer<int>
        {
            private readonly Dictionary<int, int[]> rules;

            public PageComparer(Dictionary<int, int[]> rules)
            {
                this.rules = rules;
            }

            public int Compare(int x, int y)
            {
                if(rules.ContainsKey(x) && rules[x].Contains(y))
                    return 1;
                else if (rules.ContainsKey(y) && rules[y].Contains(x))
                    return -1;

                return 0;
            }
        }
    }
}
