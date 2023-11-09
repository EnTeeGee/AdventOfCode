using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day05
    {
        [Solution(5, 1)]
        public int Solution1(string input)
        {
            var instructions = Parser.ToArrayOfInt(input);
            var index = 0;
            var step = 1;

            while (true)
            {
                var newIndex = index + instructions[index];
                if (newIndex < 0 || newIndex >= instructions.Length)
                    return step;

                step += 1;
                instructions[index] += 1;
                index = newIndex;
            }
        }

        [Solution(5, 2)]
        public int Solution2(string input)
        {
            var instructions = Parser.ToArrayOfInt(input);
            var index = 0;
            var step = 1;

            while (true)
            {
                var newIndex = index + instructions[index];
                if (newIndex < 0 || newIndex >= instructions.Length)
                    return step;

                step += 1;
                instructions[index] += (instructions[index] >= 3 ? -1 : 1);
                index = newIndex;
            }
        }
    }
}
