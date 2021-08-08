using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day05
    {
        [Solution(5, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var invalidStings = new[] { "ab", "cd", "pq", "xy" };
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u' };

            var valid = 0;
            foreach(var item in lines)
            {
                if (invalidStings.Any(it => item.Contains(it)))
                    continue;

                if (item.Zip(item.Skip(1), (a, b) => a == b).All(it => !it))
                    continue;

                if (item.Where(it => vowels.Contains(it)).Count() < 3)
                    continue;

                valid++;
            }

            return valid;
        }

        [Solution(5, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var valid = 0;
            foreach(var item in lines)
            {
                var test = item.Zip(item.Skip(1), (a, b) => $"{a}{b}")
                    .Zip(item.Skip(2).Concat(new[] { ' ', ' ' }), (a, b) => (a[0] == b && a[1] == b) ? null : a).ToList();

                var result = item.Zip(item.Skip(1), (a, b) => $"{a}{b}")
                    .Zip(item.Skip(2).Concat(new[] { ' ', ' ' }), (a, b) => (a[0] == b && a[1] == b) ? null : a)
                    .Where(it => it != null)
                    .GroupBy(it => it)
                    .Where(it => it.Count() >= 2)
                    .Any();

                if (!result)
                    continue;

                var result2 = item.Zip(item.Skip(2), (a, b) => a == b ? a : (char?)null).Zip(item.Skip(1), (a, b) => a == b ? (char?)null : a).Any(it => it != null);

                if (!result2)
                    continue;

                valid++;
            }

            return valid;
        }
    }
}
