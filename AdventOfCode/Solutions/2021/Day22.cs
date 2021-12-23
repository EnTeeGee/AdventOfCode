using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day22
    {
        [Solution(22, 1)]
        public long Solution1(string input)
        {
            return Run(input, true);
        }

        [Solution(22, 2)]
        public long Solution2(string input)
        {
            return Run(input, false);
        }

        private long Run(string input, bool ignoreOuter)
        {
            var info = Parser.ToArrayOfString(input).Select(it => Parser.SplitOn(it, ' ', '=', ',', '.')).ToArray();
            var points = new HashSet<Voxel>();
            var target = new Cuboid(50, -50, 50, -50, 50, -50);
            var covered = new List<Cuboid>();
            foreach (var line in info)
            {
                var values = new[] { line[2], line[3], line[5], line[6], line[8], line[9] }.Select(it => int.Parse(it)).ToArray();
                var top = new Voxel(values[0], values[2], values[4]);
                var bottom = new Voxel(values[1], values[3], values[5]);

                if (ignoreOuter && (!target.Intersects(top) || !target.Intersects(bottom)))
                    continue;

                var section = new Cuboid(values[0], values[1], values[2], values[3], values[4], values[5]);
                if (line[0] == "on")
                {
                    foreach (var item in covered)
                    {
                        item.AddSameSection(section);
                        section.AddIgnoredSection(item);
                    }

                    if (section.GetVolume() <= 0)
                        continue;

                    covered.Add(section);
                }
                else
                {
                    var toRemove = new List<Cuboid>();
                    foreach (var item in covered)
                        if (item.AddToggledSection(section))
                            toRemove.Add(item);

                    covered.RemoveAll(it => toRemove.Contains(it));
                }

                //Console.WriteLine("Current vol: " + covered.Sum(it => it.GetVolume()));
            }

            return covered.Sum(it => it.GetVolume());
        }

        private class Cuboid
        {
            public int MaxX { get; }
            public int MinX { get; }
            public int MaxY { get; }
            public int MinY { get; }
            public int MaxZ { get; }
            public int MinZ { get; }

            private List<Cuboid> ignored;
            private List<Cuboid> toggledSections;

            public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                MaxX = Math.Max(x1, x2);
                MinX = Math.Min(x1, x2);
                MaxY = Math.Max(y1, y2);
                MinY = Math.Min(y1, y2);
                MaxZ = Math.Max(z1, z2);
                MinZ = Math.Min(z1, z2);

                ignored = new List<Cuboid>();
                toggledSections = new List<Cuboid>();
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
                var xEnd = Math.Min(MaxX, target.MaxX);
                if (xStart > xEnd)
                    return null;

                var yStart = Math.Max(MinY, target.MinY);
                var yEnd = Math.Min(MaxY, target.MaxY);
                if (yStart > yEnd)
                    return null;

                var zStart = Math.Max(MinZ, target.MinZ);
                var zEnd = Math.Min(MaxZ, target.MaxZ);
                if (zStart > zEnd)
                    return null;

                return new Cuboid(xStart, xEnd, yStart, yEnd, zStart, zEnd);
            }

            public long GetVolume()
            {
                var toggledVolume = toggledSections.Sum(it => it.GetVolume());

                return GetBaseVolume() - GetIgnoredVolume() - toggledVolume;
            }

            public bool AddIgnoredSection(Cuboid section)
            {
                var overlap = GetOverlap(section);
                if (overlap == null)
                    return false;

                foreach(var item in ignored)
                {
                    if (overlap.AddIgnoredSection(item))
                        return false; // overlap covered by other overlaps
                }

                ignored.Add(overlap);
                if (GetBaseVolume() - GetIgnoredVolume() <= 0)
                    return true; // fully ignored

                var toRemove = new List<Cuboid>();
                foreach (var item in toggledSections)
                {
                    if (item.AddIgnoredSection(overlap))
                        toRemove.Add(item);
                }

                toggledSections.RemoveAll(it => toRemove.Contains(it));

                return false;
            }

            // returns true if it's been completey overwritten and no longer needs to be tracked
            public bool AddToggledSection(Cuboid section)
            {
                var subSection = GetOverlap(section);
                if (subSection == null)
                    return false;

                foreach (var item in ignored)
                {
                    if (subSection.AddIgnoredSection(item))
                        return false;
                }

                foreach (var item in toggledSections)
                    item.AddSameSection(subSection);

                foreach (var item in toggledSections)
                    if (subSection.AddIgnoredSection(item))
                        return false;

                toggledSections.Add(subSection);

                return GetVolume() <= 0;
            }

            public void AddSameSection(Cuboid section)
            {
                var toRemove = new List<Cuboid>();
                foreach (var item in toggledSections)
                {
                    if (item.AddToggledSection(section))
                        toRemove.Add(item);
                }
                toggledSections.RemoveAll(it => toRemove.Contains(it)); 
            }

            private long GetIgnoredVolume()
            {
                return ignored.Sum(it => it.GetVolume());
            }

            private long GetBaseVolume()
            {
                return (long)(MaxX - MinX + 1) * (MaxY - MinY + 1) * (MaxZ - MinZ + 1);
            }
        }
    }
}
