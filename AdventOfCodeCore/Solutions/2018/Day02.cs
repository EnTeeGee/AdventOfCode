using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day02
    {
        [Solution(2, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Select(it => it.GroupBy(l => l))
                .Select(it => new
                {
                    Has2Group = it.Any(g => g.Count() == 2),
                    Has3Group = it.Any(g => g.Count() == 3)
                });

            return (lines.Where(it => it.Has2Group).Count() * lines.Where(it => it.Has3Group).Count());
        }

        [Solution(2, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .ToArray();
            var targetLength = lines[0].Length - 1;

            for (var i = 0; i < lines.Length; i++)
            {
                var match = lines.Skip(i + 1)
                    .Select(it => it.Zip(lines[i], (a, b) => new { A = a, B = b }).Where(l => l.A == l.B))
                    .Where(it => it.Count() == targetLength)
                    .FirstOrDefault();

                if (match != null)
                    return string.Join(string.Empty, match.Select(it => it.A));
            }

            throw new Exception("Found no match");
        }
    }
}
