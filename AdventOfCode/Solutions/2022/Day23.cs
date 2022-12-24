using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2022
{
    class Day23
    {
        [Solution(23, 1)]
        public long Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var elves = new List<Elf>();
            for(var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for(var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                        elves.Add(new Elf(new Point(j, i)));
                }
            }

            var checkList = new List<Func<Point, HashSet<Point>, Point?>>
            {
                CheckNorth,
                CheckSouth,
                CheckWest,
                CheckEast
            };

            for(var i = 0; i < 10; i++)
            {
                var currentElves = elves.Select(it => it.Location).ToHashSet();

                foreach(var item in elves)
                {
                    var anySurrounding = item.Location.GetSurrounding8().Any(it => currentElves.Contains(it));
                    if(!anySurrounding)
                    {
                        item.PotentialMove = item.Location;
                        continue;
                    }

                    foreach(var moveCheck in checkList)
                    {
                        var result = moveCheck.Invoke(item.Location, currentElves);
                        if(result != null)
                        {
                            item.PotentialMove = result.Value;
                            break;
                        }
                    }
                }

                var toAbort = new List<Elf>();
                for(var j = 0; j < elves.Count; j++)
                {
                    var elf = elves[j];
                    for(var k = (j + 1); k < elves.Count; k++)
                    {
                        var toCompare = elves[k];
                        if (elf.PotentialMove.Equals(toCompare.PotentialMove))
                        {
                            toAbort.Add(elf);
                            toAbort.Add(toCompare);
                        }
                    }
                }

                foreach (var item in toAbort)
                    item.PotentialMove = item.Location;

                foreach (var item in elves)
                    item.Location = item.PotentialMove;

                var toSwap = checkList[0];
                checkList.RemoveAt(0);
                checkList.Add(toSwap);
            }

            var upperLeft = new Point(elves.Min(it => it.Location.X), elves.Min(it => it.Location.Y));
            var lowerRight = new Point(elves.Max(it => it.Location.X), elves.Max(it => it.Location.Y));

            return ((lowerRight.X - upperLeft.X + 1) * (lowerRight.Y - upperLeft.Y + 1) - elves.Count);
        }

        [Solution(23, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var elves = new List<Elf>();
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                        elves.Add(new Elf(new Point(j, i)));
                }
            }

            var checkList = new List<Func<Point, HashSet<Point>, Point?>>
            {
                CheckNorth,
                CheckSouth,
                CheckWest,
                CheckEast
            };

            var round = 1;
            while (true)
            {
                var currentElves = elves.Select(it => it.Location).ToHashSet();

                foreach (var item in elves)
                {
                    var anySurrounding = item.Location.GetSurrounding8().Any(it => currentElves.Contains(it));
                    if (!anySurrounding)
                    {
                        item.PotentialMove = item.Location;
                        continue;
                    }

                    foreach (var moveCheck in checkList)
                    {
                        var result = moveCheck.Invoke(item.Location, currentElves);
                        if (result != null)
                        {
                            item.PotentialMove = result.Value;
                            break;
                        }
                    }
                }

                var toAbort = new List<Elf>();
                var seenPoints = new Dictionary<Point, Elf>();
                for (var i = 0; i < elves.Count; i++)
                {
                    var elf = elves[i];
                    if (elf.Location.Equals(elf.PotentialMove))
                        continue;

                    if (seenPoints.ContainsKey(elf.PotentialMove))
                    {
                        seenPoints[elf.PotentialMove].PotentialMove = seenPoints[elf.PotentialMove].Location;
                        elf.PotentialMove = elf.Location;
                    }
                    else
                        seenPoints.Add(elf.PotentialMove, elf);
                }

                foreach (var item in toAbort)
                    item.PotentialMove = item.Location;

                var hasHadMove = false;
                foreach (var item in elves)
                {
                    if (!item.Location.Equals(item.PotentialMove))
                        hasHadMove = true;

                    item.Location = item.PotentialMove;
                }

                if (!hasHadMove && !toAbort.Any())
                {
                    //Draw(elves.Select(it => it.Location).ToHashSet());

                    return round;
                }

                round += 1;

                var toSwap = checkList[0];
                checkList.RemoveAt(0);
                checkList.Add(toSwap);
            }
        }

        private Point? CheckNorth(Point elf, HashSet<Point> all)
        {
            if (!all.Contains(new Point(elf.X - 1, elf.Y - 1))
                && !all.Contains(new Point(elf.X, elf.Y - 1))
                && !all.Contains(new Point(elf.X + 1, elf.Y - 1)))
                return new Point(elf.X, elf.Y - 1);

            return null;
        }

        private Point? CheckSouth(Point elf, HashSet<Point> all)
        {
            if (!all.Contains(new Point(elf.X - 1, elf.Y + 1))
                && !all.Contains(new Point(elf.X, elf.Y + 1))
                && !all.Contains(new Point(elf.X + 1, elf.Y + 1)))
                return new Point(elf.X, elf.Y + 1);

            return null;
        }

        private Point? CheckWest(Point elf, HashSet<Point> all)
        {
            if (!all.Contains(new Point(elf.X - 1, elf.Y - 1))
                && !all.Contains(new Point(elf.X - 1, elf.Y))
                && !all.Contains(new Point(elf.X - 1, elf.Y + 1)))
                return new Point(elf.X - 1, elf.Y);

            return null;
        }

        private Point? CheckEast(Point elf, HashSet<Point> all)
        {
            if (!all.Contains(new Point(elf.X + 1, elf.Y - 1))
                && !all.Contains(new Point(elf.X + 1, elf.Y))
                && !all.Contains(new Point(elf.X + 1, elf.Y + 1)))
                return new Point(elf.X + 1, elf.Y);

            return null;
        }

        private void Draw(HashSet<Point> points)
        {
            var upperLeft = new Point(points.Min(it => it.X), points.Min(it => it.Y));
            var lowerRight = new Point(points.Max(it => it.X), points.Max(it => it.Y));

            for(var y = upperLeft.Y; y <= lowerRight.Y; y++)
            {
                var builder = new StringBuilder();
                for(var x = upperLeft.X; x <= lowerRight.X; x++)
                {
                    var point = new Point(x, y);
                    if (points.Contains(point))
                        builder.Append("#");
                    else
                        builder.Append(".");
                }
                Console.WriteLine(builder.ToString());
            }
        }

        private class Elf
        {
            public Point Location { get; set; }
            public Point PotentialMove { get; set; }

            public Elf(Point location)
            {
                Location = location;
                PotentialMove = location;
            }
        }
    }
}
