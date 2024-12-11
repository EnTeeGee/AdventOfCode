using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day11
    {
        [Solution(11, 1)]
        public long Solution1(string input)
        {
            var history = new Dictionary<(long, int), long>();

            return Parser.SplitOnSpace(input).Sum(it => TurnsInto(long.Parse(it), 25, history));
        }

        [Solution(11, 2)]
        public long Solution2(string input)
        {
            var history = new Dictionary<(long, int), long>();

            return Parser.SplitOnSpace(input).Sum(it => TurnsInto(long.Parse(it), 75, history));
        }

        private long TurnsInto(long value, int stepsLeft, Dictionary<(long, int), long> history)
        {
            if (stepsLeft == 0)
                return 1;

            if (history.ContainsKey((value, stepsLeft)))
                return history[(value, stepsLeft)];

            long result;
            if (value == 0)
                result = TurnsInto(1, stepsLeft - 1, history);
            else if (value.ToString().Length % 2 == 0)
            {
                var str = value.ToString();
                result = TurnsInto(long.Parse(str[..(str.Length / 2)]), stepsLeft - 1, history);
                result += TurnsInto(long.Parse(str[(str.Length / 2)..]), stepsLeft - 1, history);
            }
            else
                result = TurnsInto(value * 2024, stepsLeft - 1, history);

            history.Add((value, stepsLeft), result);

            return result;
        }
    }
}
