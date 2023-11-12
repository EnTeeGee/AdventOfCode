using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day18
    {
        [Solution(18, 1)]
        public long Solution1(string input)
        {
            var sound = 0L;
            var result = false;
            var runner = new Duet(input, false, (it) => sound = it, () => { result = sound != 0; return sound; });

            while (!result)
                runner.TryStep();

            return sound;
        }

        [Solution(18, 2)]
        public long Solution2(string input)
        {
            var queue1 = new Queue<long>();
            var queue2 = new Queue<long>();
            var count = 0;

            var runner1 = new Duet(input, false, (it) => queue2.Enqueue(it), () => queue1.Any() ? queue1.Dequeue() : null);
            var runner2 = new Duet(input, true, (it) => { queue1.Enqueue(it); count++; }, () => queue2.Any() ? queue2.Dequeue() : null);

            while (runner1.TryStep() || runner2.TryStep()) { }

            return count;
        }

        private class Duet
        {
            string[][] instructions;
            int pointer;
            Dictionary<string, long> registers;
            Action<long> writeFunc;
            Func<long?> readFunc;

            public Duet(string input, bool overrideP, Action<long> writeFunc, Func<long?> readFunc)
            {
                instructions = Parser.ToArrayOf(input, it => Parser.SplitOnSpace(it));
                pointer = 0;
                this.writeFunc = writeFunc;
                this.readFunc = readFunc;
                registers = new Dictionary<string, long>();
                if (overrideP)
                    registers.Add("p", 1);
            }

            public bool TryStep()
            {
                if (pointer < 0 || pointer >= instructions.Length)
                    return false;

                var instruction = instructions[pointer];
                switch(instruction[0])
                {
                    case "snd":
                        writeFunc(GetValue(instruction[1]));
                        break;
                    case "set":
                        SetValue(instruction[1], GetValue(instruction[2]));
                        break;
                    case "add":
                        registers[instruction[1]] = GetValue(instruction[1]) + GetValue(instruction[2]);
                        break;
                    case "mul":
                        registers[instruction[1]] = GetValue(instruction[1]) * GetValue(instruction[2]);
                        break;
                    case "mod":
                        registers[instruction[1]] = GetValue(instruction[1]) % GetValue(instruction[2]);
                        break;
                    case "rcv":
                        var value = readFunc();
                        if (value == null)
                            return false;
                        SetValue(instruction[1], value.Value);
                        break;
                    case "jgz":
                        var jumpValue = GetValue(instruction[1]);
                        if(jumpValue > 0)
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
                    if (!registers.ContainsKey(input))
                        registers.Add(input, 0);

                    return registers[input];
                }

                return int.Parse(input);
            }

            private void SetValue(string input, long value)
            {
                if(!registers.ContainsKey(input))
                    registers.Add(input, value);
                else
                    registers[input] = value;
            }
        }

        
    }
}
