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
            var parts = Parser.ToArrayOf(chunks[1], it => new Part(it));

            var total = 0;
            foreach(var item in parts)
            {
                var currentWorkflow = "in";
                while(currentWorkflow != "A" && currentWorkflow != "R")
                { 
                    var next = workflows[currentWorkflow];
                    currentWorkflow = next.Checks.First(it => it.checkFunc(item)).nextWorkflow;
                }

                if (currentWorkflow == "A")
                    total += item.Sum();
            }

            return total;
        }

        [Solution(19, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var workflows = Parser.ToArrayOf(chunks[0], it => new Workflow2(it)).ToDictionary(it => it.Name);

            return workflows["in"].GetCovered(new PartRange(), workflows);
        }

        private class Workflow
        {
            public string Name { get; }
            public (Func<Part, bool> checkFunc, string nextWorkflow)[] Checks { get; }

            public Workflow(string input)
            {
                var chunks = Parser.SplitOn(input, '{', ',', '}');
                Name = chunks[0];
                var checksList = new List<(Func<Part, bool>, string)>();
                foreach(var item in chunks.Skip(1))
                {
                    var items = item.Split(':');
                    if (items.Length == 2)
                    {
                        var funcItems = Parser.SplitOn(items[0], '>', '<');
                        var val = int.Parse(funcItems[1]);
                        var isGreater = items[0].Contains('>');
                        var func = (Part part) =>
                        {
                            var compValue = funcItems[0] == "x" ? part.X
                                : funcItems[0] == "m" ? part.M
                                : funcItems[0] == "a" ? part.A
                                : part.S;

                            return isGreater ? compValue > val : compValue < val;
                        };
                        checksList.Add((func, items[1]));
                    }
                    else
                    {
                        var func = (Part part) => { return true; };
                        checksList.Add((func, items[0]));
                    }
                }

                Checks = checksList.ToArray();
            }
        }

        private class Part
        {
            public int X { get; }
            public int M { get; }
            public int A { get; }
            public int S { get; }

            public Part(string input)
            {
                var chunks = Parser.SplitOn(input, '=', ',', '}');
                X = int.Parse(chunks[1]);
                M = int.Parse(chunks[3]);
                A = int.Parse(chunks[5]);
                S = int.Parse(chunks[7]);
            }

            public int Sum()
            {
                return X + M + A + S;
            }
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

        private class Workflow2
        {
            public string Name { get; }
            public (string prop, bool greaterThan, int value, string target)[] Checks { get; }

            public Workflow2(string input)
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

            public long GetCovered(PartRange range, Dictionary<string, Workflow2> workflows)
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
