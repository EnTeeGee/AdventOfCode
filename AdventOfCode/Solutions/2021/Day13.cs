using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2021
{
    class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var points = Parser.ToArrayOf(sections[0], it => { var split = Parser.SplitOn(it, ','); return new Point(int.Parse(split[0]), int.Parse(split[1])); });
            var directions = Parser.ToArrayOfString(sections[1]).Select(it => new Instruction(it)).ToArray();

            return FoldPoints(points, directions[0]).Count();
        }

        [Solution(13, 2)]
        public string Solution2(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var points = Parser.ToArrayOf(sections[0], it => { var split = Parser.SplitOn(it, ','); return new Point(int.Parse(split[0]), int.Parse(split[1])); });
            var directions = Parser.ToArrayOfString(sections[1]).Select(it => new Instruction(it)).ToArray();

            var finalPoints = directions.Aggregate(points, (acc, i) => FoldPoints(acc, i)).ToHashSet();
            var finalSize = new Point(finalPoints.Max(it => it.X), finalPoints.Max(it => it.Y));
            var output = new StringBuilder();

            for (var i = 0; i <= finalSize.Y; i++)
            {
                for(var j = 0; j <= finalSize.X; j++)
                    output.Append(finalPoints.Contains(new Point(j, i)) ? "#" : ".");
                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }

        private Point[] FoldPoints(Point[] points, Instruction instruction)
        {
            if(instruction.Direction == "x")
                return points.Select(it => new Point(it.X > instruction.Location ? instruction.Location - (it.X - instruction.Location) : it.X, it.Y)).Distinct().ToArray();
            else
                return points.Select(it => new Point(it.X, it.Y > instruction.Location ? instruction.Location - (it.Y - instruction.Location) : it.Y)).Distinct().ToArray();
        }

        private class Instruction
        {
            public string Direction { get; }
            public int Location { get; }

            public Instruction(string input)
            {
                var split = Parser.SplitOn(input, ' ', '=');
                Direction = split[2];
                Location = int.Parse(split[3]);
            }
        }
    }
}
