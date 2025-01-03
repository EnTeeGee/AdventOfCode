﻿using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    class Day01
    {
        [Solution(1, 1)]
        public string Solution1(string input)
        {
            return Parser.ToArrayOfInt(input).Select(it => ConvertValue(it)).Sum().ToString();
        }

        private int ConvertValue(int input)
        {
            return (input / 3) - 2;
        }


        [Solution(1, 2)]
        public string Solution2(string input)
        {
            return Parser.ToArrayOfInt(input).Select(it => CalcForModule(it)).Sum().ToString();
        }

        private int CalcForModule(int input)
        {
            var result = Math.Max(0, (input / 3) - 2);

            return result == 0 ? result : result + CalcForModule(result);
        }
    }
}
