using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            var process = new Coprocessor(input);
            do { } while (process.TryStep());

            return process.MulInvokes;
        }

        [Solution(23, 2)]
        public int Solution2(string input)
        {
            // From working out using pen and paper:
            // The code takes all numbers from register B to C (inclusive) in steps of 17 and counts the number that are not prime
            // 17 is hardcoded based on the second to last line of my input file
            var process = new Coprocessor(input);
            process.Registers.Add("a", 1);
            for (var i = 0; i < 8; i++)
                process.TryStep();
            var nonPrimes = 0;
            for(var i = process.Registers["b"]; i <= process.Registers["c"]; i += 17)
            {
                if(!Primes.IsPrime(i))
                    nonPrimes++;
            }

            return nonPrimes;
        }

        private class Coprocessor
        {
            string[][] instructions;
            int pointer;
            public Dictionary<string, long> Registers { get; }
            public int MulInvokes { get; private set; }

            public Coprocessor(string input)
            {
                instructions = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
                pointer = 0;
                Registers = new Dictionary<string, long>();
                MulInvokes = 0;
            }

            public bool TryStep()
            {
                if (pointer < 0 || pointer >= instructions.Length)
                    return false;

                var instruction = instructions[pointer];
                switch (instruction[0])
                {
                    case "set":
                        SetValue(instruction[1], GetValue(instruction[2]));
                        break;
                    case "sub":
                        Registers[instruction[1]] = GetValue(instruction[1]) - GetValue(instruction[2]);
                        break;
                    case "mul":
                        Registers[instruction[1]] = GetValue(instruction[1]) * GetValue(instruction[2]);
                        MulInvokes++;
                        break;
                    case "jnz":
                        var jumpValue = GetValue(instruction[1]);
                        if (jumpValue != 0)
                        {
                            pointer += (int)GetValue(instruction[2]);

                            return true;
                        }
                        break;
                    default:
                        throw new Exception("Unexpected command " + instruction[0]);
                }

                pointer++;

                return true;
            }

            private long GetValue(string input)
            {
                if (input.Length == 1 && char.IsLetter(input[0]))
                {
                    if (!Registers.ContainsKey(input))
                        Registers.Add(input, 0);

                    return Registers[input];
                }

                return int.Parse(input);
            }

            private void SetValue(string input, long value)
            {
                if (!Registers.ContainsKey(input))
                    Registers.Add(input, value);
                else
                    Registers[input] = value;
            }
        }
    }
}
