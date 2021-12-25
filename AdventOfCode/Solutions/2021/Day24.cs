using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day24
    {
        [Solution(24, 1)]
        public string Solution1(string input)
        {
            var startingState = new Dictionary<char, int> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
            var currentComp = new Computer(Parser.ToArrayOfString(input), startingState);

            var match = GetValidNumbers(currentComp);

            return string.Join(string.Empty, match);
        }

        [Solution(24, 2)]
        public string Solution2(string input)
        {
            var startingState = new Dictionary<char, int> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
            var currentComp = new Computer(Parser.ToArrayOfString(input), startingState);

            var match = GetValidNumbers2(currentComp);

            return string.Join(string.Empty, match);
        }

        private List<int> GetValidNumbers(Computer state)
        {
            for(var i = 9; i > 0; i--)
            {
                var tempComp = state.Clone();
                tempComp.RunUntilNextInput(i);
                if (!tempComp.IsWithinLimit())
                    continue;

                if (tempComp.AtEnd())
                    return new List<int> { i };

                var tail = GetValidNumbers(tempComp);
                if (tail != null)
                    return new List<int> { i }.Concat(tail).ToList();
            }

            return null;
        }

        private List<int> GetValidNumbers2(Computer state)
        {
            for (var i = 1; i < 10; i++)
            {
                var tempComp = state.Clone();
                tempComp.RunUntilNextInput(i);
                if (!tempComp.IsWithinLimit())
                    continue;

                if (tempComp.AtEnd())
                    return new List<int> { i };

                var tail = GetValidNumbers2(tempComp);
                if (tail != null)
                    return new List<int> { i }.Concat(tail).ToList();
            }

            return null;
        }

        private class Computer
        {
            public Dictionary<char, int> State { get; }
            public string[] RemainingLines { get; private set; }

            public Computer(string[] lines, Dictionary<char, int> state)
            {
                State = state;
                RemainingLines = lines;
            }

            public void RunUntilNextInput(int input)
            {
                if (!RemainingLines[0].StartsWith("inp"))
                    throw new Exception("Odd state");

                var target = Parser.SplitOnSpace(RemainingLines[0])[1][0];
                State[target] = input;
                RemainingLines = RemainingLines.Skip(1).ToArray();

                while (RemainingLines.Any() && !RemainingLines[0].StartsWith("inp"))
                {
                    var items = Parser.SplitOnSpace(RemainingLines[0]);
                    var opTarget = items[1][0];
                    var opSecond = items[2].Length == 1 && char.IsLetter(items[2][0]) ? State[items[2][0]] : int.Parse(items[2]);

                    switch (items[0])
                    {
                        case "add":
                            State[opTarget] += opSecond;
                            break;
                        case "mul":
                            State[opTarget] *= opSecond;
                            break;
                        case "div":
                            State[opTarget] /= opSecond;
                            break;
                        case "mod":
                            State[opTarget] %= opSecond;
                            break;
                        case "eql":
                            State[opTarget] = (State[opTarget] == opSecond ? 1 : 0);
                            break;
                    }

                    RemainingLines = RemainingLines.Skip(1).ToArray();
                }
            }

            public Computer Clone()
            {
                return new Computer(RemainingLines, State.ToDictionary(it => it.Key, it => it.Value));
            }

            public bool IsWithinLimit()
            {
                var remaining = RemainingLines.Where(it => it == "div z 26").Count();
                var maxVal = Math.Pow(26, remaining);

                return State['z'] < maxVal;
            }

            public bool AtEnd()
            {
                return !RemainingLines.Any();
            }
        }
    }
}
