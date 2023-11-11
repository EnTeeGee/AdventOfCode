using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            var start = Parser.ToArrayOf(input, it => long.Parse(it.Substring(24)));
            var generatorA = start[0];
            var generatorB = start[1];
            var factorA = 16807;
            var factorB = 48271;
            var mod = 2147483647;

            var target = 40.Million();
            var matches = 0;

            for(var i = 0; i < target; i++)
            {
                generatorA = (generatorA * factorA) % mod;
                generatorB = (generatorB * factorB) % mod;

                if ((generatorA & ushort.MaxValue) == (generatorB & ushort.MaxValue))
                    matches++;
            }

            return matches;
        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            var start = Parser.ToArrayOf(input, it => long.Parse(it.Substring(24)));
            var generatorA = start[0];
            var generatorB = start[1];
            var factorA = 16807;
            var factorB = 48271;
            var mod = 2147483647;

            var target = 5.Million();
            var matches = 0;

            for (var i = 0; i < target; i++)
            {
                do
                {
                    generatorA = (generatorA * factorA) % mod;
                } while (generatorA % 4 != 0);
                do
                {
                    generatorB = (generatorB * factorB) % mod;
                } while (generatorB % 8 != 0);

                if ((generatorA & ushort.MaxValue) == (generatorB & ushort.MaxValue))
                    matches++;
            }

            return matches;
        }
    }
}
