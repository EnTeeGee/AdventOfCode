using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions._2016
{
    class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var registers = new Dictionary<string, int>
            {
                { "a", 0 },
                { "b", 0 },
                { "c", 0 },
                { "d", 0 }
            };

            return RunForInput(registers, lines);
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var registers = new Dictionary<string, int>
            {
                { "a", 0 },
                { "b", 0 },
                { "c", 1 },
                { "d", 0 }
            };

            return RunForInput(registers, lines);
        }

        private int RunForInput(Dictionary<string, int> registers, string[] lines)
        {
            var index = 0;
            while (index < lines.Length)
            {
                var instructions = Parser.SplitOnSpace(lines[index]);
                if (instructions[0] == "cpy")
                {
                    var source = 0;
                    if (registers.ContainsKey(instructions[1]))
                        source = registers[instructions[1]];
                    else
                        source = int.Parse(instructions[1]);
                    registers[instructions[2]] = source;
                }
                else if (instructions[0] == "inc")
                    registers[instructions[1]] += 1;
                else if (instructions[0] == "dec")
                    registers[instructions[1]] -= 1;
                else if (instructions[0] == "jnz")
                {
                    var val1 = registers.ContainsKey(instructions[1]) ? registers[instructions[1]] : int.Parse(instructions[1]);
                    var val2 = registers.ContainsKey(instructions[2]) ? registers[instructions[2]] : int.Parse(instructions[2]);
                    if (val1 != 0)
                    {
                        index += val2;
                        continue;
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
