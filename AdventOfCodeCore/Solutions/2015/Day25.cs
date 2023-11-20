using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day25
    {
        private const int startingValue = 20151125;

        [Solution(25, 1)]
        public long Solution1(string input)
        {
            var items = Parser.SplitOn(input, ' ', '.', ',');
            var row = int.Parse(items[15]);
            var column = int.Parse(items[17]);

            var priorDiag = (row + column - 2);
            var diagStart = (priorDiag * (priorDiag + 1)) / 2;
            var index = diagStart + column;

            long value = startingValue;

            for (var i = 1; i < index; i++)
            {
                long interim = value * 252533L;
                value = interim % 33554393;
            }

            return value;
        }
    }
}
