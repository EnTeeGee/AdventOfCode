using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day25
    {
        [Solution(25, 1)]
        public int Solution1(string input)
        {
            var groups = Parser.ToArrayOfGroups(input);
            var instructions = groups.Skip(1).Select(it => new Instruction(it)).ToArray();
            var intro = Parser.ToArrayOfString(groups[0]);
            var state = Parser.SplitOn(intro[0], ' ', '.')[3];
            var steps = int.Parse(Parser.SplitOnSpace(intro[1])[5]);
            var tape = new HashSet<int>();
            var pointer = 0;

            for(var i = 0; i < steps; i++)
            {
                var matching = instructions.First(it => it.State == state);
                var result = matching.UpdateTape(pointer, tape);
                state = result.state;
                pointer = result.pointer;
            }

            return tape.Count();
        }

        private class Instruction
        {
            public string State { get; }

            public bool zeroWrite;
            public bool zeroMoveLeft;
            public string zeroState;
            public bool oneWrite;
            public bool oneMoveLeft;
            public string oneState;


            public Instruction(string input)
            {
                var lines = Parser.ToArrayOfString(input);
                State = Parser.SplitOn(lines[0], ' ', ':')[2];
                zeroWrite = Parser.SplitOn(lines[2], ' ', '.')[4] == "1";
                zeroMoveLeft = Parser.SplitOn(lines[3], ' ', '.')[6] == "left";
                zeroState = Parser.SplitOn(lines[4], ' ', '.')[4];
                oneWrite = Parser.SplitOn(lines[6], ' ', '.')[4] == "1";
                oneMoveLeft = Parser.SplitOn(lines[7], ' ', '.')[6] == "left";
                oneState = Parser.SplitOn(lines[8], ' ', '.')[4];
            }

            public (int pointer, string state) UpdateTape(int currentPointer, HashSet<int> tape)
            {
                if (tape.Contains(currentPointer))
                {
                    if (!oneWrite)
                        tape.Remove(currentPointer);
                    var newPointer = currentPointer + (oneMoveLeft ? -1 : 1);

                    return (newPointer, oneState);
                }
                else
                {
                    if (zeroWrite)
                        tape.Add(currentPointer);
                    var newPointer = currentPointer + (zeroMoveLeft ? -1 : 1);

                    return (newPointer, zeroState);
                }
            }
        }
    }
}
