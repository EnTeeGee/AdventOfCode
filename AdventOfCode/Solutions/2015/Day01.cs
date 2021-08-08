using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            return input.Count(it => it == '(') - input.Count(it => it == ')');
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            var current = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var item = input[i];
                current += (item == '(' ? 1 : -1);

                if (current < 0)
                    return i + 1;
            }

            throw new Exception("Never reached basement");
        }
    }
}
