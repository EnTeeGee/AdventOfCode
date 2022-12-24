using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day24
    {
        [Solution(24, 1)]
        public int Solution1(string input)
        {
            var (blizzards, limit) = ParseInput(input);
            var targets = new[] { new Point(limit.X, limit.Y + 1) };

            return RunForTargets(blizzards, limit, targets);
        }

        [Solution(24, 2)]
        public int Solution2(string input)
        {
            var (blizzards, limit) = ParseInput(input);
            var targets = new[]
            {
                new Point(limit.X, limit.Y + 1),
                new Point(0, -1),
                new Point(limit.X, limit.Y + 1)
            };

            return RunForTargets(blizzards, limit, targets);
            
        }

        private (List<Blizzard> blizzards, Point limit) ParseInput(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var blizzards = new List<Blizzard>();
            for (var i = 1; i < lines.Length - 1; i++)
            {
                var line = lines[i];
                for (var j = 1; j < line.Length - 1; j++)
                {
                    var tile = line[j];
                    var point = new Point(j - 1, i - 1);
                    if (tile == '^')
                        //blizzards.Add(new Blizzard(point, Orientation.North));
                        blizzards.Add(new Blizzard(point, Orientation.South));
                    else if (tile == '>')
                        blizzards.Add(new Blizzard(point, Orientation.East));
                    else if (tile == 'v')
                        //blizzards.Add(new Blizzard(point, Orientation.South));
                        blizzards.Add(new Blizzard(point, Orientation.North));
                    else if (tile == '<')
                        blizzards.Add(new Blizzard(point, Orientation.West));
                }
            }

            var limit = new Point(lines[0].Length - 3, lines.Length - 3);

            return (blizzards, limit);
        }

        private int RunForTargets(List<Blizzard> blizzards, Point limit, Point[] targets)
        {
            var round = 1;
            var positions = new List<Point> { new Point(0, 1) };

            while (true)
            {
                var newPositions = new List<Point>();
                foreach (var item in blizzards)
                    item.Advance(limit);
                var hash = blizzards.Select(it => it.Pos).ToHashSet();

                foreach (var item in positions)
                {
                    var options = item.GetSurrounding4();
                    if (options.Any(it => it.Equals(targets[0])))
                    {
                        newPositions.Clear();
                        newPositions.Add(targets[0]);
                        targets = targets.Skip(1).ToArray();

                        if (targets.Length == 0)
                            return round;

                        break;
                    }


                    options = options.Concat(new[] { item })
                        .Where(it => it.WithinBounds(0, limit.X, 0, limit.Y) || it.Equals(item))
                        .Where(it => !hash.Contains(it))
                        .ToArray();
                    foreach (var newItem in options)
                        newPositions.Add(newItem);
                }

                positions = newPositions.Distinct().ToList();
                round += 1;
            }
        }

        private class Blizzard
        {
            public Point Pos { get; private set; }
            public Orientation Direction { get; }

            public Blizzard(Point pos, Orientation direction)
            {
                Pos = pos;
                Direction = direction;
            }

            public void Advance(Point limit)
            {
                Pos = Pos.MoveOrient(Direction);
                if (Pos.X < 0)
                    Pos = new Point(limit.X, Pos.Y);
                else if (Pos.X > limit.X)
                    Pos = new Point(0, Pos.Y);
                else if (Pos.Y < 0)
                    Pos = new Point(Pos.X, limit.Y);
                else if (Pos.Y > limit.Y)
                    Pos = new Point(Pos.X, 0);
            }
        }
    }
}
