using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    class Day04
    {
        [Solution(4, 1)]
        public int Problem1(string input)
        {
            var items = Parser.SplitOn(input, '-').Select(it => int.Parse(it)).ToArray();

            var totalMatches = 0;

            for(var i = items[0]; i <= items[1]; i++)
            {
                var str = i.ToString();

                var matches = str.Zip(str.Skip(1), (a, b) => b - a).ToArray();

                if (matches.All(it => it >= 0) && matches.Any(it => it == 0))
                    totalMatches++;
            }

            return totalMatches;
        }

        [Solution(4, 2)]
        public int Problem2(string input)
        {
            var items = Parser.SplitOn(input, '-').Select(it => int.Parse(it)).ToArray();

            var totalMatches = 0;

            for (var i = items[0]; i <= items[1]; i++)
            {
                var str = i.ToString();

                var matches = str.Zip(str.Skip(1), (a, b) => b - a).ToArray();

                if (matches.All(it => it >= 0) && str.GroupBy(it => it).Any(it => it.Count() == 2))
                    totalMatches++;
            }

            return totalMatches;
        }

    }
}
