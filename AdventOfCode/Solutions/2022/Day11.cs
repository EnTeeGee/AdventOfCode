using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day11
    {
        [Solution(11, 1)]
        public long Solution1(string input)
        {
            return RunFor(input, 20, true);
        }

        [Solution(11, 2)]
        public long Solution2(string input)
        {
            return RunFor(input, 10000, false);
        }

        private long RunFor(string input, int steps, bool lowerWorry)
        {
            var monkeys = Parser.ToArrayOfGroups(input).Select(it => new Monkey(it)).ToArray();

            for (var i = 0; i < steps; i++)
            {
                Console.WriteLine("running line " + i);

                foreach (var item in monkeys)
                {
                    var result = item.ThrowAll(lowerWorry);
                    foreach (var val in result)
                        monkeys[val.target].Items.Add(val.value);
                }
            }

            var mostActive = monkeys.OrderByDescending(it => it.Inspections).Select(it => it.Inspections).Take(2).ToArray();

            return (long)mostActive[0] * mostActive[1];
        }

        private class Monkey
        {
            public List<BigInteger> Items { get; }
            public int Inspections { get; private set; }

            private Func<BigInteger, BigInteger> op;
            private int test;
            private int trueTarget;
            private int falseTarget;

            public Monkey(string input)
            {
                var lines = Parser.ToArrayOfString(input);
                Items = Parser.SplitOn(lines[1], ' ', ',').Skip(2).Select(it => BigInteger.Parse(it)).ToList();
                var opSections = Parser.SplitOnSpace(lines[2]);
                if (opSections[3] == "old" && opSections[5] == "old")
                    op = ((it) => it * it);
                else if (opSections[4] == "+")
                    op = (it => it + int.Parse(opSections[5]));
                else
                    op = (it => it * int.Parse(opSections[5]));
                test = int.Parse(Parser.SplitOnSpace(lines[3])[3]);
                trueTarget = int.Parse(Parser.SplitOnSpace(lines[4])[5]);
                falseTarget = int.Parse(Parser.SplitOnSpace(lines[5])[5]);
            }

            public (int target, BigInteger value)[] ThrowAll(bool lowerWorry)
            {
                var output = new List<(int, BigInteger)>();

                foreach (var item in Items)
                {
                    var newItem = op.Invoke(item) / (lowerWorry ? 3 : 1);
                    if (newItem % test == 0)
                        output.Add((trueTarget, newItem));
                    else
                        output.Add((falseTarget, newItem));
                }

                Inspections += Items.Count;
                Items.Clear();

                return output.ToArray();
            }
        }
    }
}
