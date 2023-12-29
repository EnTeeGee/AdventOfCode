using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day18
    {
        [Solution(18, 1)]
        public int Solution1(string input)
        {
            return RunFor(input, 40);
        }

        [Solution(18, 2)]
        public int Solution2(string input)
        {
            return RunFor(input, 400.Thousand());
        }

        private int RunFor(string input, int steps)
        {
            var line = input.Select(it => it == '^').ToArray();
            var safeCount = line.Count(it => !it);
            for (var i = 1; i < steps; i++)
            {
                line = line
                    .Zip(new[] { false }.Concat(line), (a, b) => new { l = b, c = a })
                    .Zip(line.Skip(1).Concat(new[] { false }), (a, b) => new { a.l, a.c, r = b })
                    .Select(it => IsTrap(it.l, it.c, it.r))
                    .ToArray();
                safeCount += line.Count(it => !it);
            }

            return safeCount;
        }

        private bool IsTrap(bool left, bool centre, bool right)
        {
            return (left && centre && !right)
                || (!left && centre && right)
                || (left && !centre && !right)
                || (!left && !centre && right);
        }
    }
}
