using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => int.Parse(it));
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            var items = Parser.ToArrayOf(input, it => int.Parse(it));
            var seen = new HashSet<int>();

            var index = 0;
            var total = 0;
            seen.Add(0);

            while (true)
            {
                total += items[index % items.Length];
                if (seen.Contains(total))
                    return total;

                seen.Add(total);
                index++;
            }
        }
    }
}
