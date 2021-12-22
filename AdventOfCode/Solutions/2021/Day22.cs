using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2021
{
    class Day22
    {
        [Solution(22, 1)]
        public int Solution1(string input)
        {
            var info = Parser.ToArrayOfString(input).Select(it => Parser.SplitOn(it, ' ', '=', ',', '.')).ToArray();
            var points = new HashSet<Voxel>();
            var target = new Cuboid(50, -50, 50, -50, 50, -50);
            foreach(var line in info)
            {
                var values = new[] { line[2], line[3], line[5], line[6], line[8], line[9] }.Select(it => int.Parse(it)).ToArray();
                var top = new Voxel(values[0], values[2], values[4]);
                var bottom = new Voxel(values[1], values[3], values[5]);

                if (!target.Intersects(top) || !target.Intersects(bottom))
                    continue;

                for(var i = top.X; i <= bottom.X; i++)
                {
                    for(var j = top.Y; j <= bottom.Y; j++)
                    {
                        for(var k = top.Z; k <= bottom.Z; k++)
                        {
                            var point = new Voxel(i, j, k);
                            if (points.Contains(point))
                                points.Remove(point);
                            else
                                points.Add(point);
                        }
                    }
                }
            }

            return points.Count();
        }

        // https://stackoverflow.com/questions/5556170/finding-shared-volume-of-two-overlapping-cuboids

        private class Cuboid
        {
            public int MaxX { get; }
            public int MinX { get; }
            public int MaxY { get; }
            public int MinY { get; }
            public int MaxZ { get; }
            public int MinZ { get; }

            public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                MaxX = Math.Max(x1, x2);
                MinX = Math.Min(x1, x2);
                MaxY = Math.Max(y1, y2);
                MinY = Math.Min(y1, y2);
                MaxZ = Math.Max(z1, z2);
                MinZ = Math.Min(z1, z2);
            }

            public bool Intersects(Voxel point)
            {
                return point.X <= MaxX && point.X >= MinX
                    && point.Y <= MaxY && point.Y >= MinY
                    && point.Z <= MaxZ && point.Z >= MinZ;
            }

            public Cuboid GetOverlap(Cuboid target)
            {
                var xStart = Math.Max(MinX, target.MinX);
                var xEnd = Math.Min(MaxX, target.MinX);
                if (xStart > xEnd)
                    return null;

                var yStart = Math.Max(MinY, target.MinY);
                var yEnd = Math.Min(MaxY, target.MinY);
                if (yStart > yEnd)
                    return null;

                var zStart = Math.Max(MinZ, target.MinZ);
                var zEnd = Math.Min(MaxZ, target.MinZ);
                if (zStart > zEnd)
                    return null;

                return new Cuboid(xStart, xEnd, yStart, yEnd, zStart, zEnd);
            }
        }
    }
}
