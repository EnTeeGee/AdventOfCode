using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day10
    {
        private static readonly string openers = "([{<";
        private static readonly Dictionary<char, int> syntaxScores = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
        private static readonly Dictionary<char, int> autoScores = new Dictionary<char, int> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };

        [Solution(10, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Select(it => CheckString(it)).Sum(it => it.Score);
        }

        [Solution(10, 2)]
        public long Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);

            var scores = lines
                .Select(it => CheckString(it))
                .Where(it => it.Score == 0)
                .Select(it => it.ToClose.Aggregate(0L, (acc, i) => (acc * 5) + autoScores[i]))
                .OrderBy(it => it)
                .ToArray();

            return scores[scores.Length / 2];
        }

        private (int Score, char[] ToClose) CheckString(string input)
        {
            var stack = new Stack<char>();

            foreach (var item in input)
            {
                if (openers.Contains(item))
                    stack.Push(item);
                else if (Math.Abs(item - stack.Peek()) > 2)
                    return (syntaxScores[item], new char[0]);
                else
                    stack.Pop();
            }

            return (0, stack.ToArray());
        }
    }
}
