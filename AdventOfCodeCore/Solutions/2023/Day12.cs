using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day12
    {
        [Solution(12, 1)]
        public long Solution1(string input)
        {
            var items = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it))
                .Select(it => new { spring = it[0], groups = Parser.SplitOn(it[1], ',').Select(g => int.Parse(g)).ToArray() })
                .ToArray();

            return items.Sum(it => GetValidArrangements(it.spring, it.groups, new Dictionary<State, long>()));
        }

        [Solution(12, 2)]
        public long Solution2(string input)
        {
            var items = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it))
                .Select(it => new {
                    spring = string.Join('?', Enumerable.Repeat(it[0], 5)),
                    groups = Parser.SplitOn(string.Join(',', Enumerable.Repeat(it[1], 5)), ',').Select(g => int.Parse(g)).ToArray() })
                .ToArray();

            return items.Sum(it => GetValidArrangements(it.spring, it.groups, new Dictionary<State, long>()));
        }

        private long GetValidArrangements(string spring, int[] groups, Dictionary<State, long> seen)
        {
            var state = new State(spring, groups);
            if (seen.ContainsKey(state))
                return seen[state];

            var output = 0L;
            if (spring.All(it => it == '.' || it == '?') && groups.Length == 0)
                output = 1;
            else if (spring.All(it => it == '#' || it == '?') && groups.Length == 1 && groups[0] == spring.Length)
                output = 1;
            else if ((spring.Length < groups.Sum() + groups.Length - 1) || groups.Length == 0)
                output = 0;
            else
            {
                if (spring.Take(groups[0]).All(it => it == '#' || it == '?') && (spring[groups[0]] == '.' || spring[groups[0]] == '?'))
                    output += GetValidArrangements(spring.Substring(groups[0] + 1), groups.Skip(1).ToArray(), seen);
                if (spring[0] == '.' || spring[0] == '?')
                    output += GetValidArrangements(spring.Substring(1), groups, seen);
            }

            seen.Add(state, output);

            return output;
        }

        private class State
        {
            public string Spring { get; }
            public int[] Groups { get; }
            private int hash;

            public State (string spring, int[] groups)
            {
                Spring = spring;
                Groups = groups;
                hash = (spring + string.Join(',', groups)).GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                if (obj is not State cast)
                    return false;

                return cast.Spring == Spring
                    && cast.Groups.Length == Groups.Length
                    && cast.Groups.Zip(Groups, (a, b) => a == b).All(it => it);
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }
    }
}
