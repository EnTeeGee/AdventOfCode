using AdventOfCode.Common;
using AdventOfCode.Core;
using System;

namespace AdventOfCode.Solutions._2015
{
    class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            var instructions = Parser.ToArrayOf(input, it => new Instruction(it));
            var lightGrid = new bool[1000, 1000];

            foreach(var item in instructions)
            {
                for(var i = item.TopLeft.Y; i <= item.BottomRight.Y; i++)
                {
                    for (var j = item.TopLeft.X; j <= item.BottomRight.X; j++)
                    {
                        if (item.InstructionType == InstructionType.TurnOn)
                            lightGrid[j, i] = true;
                        else if (item.InstructionType == InstructionType.TurnOff)
                            lightGrid[j, i] = false;
                        else
                            lightGrid[j, i] = !lightGrid[j, i];
                    }
                }
            }

            var total = 0;

            for (var i = 0; i < 1000; i++)
                for (var j = 0; j < 1000; j++)
                    if (lightGrid[j, i])
                        total++;

            return total;
        }

        [Solution(6, 2)]
        public long Solution2(string input)
        {
            var instructions = Parser.ToArrayOf(input, it => new Instruction(it));
            var lightGrid = new int[1000, 1000];

            foreach (var item in instructions)
            {
                for (var i = item.TopLeft.Y; i <= item.BottomRight.Y; i++)
                {
                    for (var j = item.TopLeft.X; j <= item.BottomRight.X; j++)
                    {
                        if (item.InstructionType == InstructionType.TurnOn)
                            lightGrid[j, i] += 1;
                        else if (item.InstructionType == InstructionType.TurnOff)
                            lightGrid[j, i] = Math.Max(lightGrid[j, i] - 1, 0);
                        else
                            lightGrid[j, i] += 2;
                    }
                }
            }

            var total = 0L;

            for (var i = 0; i < 1000; i++)
                for (var j = 0; j < 1000; j++)
                    total += lightGrid[i, j];

            return total;
        }

        private class Instruction
        {
            public Instruction(string input)
            {
                var tokens = Parser.SplitOnSpace(input);
                if(tokens[0] == "toggle")
                {
                    var pair1 = Parser.SplitOn(tokens[1], ',');
                    var pair2 = Parser.SplitOn(tokens[3], ',');
                    TopLeft = new Point(int.Parse(pair1[0]), int.Parse(pair1[1]));
                    BottomRight = new Point(int.Parse(pair2[0]), int.Parse(pair2[1]));
                    InstructionType = InstructionType.Toggle;
                }
                else
                {
                    var pair1 = Parser.SplitOn(tokens[2], ',');
                    var pair2 = Parser.SplitOn(tokens[4], ',');
                    TopLeft = new Point(int.Parse(pair1[0]), int.Parse(pair1[1]));
                    BottomRight = new Point(int.Parse(pair2[0]), int.Parse(pair2[1]));
                    InstructionType = tokens[1] == "on" ? InstructionType.TurnOn : InstructionType.TurnOff;
                }
            }

            public Point TopLeft { get; }
            
            public Point BottomRight { get; }

            public InstructionType InstructionType { get; }
        }

        private enum InstructionType { TurnOn, Toggle, TurnOff }
    }
}
