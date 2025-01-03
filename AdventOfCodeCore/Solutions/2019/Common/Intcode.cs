﻿using AdventOfCodeCore.Common;

namespace AdventOfCode2019.Common
{
    public class Intcode
    {
        private enum Mode { Position, Absolute, Relative }

        private static readonly Dictionary<long, Mode> ModeMapping = new()
        {
            { 0, Mode.Position },
            { 1, Mode.Absolute },
            { 2, Mode.Relative }
        };

        private long[] program;
        private bool needsInput;
        private Queue<int> inputs;
        private int? constInput;
        private List<long> outputs;
        private long relativeBase;

        public long Index { get; private set; }
        public bool HasHalted { get; private set; }
        public IEnumerable<long> Outputs => outputs;

        public Intcode(string input)
        {
            program = Parser.SplitOn(input, ',').Select(it => long.Parse(it)).ToArray();
            inputs = new Queue<int>();
            outputs = new List<long>();
        }

        public Intcode(int[] input)
        {
            program = input.Select(it => (long)it).ToArray();
            inputs = new Queue<int>();
            outputs = new List<long>();
        }

        public void AddInput(int input)
        {
            inputs.Enqueue(input);
            needsInput = false;
        }

        public void SetConstInput(int? input)
        {
            constInput = input;
            needsInput = input == null && !inputs.Any();
        }

        public void ClearOutput()
        {
            outputs = new List<long>();
        }

        public void OverwriteAddress(int index, long value)
        {
            program[index] = value;
        }

        public long[] RunToEnd()
        {
            while (!HasHalted && !needsInput)
                RunStep();

            return outputs.ToArray();
        }

        public long? RunToOutput()
        {
            var currentLength = outputs.Count();

            while (outputs.Count() == currentLength)
            {
                RunStep();
                if (HasHalted || needsInput)
                    return null;
            }

            return outputs.Last();
        }

        private void RunStep()
        {
            if (HasHalted)
                return;

            var instruction = program[Index];
            var param1Mode = ModeMapping[(instruction / 100) % 10];
            var param2Mode = ModeMapping[(instruction / 1000) % 10];
            var param3Mode = ModeMapping[(instruction / 10000) % 10];
            

            switch (instruction % 100)
            {
                case 01:
                    RunAddition(param1Mode, param2Mode, param3Mode);
                    break;
                case 02:
                    RunMultiply(param1Mode, param2Mode, param3Mode);
                    break;
                case 03:
                    RunRead(param1Mode);
                    break;
                case 04:
                    RunWrite(param1Mode);
                    break;
                case 05:
                    RunJumpIfTrue(param1Mode, param2Mode);
                    break;
                case 06:
                    RunJumpIfFalse(param1Mode, param2Mode);
                    break;
                case 07:
                    RunLessThan(param1Mode, param2Mode, param3Mode);
                    break;
                case 08:
                    RunEquals(param1Mode, param2Mode, param3Mode);
                    break;
                case 09:
                    RunRelativeBaseOffset(param1Mode);
                    break;
                case 99:
                    HasHalted = true;
                    break;
                default:
                    throw new Exception("Unexpected opcode");
            }
        }

        private void RunAddition(Mode param1Mode, Mode param2Mode, Mode param3Mode)
        {
            var a = GetAtPos(1, param1Mode);
            var b = GetAtPos(2, param2Mode);
            WriteToPos(a + b, 3, param3Mode);
            Index += 4;
        }

        private void RunMultiply(Mode param1Mode, Mode param2Mode, Mode param3Mode)
        {
            var a = GetAtPos(1, param1Mode);
            var b = GetAtPos(2, param2Mode);
            WriteToPos(a * b, 3, param3Mode);
            Index += 4;
        }

        private void RunRead(Mode param1Mode)
        {
            if (constInput == null && !inputs.Any())
            {
                needsInput = true;
                return;
            }

            var input = constInput == null ? inputs.Dequeue() : constInput.Value;
            needsInput = false;
            WriteToPos(input, 1, param1Mode);
            Index += 2;
        }

        private void RunWrite(Mode param1Mode)
        {
            outputs.Add(GetAtPos(1, param1Mode));
            Index += 2;
        }

        private void RunJumpIfTrue(Mode param1Mode, Mode param2Mode)
        {
            var val = GetAtPos(1, param1Mode);
            if (val != 0)
                Index = GetAtPos(2, param2Mode);
            else
                Index += 3;
        }

        private void RunJumpIfFalse(Mode param1Mode, Mode param2Mode)
        {
            var val = GetAtPos(1, param1Mode);
            if (val == 0)
                Index = GetAtPos(2, param2Mode);
            else
                Index += 3;
        }

        private void RunLessThan(Mode param1Mode, Mode param2Mode, Mode param3Mode)
        {
            var a = GetAtPos(1, param1Mode);
            var b = GetAtPos(2, param2Mode);
            WriteToPos((a < b ? 1 : 0), 3, param3Mode);
            Index += 4;
        }

        private void RunEquals(Mode param1Mode, Mode param2Mode, Mode param3Mode)
        {
            var a = GetAtPos(1, param1Mode);
            var b = GetAtPos(2, param2Mode);
            WriteToPos((a == b ? 1 : 0), 3, param3Mode);
            Index += 4;
        }

        private void RunRelativeBaseOffset(Mode param1Mode)
        {
            relativeBase += GetAtPos(1, param1Mode);
            Index += 2;
        }


        private long GetAtPos(int offset, Mode posMode)
        {
            ExpandTo(Index + offset);

            switch (posMode)
            {
                case Mode.Position:
                    ExpandTo(program[Index + offset]);
                    return program[program[Index + offset]];
                case Mode.Absolute:
                    ExpandTo(Index + offset);
                    return program[Index + offset];
                case Mode.Relative:
                    ExpandTo(relativeBase + program[Index + offset]);
                    return program[relativeBase + program[Index + offset]];
                default:
                    throw new Exception("Unepxected prameter mode");
            }
        }

        private void WriteToPos(long value, int offset, Mode posMode)
        {
            ExpandTo(Index + offset);

            switch (posMode)
            {
                case Mode.Position:
                    ExpandTo(program[Index + offset]);
                    program[program[Index + offset]] = value;
                    break;
                case Mode.Relative:
                    ExpandTo(relativeBase + program[Index + offset]);
                    program[relativeBase + program[Index + offset]] = value;
                    break;
                default:
                    throw new Exception("Unexpected write parameter mode");
            }
        }

        private void ExpandTo(long limit)
        {
            if (limit < program.Length)
                return;

            var range = limit + 1 - program.Length;
            program = program.Concat(new long[range]).ToArray();
        }
    }
}
