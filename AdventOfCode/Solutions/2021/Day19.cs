using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day19
    {
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var scanners = Parser.ToArrayOfGroups(input).Select(it => new Scanner(it)).ToArray();
            var arrangedBeacons = ArrangeBeacons(scanners);

            return arrangedBeacons.Count();
        }

        [Solution(19, 2)]
        public int Solution2(string input)
        {
            var scanners = Parser.ToArrayOfGroups(input).Select(it => new Scanner(it)).ToArray();
            var arrangedBeacons = ArrangeBeacons(scanners);

            var maxDist = 0;
            for(var i = 0; i < scanners.Length; i++)
            {
                for (var j = i + 1; j < scanners.Length; j++)
                {
                    var dist = scanners[i].ScannerPos.GetTaxicabDistanceTo(scanners[j].ScannerPos);
                    maxDist = Math.Max(dist, maxDist);
                }
            }

            return maxDist;
        }

        private HashSet<Voxel> ArrangeBeacons(Scanner[] scanners)
        {
            var matchedPoints = new HashSet<Voxel>(scanners[0].Points);

            var unmoved = scanners.Skip(1).ToArray();
            while (unmoved.Any())
            {
                foreach (var item in unmoved)
                {
                    var match = item.Matches(matchedPoints);
                    if (match)
                    {
                        foreach (var point in item.Points.Where(it => !matchedPoints.Contains(it)))
                            matchedPoints.Add(point);
                    }
                }

                var newUnmoved = unmoved.Where(it => !it.Moved).ToArray();
                if (newUnmoved.Length == unmoved.Length)
                    throw new Exception("Failed to associate any more scanners");

                unmoved = newUnmoved;
            }

            return matchedPoints;
        }

        private class Scanner
        {
            public Voxel[] Points { get; private set; }
            public bool Moved { get; private set; }
            public Voxel ScannerPos { get; private set; }

            public Scanner(string input)
            {
                Points = Parser
                    .ToArrayOfString(input)
                    .Skip(1)
                    .Select(it => Parser.SplitOn(it, ',').Select(a => int.Parse(a)).ToArray())
                    .Select(it => new Voxel(it[0], it[1], it[2]))
                    .ToArray();
                ScannerPos = Voxel.Origin;
            }

            public void MoveTo(Voxel pos)
            {
                Points = Points.Select(it => it.Add(pos)).ToArray();
                ScannerPos = ScannerPos.Add(pos);
                Moved = true;
            }

            public bool Matches(HashSet<Voxel> input)
            {
                var orientations = Points.Select(it => it.GetAllOrientations());
                var groups = Enumerable.Range(0, 24).Select(it => orientations.Select(x => x[it]).ToArray()).ToArray();

                foreach(var group in groups)
                {
                    foreach (var item in group)
                    {
                        foreach (var target in input)
                        {
                            var offset = item.OffsetTo(target);

                            var matching = group.Select(it => it.Add(offset)).Where(it => input.Contains(it)).Count();

                            if (matching >= 12)
                            {
                                Points = group;
                                MoveTo(offset);

                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}
