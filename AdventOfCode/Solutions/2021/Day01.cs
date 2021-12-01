﻿using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            var depths = Parser.ToArrayOfInt(input);

            return depths.Zip(depths.Skip(1), (a, b) => b - a > 0).Count(it => it);
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            var depths = Parser.ToArrayOfInt(input);
            var windows = depths.Zip(depths.Skip(1), (a, b) => a + b).Zip(depths.Skip(2), (a, b) => a + b).ToArray();

            return windows.Zip(windows.Skip(1), (a, b) => b - a > 0).Count(it => it);
        }
    }
}
