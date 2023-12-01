using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day01
    {
        [Solution(1, 1)]
        public int Solution1(string input)
        {
            var letters = Enumerable.Range('a', 26).Select(it => (char)it).ToArray();

            return Parser.ToArrayOfString(input)
                .Select(it => it.Trim(letters))
                .Sum(it => int.Parse($"{it.First()}{it.Last()}"));
        }

        [Solution(1, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOfString(input)
                .Select(it => GetIndexes(it))
                .Sum(it => int.Parse($"{it.First().value}{it.Last().value}"));
        }

        private (int value, int index)[] GetIndexes(string line)
        {
            var numbers = new Dictionary<string, int>
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
            };
            var output = new List<(int value, int index)>();
            foreach(var item in numbers)
            {
                if (line.IndexOf(item.Key) >= 0)
                    output.Add((item.Value, line.IndexOf(item.Key)));
                if (line.LastIndexOf(item.Key) >= 0)
                    output.Add((item.Value, line.LastIndexOf(item.Key)));

                if (line.IndexOf(item.Value.ToString()) >= 0)
                    output.Add((item.Value, line.IndexOf(item.Value.ToString())));
                if (line.LastIndexOf(item.Value.ToString()) >= 0)
                    output.Add((item.Value, line.LastIndexOf(item.Value.ToString())));
            }

            return output.OrderBy(it => it.index).ToArray();
        }
    }
}
