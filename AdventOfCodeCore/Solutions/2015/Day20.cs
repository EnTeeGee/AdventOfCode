using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day20
    {
        [Solution(20, 1)]
        public int Solution1(string input)
        {
            var target = int.Parse(input) / 10;

            var house = 1;
            while (true)
            {
                var factorSum = Factor.GetFactors(house).Sum();

                if (factorSum >= target)
                    return house;

                ++house;
            }
        }

        [Solution(20, 2)]
        public int Solution2(string input)
        {
            var target = int.Parse(input);

            var house = 1;
            while (true)
            {
                var factors = Factor.GetFactors(house);
                var factorSum = factors.Where(it => house / it <= 50).Select(it => it * 11).Sum();

                if (factorSum >= target)
                    return house;

                ++house;
            }
        }
    }
}
