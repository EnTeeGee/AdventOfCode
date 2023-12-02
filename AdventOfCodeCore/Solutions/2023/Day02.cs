using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day02
    {
        [Solution(2, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOf(input, it => new Game(it))
                .Where(it => it.Draws.All(d => d.WithinLimit()))
                .Sum(it => it.Id);
        }

        [Solution(2, 2)]
        public int Solution2(string input)
        {
            return Parser.ToArrayOf(input, it => new Game(it))
                .Sum(it => it.GetPower());
        }

        private class Game
        {
            public int Id { get; }
            public Draw[] Draws { get; }

            public Game(string input)
            {
                var chunks = Parser.SplitOn(input, ':', ';');
                Id = int.Parse(chunks[0].Split(' ')[1]);
                Draws = chunks.Skip(1).Select(it => new Draw(it)).ToArray();
            }

            public int GetPower()
            {
                return Draws.Max(it => it.Red) * Draws.Max(it => it.Blue) * Draws.Max(it => it.Green);
            }
        }

        private class Draw
        {
            public int Red { get; }
            public int Blue { get; }
            public int Green { get; }

            public Draw(string input)
            {
                var items = Parser.SplitOn(input, ", ").Select(it => it.Trim()).ToArray();
                Red = int.Parse(items.FirstOrDefault(it => it.EndsWith("red"))?.Split(' ')[0] ?? "0");
                Blue = int.Parse(items.FirstOrDefault(it => it.EndsWith("blue"))?.Split(' ')[0] ?? "0");
                Green = int.Parse(items.FirstOrDefault(it => it.EndsWith("green"))?.Split(' ')[0] ?? "0");
            }

            public bool WithinLimit()
            {
                return Red <= 12 && Blue <= 14 && Green <= 13;
            }
        }
    }
}
