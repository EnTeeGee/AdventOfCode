using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day16
    {
        [Solution(16, 1)]
        public string Solution1(string input)
        {
            return RunFor(input, 272);
        }

        [Solution(16, 2)]
        public string Solution2(string input)
        {
            return RunFor(input, 35651584);
        }

        private string RunFor(string input, int limit)
        {
            var values = input.Select(it => it == '1').ToArray();
            while (values.Length < limit)
            {
                values = values
                .Concat(new[] { false })
                    .Concat(values.Reverse().Select(it => !it))
                    .Take(limit)
                    .ToArray();
            }

            while (values.Length % 2 == 0)
            {
                values = Enumerable.Range(0, values.Length / 2)
                    .Select(it => values[it * 2] == values[(it * 2) + 1])
                    .ToArray();
            }

            return new string(values.Select(it => it ? '1' : '0').ToArray());
        }
    }
}
