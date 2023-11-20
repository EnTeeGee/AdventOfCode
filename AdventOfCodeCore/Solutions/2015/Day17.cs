using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day17
    {
        [Solution(17, 1)]
        public int Solution1(string input)
        {
            var containers = Parser.ToArrayOfInt(input).OrderByDescending(it => it).ToArray();

            return GetValidCombinations(150, containers);
        }

        [Solution(17, 2)]
        public int Solution2(string input)
        {
            var containers = Parser.ToArrayOfInt(input).OrderByDescending(it => it).ToArray();

            return GetCombinationCounts(150, containers, 0).GroupBy(it => it).OrderBy(it => it.Key).First().Count();
        }

        private int GetValidCombinations(int nog, int[] containers)
        {
            var total = 0;
            for (var i = 0; i < containers.Length; i++)
            {
                var container = containers[i];
                if (container > nog)
                    continue;

                if (container == nog)
                    ++total;
                else
                    total += GetValidCombinations(nog - container, containers.Skip(i + 1).ToArray());
            }

            return total;
        }

        private int[] GetCombinationCounts(int nog, int[] containers, int depth)
        {
            var output = new List<int>();
            for (var i = 0; i < containers.Length; i++)
            {
                var container = containers[i];
                if (container > nog)
                    continue;

                if (container == nog)
                    output.Add(depth + 1);
                else
                    output.AddRange(GetCombinationCounts(nog - container, containers.Skip(i + 1).ToArray(), depth + 1));
            }

            return output.ToArray();
        }
    }
}
