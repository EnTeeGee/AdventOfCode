using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day22
    {
        //192034 too high
        [Solution(22, 1)]
        public long Solution1(string input)
        {
            var chunks = Parser.ToArrayOfGroupsUntrimmed(input);
            var mapLines = Parser.ToArrayOfStringUntrimmed(chunks[0]);
            var floors = new HashSet<Point>();
            var walls = new HashSet<Point>();
            var start = Point.Origin;
            var orient = Orientation.East;

            for(var i = 0; -i < mapLines.Length; i--)
            {
                var line = mapLines[-i];
                for(var j = 0; j < line.Length; j++)
                {
                    var currentChar = line[j];
                    if (currentChar == ' ')
                        continue;

                    if (currentChar == '#')
                        walls.Add(new Point(j, i));

                    if (start.Equals(Point.Origin))
                        start = new Point(j, i);
                    floors.Add(new Point(j, i));
                }
            }

            var directions = chunks[1].Trim();
            var current = start;

            while (directions.Any())
            {
                if (directions[0] == 'L')
                {
                    orient = orient.RotateAntiClock();
                    directions = directions.Substring(1);
                }
                else if (directions[0] == 'R')
                {
                    orient = orient.RotateClockwise();
                    directions = directions.Substring(1);
                }
                else
                {
                    var trim = directions.IndexOfAny(new[] { 'L', 'R' });
                    var dist = int.Parse(trim > 0 ? directions.Substring(0, trim) : directions);
                    directions = trim > 0 ? directions.Substring(trim) : string.Empty;

                    for(var i = 0; i < dist; i++)
                    {
                        var next = current.MoveOrient(orient, 1);

                        if (!floors.Contains(next))
                        {
                            var checkPoint = current;
                            var checkOrient = orient.RotateClockwise().RotateClockwise();
                            while (floors.Contains(checkPoint))
                                checkPoint = checkPoint.MoveOrient(checkOrient);
                            next = checkPoint.MoveOrient(orient);
                        }

                        if (walls.Contains(next))
                            break;

                        current = next;
                    }
                }
            }

            var xIndex = current.X + 1;
            var yIndex = (-current.Y) + 1;
            var orientValue = orient == Orientation.East ? 0 : orient == Orientation.South ? 1 : orient == Orientation.West ? 2 : 3;

            return (yIndex * 1000) + (xIndex * 4) + orientValue;
        }

        [Solution(22, 2)]
        public long Solution2(string input)
        {
            var chunks = Parser.ToArrayOfGroupsUntrimmed(input);
            var mapLines = Parser.ToArrayOfStringUntrimmed(chunks[0]);
            var floors = new HashSet<Point>();
            var walls = new HashSet<Point>();
            var start = Point.Origin;
            var orient = Orientation.East;

            for (var i = 0; -i < mapLines.Length; i--)
            {
                var line = mapLines[-i];
                for (var j = 0; j < line.Length; j++)
                {
                    var currentChar = line[j];
                    if (currentChar == ' ')
                        continue;

                    if (currentChar == '#')
                        walls.Add(new Point(j, i));

                    if (start.Equals(Point.Origin))
                        start = new Point(j, i);
                    floors.Add(new Point(j, i));
                }
            }

            var directions = chunks[1].Trim();
            var current = start;
            var mappings = GetMapping(50);

            while (directions.Any())
            {
                if (directions[0] == 'L')
                {
                    orient = orient.RotateAntiClock();
                    directions = directions.Substring(1);
                }
                else if (directions[0] == 'R')
                {
                    orient = orient.RotateClockwise();
                    directions = directions.Substring(1);
                }
                else
                {
                    var trim = directions.IndexOfAny(new[] { 'L', 'R' });
                    var dist = int.Parse(trim > 0 ? directions.Substring(0, trim) : directions);
                    directions = trim > 0 ? directions.Substring(trim) : string.Empty;

                    for (var i = 0; i < dist; i++)
                    {
                        var next = current.MoveOrient(orient, 1);
                        var nextOrient = orient;

                        if (!floors.Contains(next))
                        {
                            var target = mappings[new Mapping(next, orient)];
                            next = target.Point;
                            nextOrient = target.Orient;
                        }

                        if (walls.Contains(next))
                            break;

                        current = next;
                        orient = nextOrient;
                    }
                }
            }

            var xIndex = current.X + 1;
            var yIndex = (-current.Y) + 1;
            var orientValue = orient == Orientation.East ? 0 : orient == Orientation.South ? 1 : orient == Orientation.West ? 2 : 3;

            return (yIndex * 1000) + (xIndex * 4) + orientValue;
        }

        private Dictionary<Mapping, Mapping> GetMapping(int size)
        {
            /* Example map, size 4
             *       +--+
             *       |  |
             *       |  |
             * +--+--+--+
             * |  |  |  |
             * |  |  |  |
             * +--+--+--+--+
             *       |  |  |
             *       |  |  |
             *       +--+--+
             *       
             * input map, size 50
             *      A  B
             *     +--+--+
             *     |  |  |C
             *    N|  |  |
             *     +--+--+
             *    M|  | D
             *   L |  |E
             *  +--+--+
             *  |  |  |F
             * K|  |  |
             *  +--+--+
             *  |  | G
             * J|  |H
             *  +--+
             *   I
             */

            var output = new Dictionary<Mapping, Mapping>();

            for(var i = 0; i < size; i++)
            {
                if(size == 50)
                {
                    // A - J
                    output.Add(new Mapping(new Point(50 + i, 1), Orientation.North), new Mapping(new Point(0, 150 + i), Orientation.East));
                    output.Add(new Mapping(new Point(-1, 150 + i), Orientation.West), new Mapping(new Point(50 + i, 0), Orientation.South));
                    // B - I
                    output.Add(new Mapping(new Point(100 + i, 1), Orientation.North), new Mapping(new Point(i, 199), Orientation.North));
                    output.Add(new Mapping(new Point(i, 200), Orientation.South), new Mapping(new Point(100 + i, 0), Orientation.South));
                    // C - F
                    output.Add(new Mapping(new Point(150, i), Orientation.East), new Mapping(new Point(99, 149 - i), Orientation.West));
                    output.Add(new Mapping(new Point(100, 100 + i), Orientation.East), new Mapping(new Point(149, 49 - i), Orientation.West));
                    // D - E
                    output.Add(new Mapping(new Point(100 + i, 50), Orientation.South), new Mapping(new Point(99, 50 + i), Orientation.West));
                    output.Add(new Mapping(new Point(100, 50 + i), Orientation.East), new Mapping(new Point(100 + i, 49), Orientation.North));
                    // G - H
                    output.Add(new Mapping(new Point(50 + i, 150), Orientation.South), new Mapping(new Point(49, 150 + i), Orientation.West));
                    output.Add(new Mapping(new Point(50, 150 + i), Orientation.East), new Mapping(new Point(50 + i, 149), Orientation.North));
                    // K - N
                    output.Add(new Mapping(new Point(-1, 100 + i), Orientation.West), new Mapping(new Point(50, 49 - i), Orientation.East));
                    output.Add(new Mapping(new Point(49, i), Orientation.West), new Mapping(new Point(0, 149 - i), Orientation.East));
                    // L - M
                    output.Add(new Mapping(new Point(i, 99), Orientation.North), new Mapping(new Point(50, 50 + i), Orientation.East));
                    output.Add(new Mapping(new Point(49, 50 + i), Orientation.East), new Mapping(new Point(i, 100), Orientation.South));
                }
                else
                {
                    throw new Exception("Size/shape not handled");
                }
            }

            return output;
        }

        private class Mapping
        {
            public Point Point { get; }
            public Orientation Orient { get; }

            public Mapping(Point point, Orientation orient)
            {
                Point = point;
                Orient = orient;
            }

            public override bool Equals(object obj)
            {
                var cast = obj as Mapping;
                if (cast == null)
                    return false;

                return cast.Point.Equals(Point) && cast.Orient == Orient;
            }

            public override int GetHashCode()
            {
                return Point.GetHashCode() ^ Orient.GetHashCode();
            }
        }
    }
}
