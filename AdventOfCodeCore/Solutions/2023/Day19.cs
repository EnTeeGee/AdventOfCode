using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day19
    {
        private static Dictionary<string, int> PropMap = new Dictionary<string, int> { { "x", 0 }, { "m", 1 }, { "a", 2 }, { "s", 3 } };

        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var workflows = Parser.ToArrayOf(chunks[0], it => new Workflow(it)).ToDictionary(it => it.Name);
            var parts = Parser.ToArrayOf(chunks[1], it => new PartRange(it));

            return parts.Where(it => workflows["in"].GetCovered(it, workflows) > 0).Sum(it => it.Start.Sum());
        }

        [Solution(19, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var workflows = Parser.ToArrayOf(chunks[0], it => new Workflow(it)).ToDictionary(it => it.Name);

            return workflows["in"].GetCovered(new PartRange(), workflows);
        }

        private class PartRange
        {
            public int[] Start { get; }
            public int[] End { get; }

            public PartRange()
            {
                Start = Enumerable.Repeat(1, 4).ToArray();
                End = Enumerable.Repeat(4.Thousand(), 4).ToArray();
            }

            public PartRange(string input)
            {
                var chunks = Parser.SplitOn(input, '=', ',', '}');
                Start = new[] { chunks[1], chunks[3], chunks[5], chunks[7] }
                    .Select(it => int.Parse(it))
                    .ToArray();
                End = Start.ToArray();
            }

            public PartRange(PartRange toCopy)
            {
                Start = toCopy.Start.ToArray();
                End = toCopy.End.ToArray();
            }

            public long GetArea()
            {
                return Start.Zip(End, (a, b) => b - a + 1).Aggregate(1L, (acc, it) => acc * it);
            }
        }

        private class Workflow
        {
            public string Name { get; }
            public (string prop, bool greaterThan, int value, string target)[] Checks { get; }

            public Workflow(string input)
            {
                var chunks = Parser.SplitOn(input, '{', ',', '}');
                Name = chunks[0];
                var checkList = new List<(string, bool, int, string)>();
                foreach(var item in chunks.Skip(1))
                {
                    var items = item.Split(':');
                    if(items.Length == 2)
                    {
                        var subItems = Parser.SplitOn(items[0], '<', '>');
                        checkList.Add((subItems[0], items[0].Contains('>'), int.Parse(subItems[1]), items[1]));
                    }
                    else
                        checkList.Add(("x", true, 0, items[0]));
                }
                Checks = checkList.ToArray();
            }

            public long GetCovered(PartRange range, Dictionary<string, Workflow> workflows)
            {
                var output = 0L;
                foreach(var item in Checks)
                {
                    var results = Compute(range, item.prop, item.greaterThan, item.value);
                    if (item.target == "A" && results.valid != null)
                        output += results.valid.GetArea();

                    else if (item.target != "R" && results.valid != null)
                        output += workflows[item.target].GetCovered(results.valid, workflows);

                    if (results.invalid != null)
                        range = results.invalid;
                    else
                        break;
                }

                return output;
            }

            private (PartRange? valid, PartRange? invalid) Compute(PartRange range, string prop, bool greaterThan, int value)
            {
                if (greaterThan)
                {
                    if (range.Start[PropMap[prop]] > value)
                        return (range, null);
                    if (range.End[PropMap[prop]] <= value)
                        return (null, range);

                    var newValid = new PartRange(range);
                    var newInvalid = new PartRange(range);
                    newValid.Start[PropMap[prop]] = value + 1;
                    newInvalid.End[PropMap[prop]] = value;

                    return (newValid, newInvalid);
                }
                else
                {
                    if (range.Start[PropMap[prop]] >= value)
                        return (null, range);
                    if (range.End[PropMap[prop]] < value)
                        return (range, null);

                    var newValid = new PartRange(range);
                    var newInvalid = new PartRange(range);
                    newValid.End[PropMap[prop]] = value - 1;
                    newInvalid.Start[PropMap[prop]] = value;

                    return (newValid, newInvalid);
                }
            }
        }
    }
}
