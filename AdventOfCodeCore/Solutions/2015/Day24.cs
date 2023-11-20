using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day24
    {
        [Solution(24, 1)]
        public long Solution1(string input)
        {
            var packages = Parser.ToArrayOfInt(input).OrderByDescending(it => it).ToArray();
            var targetWeight = packages.Sum() / 3;

            var combinations = GetVariationsOfWeight(targetWeight, packages);
            var minLength = combinations.Select(it => it.Length).Min();
            var minEntanglement = combinations.Where(it => it.Length == minLength).Select(it => it.Aggregate(1L, (a, b) => a * b)).Min();

            return minEntanglement;
        }

        [Solution(24, 2)]
        public long Solution2(string input)
        {
            var packages = Parser.ToArrayOfInt(input).OrderByDescending(it => it).ToArray();
            var targetWeight = packages.Sum() / 4;

            var combinations = GetVariationsOfWeight(targetWeight, packages);
            var minLength = combinations.Select(it => it.Length).Min();
            var minEntanglement = combinations.Where(it => it.Length == minLength).Select(it => it.Aggregate(1L, (a, b) => a * b)).Min();

            return minEntanglement;
        }

        private List<int[]> GetVariationsOfWeight(int target, int[] options)
        {
            var output = new List<int[]>();

            for (var i = 0; i < options.Length; i++)
            {
                if (options[i] > target)
                    continue;

                if (options[i] == target)
                {
                    output.Add(new[] { options[i] });
                    continue;
                }

                var subOptions = GetVariationsOfWeight(target - options[i], options.Skip(i + 1).ToArray());
                output.AddRange(subOptions.Select(it => new[] { options[i] }.Concat(it).ToArray()));
            }

            return output;
        }
    }
}
