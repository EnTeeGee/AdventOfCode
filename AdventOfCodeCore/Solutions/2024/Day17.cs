using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day17
    {
        [Solution(17, 1)]
        public string Solution1(string input)
        {
            var chunks = Parser.ToArrayOfString(input).SelectMany(it => Parser.SplitOn(it, ": ")).ToArray();
            var registers = new Dictionary<char, long>
            {
                { 'A', long.Parse(chunks[1]) },
                { 'B', long.Parse(chunks[3]) },
                { 'C', long.Parse(chunks[5]) }
            };
            var program = Parser.SplitOn(chunks[7], ',').Select(it => int.Parse(it)).ToArray();
            
            return string.Join(',', RunProgram(registers, program));
        }

        [Solution(17, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfString(input).SelectMany(it => Parser.SplitOn(it, ": ")).ToArray();
            var program = Parser.SplitOn(chunks[7], ',').Select(it => int.Parse(it)).ToArray();
            var reversedProgram = program.Reverse().ToArray();
            var output = 0L;

            for(var i = 0; i < reversedProgram.Length; i++)
            {
                output <<= 3;

                while (true)
                {
                    var registers = new Dictionary<char, long>
                        {
                            { 'A', output },
                            { 'B', 0 },
                            { 'C', 0 }
                        };

                    var result = RunProgram(registers, program);
                    if (result.Count == i + 1 && result.AsEnumerable().Reverse().Zip(reversedProgram, (a, b) => a == b).All(it => it))
                        break;

                    output++;
                }
            }

            return output;
        }

        private List<long> RunProgram(Dictionary<char, long> registers, int[] program)
        {
            var index = 0;
            var output = new List<long>();

            while (index >= 0 && index < program.Length - 1)
            {
                switch (program[index])
                {
                    case 0:
                        registers['A'] /= (long)Math.Pow(2, GetComboOperand(program[index + 1], registers));
                        break;
                    case 1:
                        registers['B'] ^= program[index + 1];
                        break;
                    case 2:
                        registers['B'] = GetComboOperand(program[index + 1], registers) % 8;
                        break;
                    case 3:
                        if (registers['A'] != 0)
                        {
                            index = program[index + 1];
                            continue;
                        }
                        break;
                    case 4:
                        registers['B'] ^= registers['C'];
                        break;
                    case 5:
                        output.Add(GetComboOperand(program[index + 1], registers) % 8);
                        break;
                    case 6:
                        registers['B'] = registers['A'] / (long)Math.Pow(2, GetComboOperand(program[index + 1], registers));
                        break;
                    case 7:
                        registers['C'] = registers['A'] / (long)Math.Pow(2, GetComboOperand(program[index + 1], registers));
                        break;
                }

                index += 2;
            }

            return output;
        }

        private long GetComboOperand(int value, Dictionary<char, long> registers)
        {
            if (value == 4)
                return registers['A'];
            if (value == 5)
                return registers['B'];
            if (value == 6)
                return registers['C'];

            return value;
        }
    }
}
