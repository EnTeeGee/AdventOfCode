using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            var rolls = Parse(input);

            return rolls.Count(it => it.GetSurrounding8().Count(it2 => rolls.Contains(it2)) < 4);
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var rolls = Parse(input);
            var output = 0;

            while (true)
            {
                var removed = rolls.RemoveWhere(it => it.GetSurrounding8().Count(it2 => rolls.Contains(it2)) < 4);
                if (removed == 0)
                    return output;

                output += removed;
            }
        }

        private HashSet<Point> Parse(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var rolls = new HashSet<Point>();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '@')
                        rolls.Add((x, y));
                }
            }

            return rolls;
        }
    }
}
