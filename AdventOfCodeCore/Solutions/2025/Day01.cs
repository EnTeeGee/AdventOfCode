using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var output = 0;
            var index = 50;
            var limit = 100;
            foreach(var line in lines)
            {
                var turn = int.Parse(line.Substring(1));
                index += line[0] == 'R' ? turn : -turn;
                while (index < 0)
                    index += limit;
                if ((index % limit) == 0)
                    output++;
            }

            return output;
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var output = 0;
            var index = 50;
            var limit = 100;
            foreach (var line in lines)
            {
                var turn = int.Parse(line.Substring(1));
                var newIndex = index + (line[0] == 'R' ? turn : -turn);
                if (index > 0)
                    output += (newIndex > 0 ? newIndex / limit : Math.Abs(newIndex / limit) + 1);
                else
                    output += (newIndex > 0 ? newIndex / limit : Math.Abs(newIndex / limit));
                index = newIndex;
                while (index < 0)
                    index += limit;
                index %= limit;
            }

            return output;
        }
    }
}
