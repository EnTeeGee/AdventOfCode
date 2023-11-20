using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day11
    {
        private static char[] invalidChars = { 'i', 'l', 'o' };

        [Solution(11, 1)]
        public string Solution1(string input)
        {
            var current = StepPassword(input);

            while (true)
            {
                if (IsPasswordValid(current))
                    return current;

                current = StepPassword(current);
            }
        }

        [Solution(11, 2)]
        public string Solution2(string input)
        {
            var first = Solution1(input);

            return Solution1(first);
        }

        private string StepPassword(string input)
        {
            for (var i = input.Length - 1; i >= 0; i--)
            {
                if (input[i] == 'z')
                    continue;

                return new string(input
                    .Take(i)
                    .Append((char)(input[i] + (invalidChars.Contains((char)(input[i] + 1)) ? 2 : 1)))
                    .Concat(Enumerable.Repeat('a', input.Length - i - 1)).ToArray());
            }

            throw new Exception("Reached the end of passwords");
        }

        private bool IsPasswordValid(string input)
        {
            var hasSequential = input
                .Zip(input.Skip(1), (a, b) => a + 1 == b ? b : (char?)null)
                .Zip(input.Skip(2), (a, b) => a + 1 == b ? b : (char?)null)
                .Any(it => it != null);

            if (!hasSequential)
                return false;

            var hasInvalid = input.Any(it => it == 'i' || it == 'l' || it == 'o');

            if (hasInvalid)
                return false;

            var hasTwoPairs = input
                .Zip(input.Skip(1), (a, b) => a == b ? a : (char?)null)
                .Zip(input.Skip(2).Concat(new[] { '-', '-' }), (a, b) => a != b ? a : (char?)null)
                .Where(it => it != null)
                .Distinct()
                .Count() >= 2;

            return hasTwoPairs;
        }
    }
}
