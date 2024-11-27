using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var registers = new Dictionary<string, int>
            {
                { "a", 7 },
                { "b", 0 },
                { "c", 0 },
                { "d", 0 }
            };

            return RunForInput(registers, lines);
        }

        [Solution(23, 2)]
        public int Solution2(string input)
        {
            // Process worked out using pen and paper
            var instructions = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
            var output = 132 * 10 * 9 * 8 * 7 * 6 * 5 * 4 * 3 * 2;
            output += (int.Parse(instructions[19][1]) * int.Parse(instructions[20][1]));

            return output;
        }

        private int RunForInput(Dictionary<string, int> registers, string[] lines)
        {
            var index = 0;
            var instructions = lines.Select(it => Parser.SplitOnSpace(it)).ToArray();
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
                else if (line[0] == "tgl")
                {
                    var val = registers.ContainsKey(line[1]) ? registers[line[1]] : int.Parse(line[1]);
                    val = index + val;
                    if (val >= 0 && val < instructions.Length)
                    {
                        var toggleLine = instructions[val];
                        if(toggleLine.Length == 2)
                        {
                            if (toggleLine[0] == "inc")
                                toggleLine[0] = "dec";
                            else
                                toggleLine[0] = "inc";
                        }
                        else
                        {
                            if (toggleLine[0] == "jnz")
                                toggleLine[0] = "cpy";
                            else
                                toggleLine[0] = "jnz";
                        }
                    }
                }
                else
                    throw new Exception("Unexpected command");

                index += 1;
            }

            return registers["a"];
        }
    }
}
