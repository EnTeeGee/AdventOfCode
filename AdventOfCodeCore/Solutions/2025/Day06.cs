using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day06
    {
        [Solution(6, 1)]
        public long Solution1(string input)
        {
            var items = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));

            return Enumerable.Range(0, items[0].Length)
                .Select(it => items.Take(items.Length - 1).Select(it2 => long.Parse(it2[it])).ToArray())
                .Select((it, i) => items[items.Length - 1][i] == "+" ? it.Sum() : it.Aggregate(1L, (a, b) => a * b))
                .Sum();
        }

        [Solution(6, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOfStringUntrimmed(input);

            var output = 0L;
            foreach(var item in lines.Last().Select((it, i) => (symbol: it, index: i)).Where(it => it.symbol != ' '))
            {
                output += Enumerable.Range(item.index, lines.Last().Skip(item.index + 1).TakeWhile(it => it == ' ').Count() + 1)
                    .Select(it => lines.Take(lines.Length - 1).Select(it2 => it2[it]).ToArray())
                    .Where(it => it.Any(it2 => it2 != ' '))
                    .Select(it => long.Parse(new string(it)))
                    .Aggregate(item.symbol == '+' ? 0L : 1L, (a, b) => item.symbol == '+' ? a + b : a * b);
            }

            return output;
        }
    }
}
