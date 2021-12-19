using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2021
{
    class Day19
    {
        // 419 too high
        // 289 too low
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            //var test = new Voxel(1, 2, 3);
            //var testRotates = test.GetAllOrientations().OrderBy(it => it.X).ThenBy(it => it.Y).ThenBy(it => it.Z).ToArray();

            //return testRotates.Distinct().Count();


            var scanners = Parser.ToArrayOfGroups(input).Select(it => new Scanner(it)).ToArray();
            //var matchedPoints = scanners[0].Points;
            var matchedPoints = new HashSet<Voxel>(scanners[0].Points);

            //for(var i = 0; i < scanners.Length; i++)
            //{
            //    for(var j = i + 1; j < scanners.Length; j++)
            //    {
            //        if (scanners[j].Moved) // already oriented
            //            continue;

            //        var match = scanners[j].Matches(matchedPoints);
            //        if (match)
            //        {
            //            foreach (var item in scanners[j].Points.Where(it => !matchedPoints.Contains(it)))
            //                matchedPoints.Add(item);
            //        }
            //            //matchedPoints = matchedPoints.Concat(scanners[j].Points).Distinct().ToArray();
            //    }
            //}

            var unmoved = scanners.Skip(1).ToArray();
            while(unmoved.Any())
            {
                foreach(var item in unmoved)
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

            //var test = scanners.Count(it => it.Moved);
            Console.WriteLine($"total of {matchedPoints.Count()} points in matched");

            return scanners.SelectMany(it => it.Points).Distinct().Count();
            //return matchedPoints.Count();
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

            public Scanner(string input)
            {
                Points = Parser
                    .ToArrayOfString(input)
                    .Skip(1)
                    .Select(it => Parser.SplitOn(it, ',').Select(a => int.Parse(a)).ToArray())
                    .Select(it => new Voxel(it[0], it[1], it[2]))
                    .ToArray();
            }

            public void MoveTo(Voxel pos)
            {
                Points = Points.Select(it => it.Add(pos)).ToArray();
                Moved = true;
            }

            //public bool Matches(Scanner input)
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

                            //if(matching > 1)
                            //{
                            //    Console.WriteLine("Found more than one match");
                            //}

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
