using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day07
    {
        [Solution(7, 1)]
        public int Solution1(string input)
        {
            var positions = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();

            return Enumerable
                .Range(positions.Min(), positions.Max() - positions.Min() + 1)
                .Select(it => positions.Select(p => Math.Abs(p - it)).Sum())
                .Min();
        }

        [Solution(7, 2)]
        public int Solution2(string input)
        {
            var fuelForDist = new Func<int, int>(it => (it * (it + 1)) / 2);
            var positions = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();

            return Enumerable
                .Range(positions.Min(), positions.Max() - positions.Min() + 1)
                .Select(it => positions.Select(p => fuelForDist(Math.Abs(p - it))).Sum())
                .Min();
        }
    }
}
