using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day21
    {
        [Solution(21, 1)]
        public long Solution1(string input)
        {
            var items = Parser.ToArrayOf(input, it => new Monkey(it));
            var dict = items.ToDictionary(it => it.Name);

            return dict["root"].GetValue(dict);
        }

        [Solution(21, 2)]
        public long Solution2(string input)
        {
            var items = Parser.ToArrayOf(input, it => new Monkey(it));
            var dict = items.ToDictionary(it => it.Name);

            return dict["root"].SolveFor(0, dict);
        }

        private class Monkey
        {
            public string Name { get; }
            public long? OveridenValue { get; set; }

            private readonly string data;

            public Monkey(string input)
            {
                var sections = Parser.SplitOn(input, ':');
                Name = sections[0];
                data = sections[1].Trim();
            }

            public long GetValue(Dictionary<string, Monkey> all)
            {
                if (OveridenValue != null)
                    return OveridenValue.Value;

                if (!data.Contains(' '))
                    return long.Parse(data);

                var sections = Parser.SplitOnSpace(data);
                var first = all[sections[0]].GetValue(all);
                var second = all[sections[2]].GetValue(all);

                if (sections[1] == "+")
                    return first + second;
                else if (sections[1] == "-")
                    return first - second;
                else if (sections[1] == "*")
                    return first * second;
                else
                    return first / second;
            }

            public long? TryGetValue(Dictionary<string, Monkey> all)
            {
                if (Name == "humn")
                    return null;

                if (!data.Contains(' '))
                    return long.Parse(data);

                var sections = Parser.SplitOnSpace(data);
                var first = all[sections[0]].TryGetValue(all);
                var second = all[sections[2]].TryGetValue(all);

                if (first == null || second == null)
                    return null;

                if (sections[1] == "+")
                    return first + second;
                else if (sections[1] == "-")
                    return first - second;
                else if (sections[1] == "*")
                    return first * second;
                else
                    return first / second;
            }

            public long SolveFor(long targetResult, Dictionary<string, Monkey> all)
            {
                if (Name == "humn")
                    return targetResult;

                var sections = Parser.SplitOnSpace(data);
                var first = all[sections[0]].TryGetValue(all);
                var second = all[sections[2]].TryGetValue(all);

                if(Name == "root")
                {
                    if (first == null)
                        return all[sections[0]].SolveFor(second.Value, all);
                    else
                        return all[sections[2]].SolveFor(first.Value, all);
                }

                if(first == null)
                {
                    var target = all[sections[0]];

                    if (sections[1] == "+") // ? + val
                        return target.SolveFor(targetResult - second.Value, all);
                    else if (sections[1] == "-") // ? - val
                        return target.SolveFor(targetResult + second.Value, all);
                    else if (sections[1] == "*") // ? * val
                        return target.SolveFor(targetResult / second.Value, all);
                    else // ? / val
                        return target.SolveFor(targetResult * second.Value, all);
                }
                else
                {
                    var target = all[sections[2]];
                    if (sections[1] == "+") // val + ?
                        return target.SolveFor(targetResult - first.Value, all);
                    else if (sections[1] == "-") // val - ?
                        return target.SolveFor(first.Value - targetResult, all);
                    else if (sections[1] == "*") // val * ?
                        return target.SolveFor(targetResult / first.Value, all);
                    else // val / ?
                        return target.SolveFor(first.Value / targetResult, all);
                }
            }
        }
    }
}
