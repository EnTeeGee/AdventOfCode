using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var machines = Parser.ToArrayOfGroups(input).Select(it => new MachineInfo(it)).ToArray();

            return (int)machines.Select(it => GetBestScore(it)).Where(it => it != null).Sum()!;
        }

        [Solution(13, 2)]
        public long Solution2(string input)
        {
            var machines = Parser.ToArrayOfGroups(input).Select(it => new MachineInfo(it, true)).ToArray();

            return (long)machines.Select(it => GetBestScore(it)).Where(it => it != null).Sum()!;
        }

        private long? GetBestScore(MachineInfo machine)
        {
            var aPoints = new[] { Point.Origin, Point.Origin + machine.ButtonA };
            var bPoints = new[] { machine.Prize, machine.Prize + machine.ButtonB };
            var xNumerator = (((aPoints[0].X * aPoints[1].Y) - (aPoints[0].Y * aPoints[1].X)) * (bPoints[0].X - bPoints[1].X))
                - ((aPoints[0].X - aPoints[1].X) * ((bPoints[0].X * bPoints[1].Y) - (bPoints[0].Y * bPoints[1].X)));
            var xDenominator = ((aPoints[0].X - aPoints[1].X) * (bPoints[0].Y - bPoints[1].Y))
                - ((aPoints[0].Y - aPoints[1].Y) * (bPoints[0].X - bPoints[1].X));
            var yNumerator = (((aPoints[0].X * aPoints[1].Y) - (aPoints[0].Y * aPoints[1].X)) * (bPoints[0].Y - bPoints[1].Y))
                - ((aPoints[0].Y - aPoints[1].Y) * ((bPoints[0].X * bPoints[1].Y) - (bPoints[0].Y * bPoints[1].X)));
            var yDenominator = ((aPoints[0].X - aPoints[1].X) * (bPoints[0].Y - bPoints[1].Y))
                - ((aPoints[0].Y - aPoints[1].Y) * (bPoints[0].X - bPoints[1].X));

            if (xDenominator == 0 || yDenominator == 0)
                return null;

            if (xNumerator % xDenominator != 0 || yNumerator % yDenominator != 0)
                return null;

            var intersect = new Point(xNumerator / xDenominator, yNumerator / yDenominator);

            if (!intersect.WithinBounds(0, machine.Prize.X, 0, machine.Prize.Y))
                return null;

            if(intersect.X % machine.ButtonA.X == 0 && intersect.Y % machine.ButtonA.Y == 0
                && (machine.Prize.X - intersect.X) % machine.ButtonB.X == 0 && (machine.Prize.Y - intersect.Y) % machine.ButtonB.Y == 0)
                return ((intersect.X / machine.ButtonA.X) * 3) + ((machine.Prize.X - intersect.X) / machine.ButtonB.X);

            return null;

        }

        private class MachineInfo
        {
            public Point ButtonA { get; }
            public Point ButtonB { get; }
            public Point Prize { get; }

            public MachineInfo(string input, bool expanded = false)
            {
                var items = Parser.ToArrayOfString(input).SelectMany(it => Parser.SplitOn(it, '+', ',', '=')).ToArray();
                ButtonA = new Point(int.Parse(items[1]), int.Parse(items[3]));
                ButtonB = new Point(int.Parse(items[5]), int.Parse(items[7]));
                Prize = new Point(int.Parse(items[9]) + (expanded ? 10.Trillion() : 0), int.Parse(items[11]) + (expanded ? 10.Trillion() : 0));
            }
        }
    }
}
