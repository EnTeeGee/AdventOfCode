using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            var computer = new Computer(input);

            var canRun = true;
            while(canRun)
            {
                canRun = computer.RunStep();
            }

            return computer.Registers["b"];
        }

        [Solution(23, 2)]
        public int Solution2(string input)
        {
            var computer = new Computer(input);
            computer.Registers["a"] = 1;

            var canRun = true;
            while (canRun)
            {
                canRun = computer.RunStep();
            }

            return computer.Registers["b"];
        }

        private class Computer
        {
            public Dictionary<string, int> Registers { get; }
            public int Index { get; private set; }

            private Instruction[] instructions;

            public Computer(string input)
            {
                Registers = new Dictionary<string, int>();
                Registers["a"] = 0;
                Registers["b"] = 0;
                instructions = Parser.ToArrayOfString(input).Select(it => new Instruction(it)).ToArray();
            }

            public bool RunStep()
            {
                if (Index >= instructions.Length || Index < 0)
                    return false;

                var current = instructions[Index];

                switch (current.Flag)
                {
                    case "hlf":
                        Registers[current.Register] = Registers[current.Register] / 2;
                        break;
                    case "tpl":
                        Registers[current.Register] = Registers[current.Register] * 3;
                        break;
                    case "inc":
                        Registers[current.Register]++;
                        break;
                    case "jmp":
                        Index += (current.Val - 1);
                        break;
                    case "jie":
                        if (Registers[current.Register] % 2 == 0)
                            Index += (current.Val - 1);
                        break;
                    case "jio":
                        if (Registers[current.Register] == 1)
                            Index += (current.Val - 1);
                        break;
                }

                ++Index;
                
                return Index < instructions.Length && Index > 0;
            }
        }

        private class Instruction
        {
            public string Flag { get; }
            public string Register { get; }
            public int Val { get; }

            public Instruction(string input)
            {
                var items = Parser.SplitOn(input, ' ', ',');
                Flag = items[0];
                if(Flag == "jmp")
                {
                    Val = int.Parse(items[1]);
                }
                else
                {
                    Register = items[1];
                    if (items.Length > 2)
                        Val = int.Parse(items[2]);
                }
                
            }
        }
    }
}
