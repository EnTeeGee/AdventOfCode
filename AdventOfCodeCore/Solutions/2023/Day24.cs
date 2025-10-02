using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Numerics;


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

        // https://advent-of-code.xavd.id/writeups/2023/day/24/
        [Solution(24, 2)]
        public long Solution2(string input)
        {
            var stones = Parser.ToArrayOf(input, it => new Hailstone(it));
            long[]? viableX = null;
            long[]? viableY = null;
            long[]? viableZ = null;

            for (var i = 0; i < stones.Length - 1; i++)
            {
                var current = stones[i];
                var matchingX = stones.Skip(i + 1).FirstOrDefault(it => it.Vel.X == current.Vel.X && it.Pos.X != current.Pos.X);
                var matchingY = stones.Skip(i + 1).FirstOrDefault(it => it.Vel.Y == current.Vel.Y && it.Pos.Y != current.Pos.Y);
                var matchingZ = stones.Skip(i + 1).FirstOrDefault(it => it.Vel.Z == current.Vel.Z && it.Pos.Z != current.Pos.Z);


                if (matchingX != null)
                    viableX = GetViableVelocities(current.Vel.X, current.Pos.X - matchingX.Pos.X, viableX);
                if (matchingY != null)
                    viableY = GetViableVelocities(current.Vel.Y, current.Pos.Y - matchingY.Pos.Y, viableY);
                if (matchingZ != null)
                    viableZ = GetViableVelocities(current.Vel.Z, current.Pos.Z - matchingZ.Pos.Z, viableZ);
            }

            if (viableX == null || viableY == null || viableZ == null)
                throw new Exception("Failed to find valid velocity");

            var viableVel = new Voxel(viableX[0], viableY[0], viableZ[0]);
            var viableStarts1 = new Hailstone(stones[0].Pos, stones[0].Vel - viableVel);
            var viableStarts2 = new Hailstone(stones[1].Pos, stones[1].Vel - viableVel);

            var test1 = viableStarts1.PosAt(1) + viableVel;
            var test2 = stones[0].PosAt(1);

            var result = GetInterceptIfExists(viableStarts1, viableStarts2);
            if (result != null)
                return result.X + result.Y + result.Z;

            throw new Exception("Failed to find intercept");
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

            public Hailstone(Voxel pos, Voxel vel)
            {
                Pos = pos;
                Vel = vel;
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

            public bool CollidesWith(Voxel start, Voxel step)
            {
                var currentPos = start;
                var currentTarget = Pos;
                var lastDist = currentPos.GetTaxicabDistanceTo(currentTarget);

                while (true)
                {
                    if (currentPos.Equals(currentTarget))
                        return true;

                    currentPos = currentPos.Add(step);
                    currentTarget = currentTarget.Add(Vel);
                    var newDist = currentPos.GetTaxicabDistanceTo(currentTarget);
                    if (newDist > lastDist)
                        return false;

                    lastDist = newDist;
                }
            }

            public Voxel PosAt(long time)
            {
                return new Voxel(
                    Pos.X + (Vel.X * time),
                    Pos.Y + (Vel.Y * time),
                    Pos.Z + (Vel.Z * time));
            }
        }

        private (long pos, bool exact) FindRoughIntersect(Point pos1, Point pos2, Point pos3, Point pos4)
        {
            var x1 = new BigInteger(pos1.X);
            var x2 = new BigInteger(pos2.X);
            var x3 = new BigInteger(pos3.X);
            var x4 = new BigInteger(pos4.X);
            var y1 = new BigInteger(pos1.Y);
            var y2 = new BigInteger(pos2.Y);
            var y3 = new BigInteger(pos3.Y);
            var y4 = new BigInteger(pos4.Y);

            var pxNum = (((x1 * y2) - (y1 * x2)) * (x3 - x4)) - ((x1 - x2) * ((x3 * y4) - (y3 * x4)));
            var pyNum = (((x1 * y2) - (y1 * x2)) * (y3 - y4)) - ((y1 - y2) * ((x3 * y4) - (y3 * x4)));
            var den = ((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));

            var exact = den != 0 && pxNum % den == 0 && pyNum % den == 0;
            var result = pyNum / den;

            return ((long)result, exact);
        }

        private long[] GetViableVelocities(long velocity, long seperation, long[]? existing)
        {
            var limit = 1000;
            var output = new List<long>();
            seperation = Math.Abs(seperation);

            for(var i = 1; i < limit; i++)
            {
                if (i != velocity && (seperation % Math.Abs(i - velocity)) == 0)
                    output.Add(i);
                if (-i != velocity && (seperation % Math.Abs(-i - velocity)) == 0)
                    output.Add(-i);
            }

            if(existing == null)
                return output.Distinct().ToArray();

            var hash = output.ToHashSet();

            var result = existing.Where(it => hash.Contains(it)).ToArray();
            if (!result.Any())
                throw new Exception("Found no valid velocities");

            return result;
        }

        private Voxel? GetInterceptIfExists(Hailstone first, Hailstone second)
        {
            var xy = FindRoughIntersect(
                new Point(first.Pos.X, first.Pos.Y),
                new Point(first.PosAt(1).X, first.PosAt(1).Y),
                new Point(second.Pos.X, second.Pos.Y),
                new Point(second.PosAt(1).X, second.PosAt(1).Y));
            var yz = FindRoughIntersect(
                new Point(first.Pos.Y, first.Pos.Z),
                new Point(first.PosAt(1).Y, first.PosAt(1).Z),
                new Point(second.Pos.Y, second.Pos.Z),
                new Point(second.PosAt(1).Y, second.PosAt(1).Z));
            var zx = FindRoughIntersect(
                new Point(first.Pos.Z, first.Pos.X),
                new Point(first.PosAt(1).Z, first.PosAt(1).X),
                new Point(second.Pos.Z, second.Pos.X),
                new Point(second.PosAt(1).Z, second.PosAt(1).X));

            if (!xy.exact || !yz.exact || !zx.exact)
                return null;

            return new Voxel(xy.pos, yz.pos, zx.pos);
        }
    }
}
