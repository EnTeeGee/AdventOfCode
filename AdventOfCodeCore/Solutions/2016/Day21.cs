using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day21
    {
        [Solution(21, 1)]
        public string Solution1(string input)
        {
            var commands = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
            var values = "abcdefgh";

            foreach (var item in commands)
                values = Translate(values, item);

            return values;
        }

        [Solution(21, 2)]
        public string Solution2(string input)
        {
            var commands = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it)).Reverse().ToArray();
            var values = "fbgdceah";

            foreach (var item in commands)
                values = Untranslate(values, item);

            return values;
        }

        private string Translate(string input, string[] command)
        {
            var items = input.ToArray();
            if (command[0] == "swap" && command[1] == "position")
            {
                var indexes = new[] { command[2], command[5] }.Select(it => int.Parse(it)).ToArray();
                items[indexes[0]] = input[indexes[1]];
                items[indexes[1]] = input[indexes[0]];
            }
            else if (command[0] == "swap" && command[1] == "letter")
            {
                var indexes = new[] { command[2][0], command[5][0] }.Select(it => Array.IndexOf(items, it)).ToArray();
                items[indexes[0]] = input[indexes[1]];
                items[indexes[1]] = input[indexes[0]];
            }
            else if (command[0] == "rotate" && command[1] == "left")
            {
                var value = int.Parse(command[2]) % items.Length;
                items = items.Skip(value).Concat(items.Take(value)).ToArray();
            }
            else if (command[0] == "rotate" && command[1] == "right")
            {
                var value = int.Parse(command[2]) % items.Length;
                items = items.Skip(items.Length - value).Concat(items.Take(items.Length - value)).ToArray();
            }
            else if (command[0] == "rotate" && command[1] == "based")
            {
                var value = Array.IndexOf(items, command[6][0]) + 1;
                if (value >= 5)
                    value++;
                var list = items.ToList();
                for(var i = 0; i < value; i++)
                {
                    var letter = list.Last();
                    list.Remove(letter);
                    list.Insert(0, letter);
                }
                items = list.ToArray();
            }
            else if (command[0] == "reverse")
            {
                var indexes = new[] { command[2], command[4] }.Select(it => int.Parse(it)).ToArray();
                items = items.Take(indexes[0])
                    .Concat(items.Skip(indexes[0]).Take(indexes[1] - indexes[0] + 1).Reverse())
                    .Concat(items.Skip(indexes[1] + 1))
                    .ToArray();
            }
            else if (command[0] == "move")
            {
                var indexes = new[] { command[2], command[5] }.Select(it => int.Parse(it)).ToArray();
                var list = items.ToList();
                list.RemoveAt(indexes[0]);
                list.Insert(indexes[1], items[indexes[0]]);
                items = list.ToArray();
            }
            else
                throw new Exception("Unexpected command");

            return new string(items);
        }

        private string Untranslate(string input, string[] command)
        {
            var items = input.ToArray();
            if (command[0] == "swap" && command[1] == "position")
            {
                var indexes = new[] { command[2], command[5] }.Select(it => int.Parse(it)).ToArray();
                items[indexes[0]] = input[indexes[1]];
                items[indexes[1]] = input[indexes[0]];
            }
            else if (command[0] == "swap" && command[1] == "letter")
            {
                var indexes = new[] { command[2][0], command[5][0] }.Select(it => Array.IndexOf(items, it)).ToArray();
                items[indexes[0]] = input[indexes[1]];
                items[indexes[1]] = input[indexes[0]];
            }
            else if (command[0] == "rotate" && command[1] == "left")
            {
                var value = int.Parse(command[2]) % items.Length;
                items = items.Skip(items.Length - value).Concat(items.Take(items.Length - value)).ToArray();
            }
            else if (command[0] == "rotate" && command[1] == "right")
            {
                var value = int.Parse(command[2]) % items.Length;
                items = items.Skip(value).Concat(items.Take(value)).ToArray();
            }
            else if (command[0] == "rotate" && command[1] == "based")
            {
                var targetChar = command[6][0];
                for(var i = 1; i < items.Length + 2; i++)
                {
                    if (i == 5)
                        continue;
                    var testCase = items.Skip(i % items.Length).Concat(items.Take(i % items.Length)).ToArray();
                    if (Array.IndexOf(testCase, targetChar) == (i > 5 ? i - 2 : i - 1))
                    {
                        items = testCase;
                        break;
                    }
                }
            }
            else if (command[0] == "reverse")
            {
                var indexes = new[] { command[2], command[4] }.Select(it => int.Parse(it)).ToArray();
                items = items.Take(indexes[0])
                    .Concat(items.Skip(indexes[0]).Take(indexes[1] - indexes[0] + 1).Reverse())
                    .Concat(items.Skip(indexes[1] + 1))
                    .ToArray();
            }
            else if (command[0] == "move")
            {
                var indexes = new[] { command[2], command[5] }.Select(it => int.Parse(it)).ToArray();
                var list = items.ToList();
                list.RemoveAt(indexes[1]);
                list.Insert(indexes[0], items[indexes[1]]);
                items = list.ToArray();
            }
            else
                throw new Exception("Unexpected command");

            return new string(items);
        }
    }
}
