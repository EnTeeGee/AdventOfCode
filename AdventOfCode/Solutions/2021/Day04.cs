using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var calls = Parser.SplitOn(chunks[0], ',').Select(it => int.Parse(it)).ToArray();
            var boards = chunks.Skip(1).Select(it => new Board(it)).ToArray();

            foreach(var item in calls)
            {
                foreach(var board in boards)
                {
                    var result = board.Mark(item);
                    if (result)
                        return board.GetScore() * item;
                }
            }

            throw new Exception("No winning board found");

        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var calls = Parser.SplitOn(chunks[0], ',').Select(it => int.Parse(it)).ToArray();
            var boards = chunks.Skip(1).Select(it => new Board(it)).ToArray();

            foreach (var item in calls)
            {
                var winningBoards = boards.Where(it => it.Mark(item)).ToArray();
                var remainingBoards = boards.Where(it => !winningBoards.Contains(it)).ToArray();

                if (!remainingBoards.Any())
                    return winningBoards.Last().GetScore() * item;

                boards = remainingBoards;
            }

            throw new Exception("No last winning board found");
        }

        private class Board
        {
            private const int BoardSize = 5;
            private readonly int?[] remainingNumbers;

            public Board(string input)
            {
                remainingNumbers = Parser
                    .ToArrayOfString(input)
                    .SelectMany(it => Parser.SplitOnSpace(it))
                    .Select(it => (int?)int.Parse(it))
                    .ToArray();
            }

            public bool Mark(int value)
            {
                var index = Array.IndexOf(remainingNumbers, value);
                if (index == -1)
                    return false;

                remainingNumbers[index] = null;

                for(var i = 0; i < BoardSize; i++)
                {
                    if (Enumerable.Range(0, BoardSize).Select(it => it + (i * BoardSize)).All(it => remainingNumbers[it] == null)
                        ||
                        Enumerable.Range(0, BoardSize).Select(it => i + (it * BoardSize)).All(it => remainingNumbers[it] == null))
                        return true;
                }

                return false;
            }

            public int GetScore()
            {
                return remainingNumbers.Select(it => it ?? 0).Sum();
            }
        }
    }
}
