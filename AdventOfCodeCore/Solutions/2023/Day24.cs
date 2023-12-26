using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;

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

        [Solution(24, 2)]
        public long Solution2(string input)
        {
            // The target will be a line that intersects with every other line.
            // For each line, take pos at t = 1
            // then find the next nearest
            var stones = Parser.ToArrayOf(input, it => new Hailstone(it));
            var limit = 10000;

            for(var stepsUntilFirst = 1; stepsUntilFirst < limit; stepsUntilFirst++)
            {
                for (var i = stepsUntilFirst + 1; i < limit; i++)
                {
                    foreach (var item in stones)
                    {
                        //var atStart = item.Pos.Add(item.Vel);
                        var atStart = ShiftBy(item.Pos, item.Vel, stepsUntilFirst);
                        foreach (var next in stones)
                        {
                            if (next == item)
                                continue;

                            var posAt = ShiftBy(next.Pos, next.Vel, i);
                            if (((posAt.X - atStart.X) % (i - stepsUntilFirst) != 0)
                                || ((posAt.Y - atStart.Y) % (i - stepsUntilFirst) != 0)
                                || ((posAt.Z - atStart.Z) % (i - stepsUntilFirst) != 0))
                            {
                                continue;
                            }

                            var resultingStep = new Voxel(
                                (posAt.X - atStart.X) / (i - stepsUntilFirst),
                                (posAt.Y - atStart.Y) / (i - stepsUntilFirst),
                                (posAt.Z - atStart.Z) / (i - stepsUntilFirst));
                            var startPoint = new Voxel(
                                atStart.X - (resultingStep.X * stepsUntilFirst),
                                atStart.Y - (resultingStep.Y * stepsUntilFirst),
                                atStart.Z - (resultingStep.Z * stepsUntilFirst));

                            var toCheck = stones.Where(it => it != item && it != next).ToArray();
                            var allValid = true;

                            foreach (var other in toCheck)
                            {
                                if (!other.CollidesWith(startPoint, resultingStep))
                                {
                                    allValid = false;
                                    break;
                                }
                            }

                            if (!allValid)
                                continue;

                            return startPoint.X + startPoint.Y + startPoint.Z;
                        }
                    }
                }
            }

            //for(var i = 2; i < int.MaxValue; i++)
            //{
            //    foreach (var item in stones)
            //    {
            //        var atStart = item.Pos.Add(item.Vel);
            //        foreach(var next in stones)
            //        {
            //            if (next == item)
            //                continue;

            //            var posAt = ShiftBy(next.Pos, next.Vel, i);
            //            if(((posAt.X - atStart.X) % (i - 1) != 0)
            //                || ((posAt.Y - atStart.Y) % (i - 1) != 0)
            //                || ((posAt.Z - atStart.Z) % (i - 1) != 0))
            //            {
            //                continue;
            //            }

            //            var resultingStep = new Voxel(
            //                (posAt.X - atStart.X) / (i - 1),
            //                (posAt.Y - atStart.Y) / (i - 1),
            //                (posAt.Z - atStart.Z) / (i - 1));
            //            var startPoint = new Voxel(atStart.X - resultingStep.X, atStart.Y - resultingStep.Y, atStart.Z - resultingStep.Z);

            //            var toCheck = stones.Where(it => it != item && it != next).ToArray();
            //            var allValid = true;

            //            foreach (var other in toCheck)
            //            {
            //                if (!other.CollidesWith(startPoint, resultingStep))
            //                {
            //                    allValid = false;
            //                    break;
            //                }
            //            }

            //            if (!allValid)
            //                continue;

            //            return startPoint.X + startPoint.Y + startPoint.Z;
            //        }

            //        //var closest = stones.Where(it => it != item).OrderBy(it => it.Pos.Add(it.Vel).Add(it.Vel).GetTaxicabDistanceTo(atStart)).First();
            //        //var closest = stones.Where(it => it != item).OrderBy(it => ShiftBy(it.Pos, it.Vel, i).GetTaxicabDistanceTo(atStart)).First();
            //        //var closestPos = closest.Pos.Add(closest.Vel).Add(closest.Vel);
            //        //var resultingStep = new Voxel(closestPos.X - atStart.X, closestPos.Y - atStart.Y, closestPos.Z - atStart.Z);
            //        //var startPoint = new Voxel(atStart.X - resultingStep.X, atStart.Y - resultingStep.Y, atStart.Z - resultingStep.Z);

            //        //var toCheck = stones.Where(it => it != item && it != closest).ToArray();
            //        //var allValid = true;

            //        //foreach (var other in toCheck)
            //        //{
            //        //    if (!other.CollidesWith(startPoint, resultingStep))
            //        //    {
            //        //        allValid = false;
            //        //        break;
            //        //    }
            //        //}

            //        //if (!allValid)
            //        //    continue;

            //        //return startPoint.X + startPoint.Y + startPoint.Z;
            //    }
            //}

            throw new Exception("Failed to find route");
        }

        private Voxel ShiftBy(Voxel start, Voxel step, int multiple)
        {
            return start.Add(new Voxel(step.X * multiple, step.Y * multiple, step.Z * multiple));
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
        }
    }
}
