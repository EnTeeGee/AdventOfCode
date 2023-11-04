using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day02
    {
        [Solution(2, 1)]
        public static int Solution1(string input)
        {
            return Parser
                .ToArrayOfString(input)
                .Select(it => Parser
                    .SplitOn(it, ' ', '\t')
                    .Select(x => int.Parse(x))
                    .OrderBy(x => x)
                    .ToArray())
                .Select(it => it.Last() - it.First())
                .Sum();
        }

        [Solution(2, 2)]
        public static int Solution2(string input)
        {
            return Parser
                .ToArrayOfString(input)
                .Select(it => Parser
                    .SplitOn(it, ' ', '\t')
                    .Select(x => int.Parse(x))
                    .OrderByDescending(x => x)
                    .ToArray())
                .Select(it => FindDivision(it))
                .Sum();
        }


        private static int FindDivision(int[] input)
        {
            for(var i = 0; i < input.Length; i++)
            {
                for(var j = i + 1; j < input.Length; j++)
                {
                    if (input[i] % input[j] == 0)
                        return input[i] / input[j];
                }
            }

            throw new Exception("No valid divisable pair found");
        }
    }
}
