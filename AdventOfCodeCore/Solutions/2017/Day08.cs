using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var instructions = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
            var registers = new Dictionary<string, int>();
            foreach (var line in instructions)
            {
                if (!registers.ContainsKey(line[0]))
                    registers[line[0]] = 0;
                if (!registers.ContainsKey(line[4]))
                    registers[line[4]] = 0;
                var val = int.Parse(line[2]) * (line[1] == "inc" ? 1 : -1);
                var comp = int.Parse(line[6]);

                if (line[5] == ">" && registers[line[4]] > comp)
                    registers[line[0]] += val;
                else if (line[5] == "<" && registers[line[4]] < comp)
                    registers[line[0]] += val;
                else if (line[5] == ">=" && registers[line[4]] >= comp)
                    registers[line[0]] += val;
                else if (line[5] == "<=" && registers[line[4]] <= comp)
                    registers[line[0]] += val;
                else if (line[5] == "==" && registers[line[4]] == comp)
                    registers[line[0]] += val;
                else if (line[5] == "!=" && registers[line[4]] != comp)
                    registers[line[0]] += val;
            }

            return registers.Values.Max();
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            var instructions = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
            var registers = new Dictionary<string, int>();
            var max = 0;
            foreach (var line in instructions)
            {
                if (!registers.ContainsKey(line[0]))
                    registers[line[0]] = 0;
                if (!registers.ContainsKey(line[4]))
                    registers[line[4]] = 0;
                var val = int.Parse(line[2]) * (line[1] == "inc" ? 1 : -1);
                var comp = int.Parse(line[6]);

                if (line[5] == ">" && registers[line[4]] > comp)
                    registers[line[0]] += val;
                else if (line[5] == "<" && registers[line[4]] < comp)
                    registers[line[0]] += val;
                else if (line[5] == ">=" && registers[line[4]] >= comp)
                    registers[line[0]] += val;
                else if (line[5] == "<=" && registers[line[4]] <= comp)
                    registers[line[0]] += val;
                else if (line[5] == "==" && registers[line[4]] == comp)
                    registers[line[0]] += val;
                else if (line[5] == "!=" && registers[line[4]] != comp)
                    registers[line[0]] += val;

                if (registers[line[0]] > max)
                    max = registers[line[0]];
            }

            return max;
        }
    }
}
