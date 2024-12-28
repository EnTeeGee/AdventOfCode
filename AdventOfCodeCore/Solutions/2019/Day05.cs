using AdventOfCode2019.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    class Day05
    {
        [Solution(5, 1)]
        public int Problem1(string input)
        {
            var program = new Intcode(input);
            program.AddInput(1);

            var output = program.RunToEnd();

            return (int)output.Last();
        }

        [Solution(5, 2)]
        public int Problem2(string input)
        {
            var program = new Intcode(input);
            program.AddInput(5);

            var output = program.RunToEnd();

            return (int)output.Last();
        }
    }
}
