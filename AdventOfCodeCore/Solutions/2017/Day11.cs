using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day11
    {
        [Solution(11, 1)]
        public int Solution1(string input)
        {
            var steps = Parser.SplitOn(input, ',');
            var pos = Voxel.Origin;

            foreach(var item in steps)
                pos = MapDirectionToMove(pos, item);

            return (int)pos.GetTaxicabDistanceTo(Voxel.Origin) / 2;
        }

        [Solution(11, 2)]
        public int Solution2(string input)
        {
            var steps = Parser.SplitOn(input, ',');
            var pos = Voxel.Origin;
            var maxDist = 0;

            foreach (var item in steps)
            {
                pos = MapDirectionToMove(pos, item);
                maxDist = Math.Max(maxDist, (int)pos.GetTaxicabDistanceTo(Voxel.Origin) / 2);
            }

            return maxDist;
        }

        private Voxel MapDirectionToMove(Voxel start, string input)
        {
            switch (input)
            {
                case "n":
                    return new Voxel(start.X, start.Y + 1, start.Z - 1);
                case "ne":
                    return new Voxel(start.X - 1, start.Y + 1, start.Z);
                case "se":
                    return new Voxel(start.X - 1, start.Y, start.Z + 1);
                case "s":
                    return new Voxel(start.X, start.Y - 1, start.Z + 1);
                case "sw":
                    return new Voxel(start.X + 1, start.Y - 1, start.Z);
                case "nw":
                    return new Voxel(start.X + 1, start.Y, start.Z - 1);
                default:
                    throw new Exception("Unexpected move");
            }
        }
    }
}
