using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Numerics;
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

        //[Solution(24, 2)]
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

            throw new Exception("Failed to find route");
        }


        
        //[Solution(24, 2)]
        public long Solution2b(string input)
        {
            var stones = Parser.ToArrayOf(input, it => new Hailstone(it));

            var aStone = stones[0];
            var bStone = stones[1];

            var aTime = 2;
            var bTime = 1;

            var testing = new Hailstone("10, 0, 0 @ -1, 0, 0");
            var result = HasValidIntersect(new Point(0, 0), new Point(1, 1), testing);


            while (true)
            {
                if(bTime == 0)
                {
                    aTime++;
                    bTime = aTime - 1;
                }

                var pos1 = new Point(aStone.PosAt(aTime).X, aTime);
                var pos2 = new Point(bStone.PosAt(bTime).X, bTime);

                if(IntersectsAllStones(pos1, pos2, stones))
                {
                    Console.WriteLine($"Found intersect for line {pos1}, {pos2}");
                    return 0;
                }
                    

                pos1 = new Point(aStone.PosAt(bTime).X, bTime);
                pos2 = new Point(bStone.PosAt(aTime).X, aTime);

                if (IntersectsAllStones(pos1, pos2, stones))
                {
                    Console.WriteLine($"Found intersect for line {pos1}, {pos2}");
                    return 0;
                }
                    

                bTime--;
            }
        }

        // Start with interects of first two stones based on avarage of 1D intersects for first two stones.
        // Then adjust these two interects by one and see how the 1D intersects with a third stone change.
        // The idea is that it should be possible to adjust one such that the ratio between the intersect differences matches the change by the second,
        // at which point the second can be applied to bring them all in line.
        [Solution(24, 2)]
        public long Solution2c(string input)
        {
            var stones = Parser.ToArrayOf(input, it => new Hailstone(it));
            var stone1 = stones[0];
            var stone2 = stones[1];
            //var stone1 = stones[2];
            //var stone2 = stones[3];

            var xIntersect = FindRoughIntersect(
                new Point(stone1.Pos.X, 0),
                new Point(stone1.Pos.X + stone1.Vel.X, 1),
                new Point(stone2.Pos.X, 0),
                new Point(stone2.Pos.X + stone2.Vel.X, 1));
            var yIntersect = FindRoughIntersect(
                new Point(stone1.Pos.Y, 0),
                new Point(stone1.Pos.Y + stone1.Vel.Y, 1),
                new Point(stone2.Pos.Y, 0),
                new Point(stone2.Pos.Y + stone2.Vel.Y, 1));
            var zIntersect = FindRoughIntersect(
                new Point(stone1.Pos.Z, 0),
                new Point(stone1.Pos.Z + stone1.Vel.Z, 1),
                new Point(stone2.Pos.Z, 0),
                new Point(stone2.Pos.Z + stone2.Vel.Z, 1));
            var timeEstimate = (xIntersect.pos + yIntersect.pos + zIntersect.pos) / 3;

            var baseline = GetThirdStoneIntersects(stones, timeEstimate + 1, timeEstimate - 1);
            var baselineRatio = (double)(baseline.x - baseline.y) / (baseline.y - baseline.z);
            var aStep = GetThirdStoneIntersects(stones, timeEstimate + 1.Billion(), timeEstimate);
            var aStepRatio = (double)(aStep.Item1 - aStep.Item2) / (aStep.Item2 - aStep.Item3);
            var aDiffs = (baseline.x - aStep.x, baseline.y - aStep.y, baseline.z - aStep.z);
            var aDiffsRatio = (double)(aDiffs.Item1 - aDiffs.Item2) / (aDiffs.Item2 - aDiffs.Item3);
            var bStep = GetThirdStoneIntersects(stones, timeEstimate, timeEstimate + 1.Billion());
            var bDiffs = (baseline.x - bStep.x, baseline.y - bStep.y, baseline.z - bStep.z);
            var bRatio = (double)(bDiffs.Item1 - bDiffs.Item2) / (bDiffs.Item2 - bDiffs.Item3);

            var step = 0x100000000;
            var aTarget = timeEstimate + 1.Billion();
            var currentARatio = aStepRatio;

            while (step > 0)
            {
                var up = aTarget + step;
                var down = aTarget - step;
                var upRatio = GetRatioOf(stones, up, timeEstimate);
                var downRatio = GetRatioOf(stones, down, timeEstimate);

                var currentDiff = Math.Abs(bRatio - currentARatio);
                var upDiff = Math.Abs(upRatio -  bRatio);
                var downDiff = Math.Abs(downRatio - bRatio);

                if (upDiff < currentDiff)
                    aTarget = up;
                else if (downDiff < currentDiff)
                    aTarget = down;
                else
                    step /= 2;
                var newStep = GetThirdStoneIntersects(stones, aTarget, timeEstimate);
                currentARatio = (double)(newStep.Item1 - newStep.Item2) / (newStep.Item2 - newStep.Item3);
            }

            step = 0x100000000;
            var bTarget = timeEstimate + 1.Billion();
            var currentSpread = new long[] { bStep.x, bStep.y, bStep.z }.Max() - new long[] { bStep.x, bStep.y, bStep.z }.Min();

            while(step > 0 && currentSpread > 0)
            {
                var up = bTarget + step;
                var down = bTarget - step;
                if (up == aTarget)
                    up += 1;
                if (down == aTarget)
                    down -= 1;

                var upIntersect = GetThirdStoneIntersects(stones, aTarget, up);
                var downIntersect = GetThirdStoneIntersects(stones, aTarget, down);
                var upSpread = new long[] { upIntersect.x, upIntersect.y, upIntersect.z }.Max()
                    - new long[] { upIntersect.x, upIntersect.y, upIntersect.z }.Min();
                var downSpread = new long[] { downIntersect.x, downIntersect.y, downIntersect.z }.Max()
                    - new long[] { downIntersect.x, downIntersect.y, downIntersect.z }.Min();

                if(upSpread < currentSpread)
                {
                    bTarget = up;
                    currentSpread = upSpread;
                }
                else if (downSpread < currentSpread)
                {
                    bTarget = down;
                    currentSpread = downSpread;
                }
                else
                {
                    step /= 2;
                }
            }

            //Testing:
            var aPos = new Point(stone1.PosAt(aTarget).X, aTarget);
            var bPos = new Point(stone2.PosAt(bTarget).X, bTarget);
            var test = GetThirdStoneIntersects(stones, aTarget, bTarget);
            var result = IntersectsAllStones(aPos, bPos, stones);

            //var lcm = Factor.
            //var test = Factor.GCD(3, 9);
            //var test2 = Factor.GCD(7, 11);

            var aPoint = stone1.PosAt(aTarget);
            var bPoint = stone2.PosAt(bTarget);
            var fullStep = bPoint - aPoint;

            var gcd = Factor.GCD(fullStep.X, fullStep.Y, fullStep.Z, bTarget - aTarget);
            var finalStep = new Voxel(fullStep.X / gcd, fullStep.Y / gcd, fullStep.Z / gcd);
            var finalTimeStep = (bTarget - aTarget) / gcd;

            return 0;
        }

        private double GetRatioOf(Hailstone[] stones, long aTime, long bTime)
        {
            var step = GetThirdStoneIntersects(stones, aTime, bTime);
            return (double)(step.Item1 - step.Item2) / (step.Item2 - step.Item3);
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

            public Voxel PosAt(long time)
            {
                return new Voxel(
                    Pos.X + (Vel.X * time),
                    Pos.Y + (Vel.Y * time),
                    Pos.Z + (Vel.Z * time));
            }
        }

        private bool IntersectsAllStones(Point pos1, Point pos2, Hailstone[] stones)
        {
            for (var i = 2; i < stones.Length; i++)
            {
                var result = HasValidIntersect(pos1, pos2, stones[i]);
                if (result == null)
                    return false;

                //Console.WriteLine($"Found an intercept at time {result.Value}");
            }

            return true;
        }

        private long? HasValidIntersect(Point pos1, Point pos2, Hailstone stone)
        {
            var x1 = new BigInteger(pos1.X);
            var x2 = new BigInteger(pos2.X);
            var x3 = new BigInteger(stone.Pos.X);
            var x4 = new BigInteger(stone.Pos.X + stone.Vel.X);
            var y1 = new BigInteger(pos1.Y);
            var y2 = new BigInteger(pos2.Y);
            var y3 = BigInteger.Zero;
            var y4 = BigInteger.One;

            //var pos3 = new Point(stone.Pos.X, 0);
            //var pos4 = new Point(stone.Pos.Y + stone.Vel.X, 1);

            var pxNum = (((x1 * y2) - (y1 * x2)) * (x3 - x4)) - ((x1 - x2) * ((x3 * y4) - (y3 * x4)));
            var pyNum = (((x1 * y2) - (y1 * x2)) * (y3 - y4)) - ((y1 - y2) * ((x3 * y4) - (y3 * x4)));
            var den = ((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4));

            if (den == 0)
                return null;

            if (pxNum % den != 0 || pyNum % den != 0)
                return null;

            var result = pyNum / den;

            if (result <= 0)
                return null;

            return (long)result;
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

        private (BigInteger xNum, BigInteger yNum, BigInteger den) FindFractionalIntersect(Point pos1, Point pos2, Point pos3, Point pos4)
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

            return (pxNum, pyNum, den);
        }

        private (long x, long y, long z) GetThirdStoneIntersects(Hailstone[] stones, long aTime, long bTime)
        {
            var aStone = stones[0].PosAt(aTime);
            var bStone = stones[1].PosAt(bTime);
            var cStone = stones[2];
            //var cStone = stones[50];

            var x = FindRoughIntersect(
                new Point(aStone.X, aTime),
                new Point(bStone.X, bTime),
                new Point(cStone.Pos.X, 0),
                new Point(cStone.Pos.X + cStone.Vel.X, 1)).pos;
            var y = FindRoughIntersect(
                new Point(aStone.Y, aTime),
                new Point(bStone.Y, bTime),
                new Point(cStone.Pos.Y, 0),
                new Point(cStone.Pos.Y + cStone.Vel.Y, 1)).pos;
            var z = FindRoughIntersect(
                new Point(aStone.Z, aTime),
                new Point(bStone.Z, bTime),
                new Point(cStone.Pos.Z, 0),
                new Point(cStone.Pos.Z + cStone.Vel.Z, 1)).pos;

            return (x, y, z);
        }

        private long? FocusUsingAPoint(Hailstone[] stones, long aPoint, long start)
        {
            var step = 0x100000000;
            var bTarget = start;
            var bStep = GetThirdStoneIntersects(stones, aPoint, start);
            var currentSpread = new long[] { bStep.x, bStep.y, bStep.z }.Max() - new long[] { bStep.x, bStep.y, bStep.z }.Min();

            while (step > 0 && currentSpread > 0)
            {
                var up = bTarget + step;
                var down = bTarget - step;
                if (up == aPoint)
                    up += 1;
                if (down == aPoint)
                    down -= 1;
                if (down <= 0)
                    down = 0;

                var upIntersect = GetThirdStoneIntersects(stones, aPoint, up);
                var downIntersect = GetThirdStoneIntersects(stones, aPoint, down);
                var upSpread = new long[] { upIntersect.x, upIntersect.y, upIntersect.z }.Max()
                    - new long[] { upIntersect.x, upIntersect.y, upIntersect.z }.Min();
                var downSpread = new long[] { downIntersect.x, downIntersect.y, downIntersect.z }.Max()
                    - new long[] { downIntersect.x, downIntersect.y, downIntersect.z }.Min();

                if (upSpread < currentSpread)
                {
                    bTarget = up;
                    currentSpread = upSpread;
                }
                else if (downSpread < currentSpread)
                {
                    bTarget = down;
                    currentSpread = downSpread;
                }
                else
                {
                    step /= 2;
                }
            }

            return currentSpread > 0 ? null : bTarget;
        }
    }
}
