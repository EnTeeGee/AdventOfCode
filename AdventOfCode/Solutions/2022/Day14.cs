using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day14
    {
        [Solution(14, 1)]
        public int Solution1(string input)
        {
            var wallPoints = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ' ', '-', '>')
                    .Select(p => Parser.SplitOn(p, ','))
                    .Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
                    .ToArray())
                .SelectMany(it => it.Zip(it.Skip(1), (first, second) => new { first, second }))
                .ToArray();

            var walls = new HashSet<Point>();
            var bottom = 0L;
            foreach(var item in wallPoints)
            {
                Point[] toAdd = null;
                if (item.first.X != item.second.X)
                    toAdd = Enumerable.Range((int)Math.Min(item.first.X, item.second.X), (int)Math.Abs(item.first.X - item.second.X) + 1)
                        .Select(it => new Point(it, item.first.Y))
                        .ToArray();
                else
                    toAdd = Enumerable.Range((int)Math.Min(item.first.Y, item.second.Y), (int)Math.Abs(item.first.Y - item.second.Y) + 1)
                        .Select(it => new Point(item.first.X, it))
                        .ToArray();

                foreach (var point in toAdd)
                    walls.Add(point);

                bottom = Math.Max(Math.Max(item.first.Y, item.second.Y), bottom);
            }

            var fallenSand = new HashSet<Point>();

            while (true)
            {
                var sand = new Point(500, 1);

                while (true)
                {
                    if (sand.Y > bottom)
                        return fallenSand.Count();

                    sand = sand.MoveNorth();
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand))
                        continue;

                    sand = sand.MoveEast(-1);
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand))
                        continue;

                    sand = sand.MoveEast(2);
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand))
                        continue;

                    sand = sand.MoveNorth(-1).MoveEast(-1);
                    fallenSand.Add(sand);
                    break;
                }
            }
        }

        [Solution(14, 2)]
        public int Solution2(string input)
        {
            var wallPoints = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOn(it, ' ', '-', '>')
                    .Select(p => Parser.SplitOn(p, ','))
                    .Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
                    .ToArray())
                .SelectMany(it => it.Zip(it.Skip(1), (first, second) => new { first, second }))
                .ToArray();

            var walls = new HashSet<Point>();
            var bottom = 0L;
            foreach (var item in wallPoints)
            {
                Point[] toAdd = null;
                if (item.first.X != item.second.X)
                    toAdd = Enumerable.Range((int)Math.Min(item.first.X, item.second.X), (int)Math.Abs(item.first.X - item.second.X) + 1)
                        .Select(it => new Point(it, item.first.Y))
                        .ToArray();
                else
                    toAdd = Enumerable.Range((int)Math.Min(item.first.Y, item.second.Y), (int)Math.Abs(item.first.Y - item.second.Y) + 1)
                        .Select(it => new Point(item.first.X, it))
                        .ToArray();

                foreach (var point in toAdd)
                    walls.Add(point);

                bottom = Math.Max(Math.Max(item.first.Y, item.second.Y), bottom);
            }

            var fallenSand = new HashSet<Point>();
            bottom += 2;

            while (true)
            {
                var sand = new Point(500, 0);

                while (true)
                {
                    sand = sand.MoveNorth();
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand) && sand.Y < bottom)
                        continue;

                    sand = sand.MoveEast(-1);
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand) && sand.Y < bottom)
                        continue;

                    sand = sand.MoveEast(2);
                    if (!walls.Contains(sand) && !fallenSand.Contains(sand) && sand.Y < bottom)
                        continue;

                    sand = sand.MoveNorth(-1).MoveEast(-1);
                    fallenSand.Add(sand);

                    if (sand.Y == 0)
                        return fallenSand.Count;

                    break;
                }
            }
        }
    }
}
