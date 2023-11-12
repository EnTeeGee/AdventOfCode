using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day16
    {
        [Solution(16, 1)]
        public string Solution1(string input)
        {
            var moves = Parser.SplitOn(input, ',').Select(it => new Move(it)).ToArray();
            var dancers = Enumerable.Range('a', 16).Select(it => (char)it).ToArray();

            dancers = Dance(dancers, moves);

            return string.Join(string.Empty, dancers);
        }

        [Solution(16, 2)]
        public string Solution2(string input)
        {
            var moves = Parser.SplitOn(input, ',').Select(it => new Move(it)).ToArray();
            var dancers = Enumerable.Range('a', 16).Select(it => (char)it).ToArray();
            var seen = new Dictionary<string, int>
            {
                { string.Join(string.Empty, dancers), 0 }
            };
            var step = 1;

            while (true)
            {
                dancers = Dance(dancers, moves);
                var result = string.Join(string.Empty, dancers);

                if (seen.ContainsKey(result))
                {
                    var stepSize = step - seen[result];
                    var remaining = 1.Billion() - step;
                    var last = remaining % stepSize;
                    var target = seen[result] + last;

                    return seen.First(it => it.Value == target).Key;
                }

                seen.Add(result, step);
                step++;
            }
        }

        private char[] Dance(char[] dancers, Move[] moves)
        {
            foreach (var item in moves)
            {
                if (item.Type == 's')
                {
                    var index = dancers.Length - int.Parse(item.Var1);
                    dancers = dancers[index..].Concat(dancers[..index]).ToArray();
                }
                else if (item.Type == 'x')
                {
                    var index1 = int.Parse(item.Var1);
                    var index2 = int.Parse(item.Var2);
                    var swap = dancers[index1];
                    dancers[index1] = dancers[index2];
                    dancers[index2] = swap;
                }
                else
                {
                    var swap = Array.IndexOf(dancers, item.Var2[0]);
                    dancers[Array.IndexOf(dancers, item.Var1[0])] = item.Var2[0];
                    dancers[swap] = item.Var1[0];
                }
            }

            return dancers;
        }

        private struct Move
        {
            public char Type { get; }
            public string Var1 { get; }
            public string Var2 { get; }

            public Move(string input)
            {
                Type = input[0];
                var items = Parser.SplitOn(input.Substring(1), '/');
                Var1 = items[0];
                Var2 = items.Length > 1 ? items[1] : string.Empty;
            }
        }

        
    }
}
