using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day25
    {
        [Solution(25, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var current = 0;
            var limit = 100;
            while (true)
            {
                var registers = new Dictionary<string, int>
                {
                    { "a", current },
                    { "b", 0 },
                    { "c", 0 },
                    { "d", 0 }
                };
                var result = RunForInput(registers, lines, limit);

                if(result.Any() && result.Zip(TargetSeq(), (a, b) => a == b).All(a => a))
                    return current;

                current++;
            }
        }

        private IEnumerable<int> TargetSeq()
        {
            while (true)
            {
                yield return 0;
                yield return 1;
            }
        }

        private List<int> RunForInput(Dictionary<string, int> registers, string[] lines, int limit)
        {
            var index = 0;
            var instructions = lines.Select(it => Parser.SplitOnSpace(it)).ToArray();
            var output = new List<int>();
            while (index < lines.Length)
            {
                var line = instructions[index];
                if (line[0] == "cpy")
                {
                    int source;
                    if (registers.ContainsKey(line[1]))
                        source = registers[line[1]];
                    else
                        source = int.Parse(line[1]);
                    if (registers.ContainsKey(line[2]))
                        registers[line[2]] = source;
                }
                else if (line[0] == "inc")
                    registers[line[1]] += 1;
                else if (line[0] == "dec")
                    registers[line[1]] -= 1;
                else if (line[0] == "jnz")
                {
                    var val1 = registers.ContainsKey(line[1]) ? registers[line[1]] : int.Parse(line[1]);
                    var val2 = registers.ContainsKey(line[2]) ? registers[line[2]] : int.Parse(line[2]);
                    if (val1 != 0)
                    {
                        index += val2;
                        continue;
                    }
                }
                else if (line[0] == "out")
                {
                    output.Add(registers.ContainsKey(line[1]) ? registers[line[1]] : int.Parse(line[1]));
                    if (output.Count >= limit)
                        return output;
                }

                else
                    throw new Exception("Unexpected command");

                index += 1;
            }

            return output;
        }
    }
}
