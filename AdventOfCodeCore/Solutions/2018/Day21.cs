using AdventOfCode2018.Common;
using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var pointer = int.Parse(lines[0].Split(' ')[1]);
            var registers = new long[6];
            var instructions = lines.Skip(1).Select(it => new Instruction(it)).ToArray();

            while (registers[pointer] >= 0 && registers[pointer] < instructions.Length)
            {
                var nextInstruction = instructions[registers[pointer]];
                if (nextInstruction.Code is EqRR && nextInstruction.B == 0)
                    return (int)registers[nextInstruction.A];

                registers = instructions[registers[pointer]].Run(registers);
                registers[pointer]++;
            }

            return 0;
        }

        [Solution(21, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var pointer = int.Parse(lines[0].Split(' ')[1]);
            var registers = new long[6];
            var instructions = lines.Skip(1).Select(it => new Instruction(it)).ToArray();
            var lastValue = 0;
            var seenValues = new HashSet<int>();

            while (registers[pointer] >= 0 && registers[pointer] < instructions.Length)
            {
                var nextInstruction = instructions[registers[pointer]];
                if (nextInstruction.Code is EqRR && nextInstruction.B == 0)
                {
                    var value = (int)registers[nextInstruction.A];
                    if (seenValues.Contains(value))
                        return lastValue;

                    seenValues.Add(value);
                    lastValue = value;
                }
                    
                    

                registers = instructions[registers[pointer]].Run(registers);
                registers[pointer]++;
            }

            return 0;
        }

        private class Instruction
        {
            public IOpCode Code { get; }
            public int A { get; }
            public int B { get; }
            public int C { get; }

            public Instruction(string input)
            {
                var items = input.Split(' ');
                Code = ElfLang.Mapping[items[0]]();
                A = int.Parse(items[1]);
                B = int.Parse(items[2]);
                C = int.Parse(items[3]);
            }

            public long[] Run(long[] input)
            {
                return Code.Perform(input, A, B, C);
            }
        }
    }
}
