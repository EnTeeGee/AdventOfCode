using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day24
    {
        [Solution(24, 1)]
        public int Solution1(string input)
        {
            var stones = Parser.ToArrayOf(input, it => new Hailstone(it));

            //var boundsMin = new Point(7, 7);
            //var boundsMax = new Point(27, 27);
            var boundsMin = new Point(200.Trillion(), 200.Trillion());
            var boundsMax = new Point(400.Trillion(), 400.Trillion());

            var pairs = Permutations.GetAllPossiblePairs(stones);
            var result = pairs.Select(it => it[0].Get2dIntercept(it[1]))
                .Where(it => it != null
                    && it.Value.x >= boundsMin.X
                    && it.Value.x <= boundsMax.X
                    && it.Value.y >= boundsMin.Y
                    && it.Value.y <= boundsMax.Y)
                .Count();

            return result;
        }

        private class Hailstone
        {
            public Voxel Pos { get; }
            public Voxel Vel { get; }

            public Hailstone(string input)
            {
                var chunks = Parser.SplitOn(input, '@', ',', ' ').Select(it => long.Parse(it)).ToArray();
                Pos = new Voxel(chunks[0], chunks[1], chunks[2]);
                Vel = new Voxel(chunks[3], chunks[4], chunks[5]);
            }

            public (double x, double y)? Get2dIntercept(Hailstone other)
            {
                var gradient = (double)Vel.Y / Vel.X;
                var otherGradient = (double)other.Vel.Y / other.Vel.X;

                if (gradient == otherGradient)
                    return null;

                var ax = gradient * Pos.X;
                var c = Pos.Y - ax;

                var otherAx = otherGradient * other.Pos.X;
                var otherC = other.Pos.Y - otherAx;

                var xIntercept = (otherC - c) / (gradient - otherGradient);
                var yIntercept = (gradient * xIntercept) + c;

                if ((Vel.X > 0 && xIntercept < Pos.X) || (Vel.X < 0 && xIntercept > Pos.X))
                    return null;

                if ((other.Vel.X > 0 && xIntercept < other.Pos.X) || (other.Vel.X < 0 && xIntercept > other.Pos.X))
                    return null;

                return (xIntercept, yIntercept);
            }
        }
    }
}
