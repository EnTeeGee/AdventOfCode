using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day25
    {
        [Solution(25, 1)]
        public int Solution(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);

            var locks = chunks
                .Where(it => it[0] == '#')
                .Select(it => Parser.ToArrayOfString(it)
                    .SelectMany(it2 => it2)
                    .Select((it2, index) => new { var = it2, col = index % 5 })
                    .GroupBy(it2 => it2.col)
                    .OrderBy(it2 => it2.Key)
                    .Select(it2 => it2.Count(it3 => it3.var == '#'))
                    .ToArray());

            var keys = chunks
                .Where(it => it[0] == '.')
                .Select(it => Parser.ToArrayOfString(it)
                    .SelectMany(it2 => it2)
                    .Select((it2, index) => new { var = it2, col = index % 5 })
                    .GroupBy(it2 => it2.col)
                    .OrderBy(it2 => it2.Key)
                    .Select(it2 => it2.Count(it3 => it3.var == '#'))
                    .ToArray());

            return keys
                .SelectMany(it => locks
                    .Select(it2 => it2.Zip(it, (a, b) => a + b)
                        .All(it3 => it3 <= 7)))
                .Count(it => it);
        }
    }
}
