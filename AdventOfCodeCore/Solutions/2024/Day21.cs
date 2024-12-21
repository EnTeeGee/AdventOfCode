using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var digitGrid = new Dictionary<char, Point>
            {
                { 'A', Point.Origin },
                { '0', new Point(-1, 0) },
                { '1', new Point(-2, -1) },
                { '2', new Point(-1, -1) },
                { '3', new Point(0, -1) },
                { '4', new Point(-2, -2) },
                { '5', new Point(-1, -2) },
                { '6', new Point(0, -2) },
                { '7', new Point(-2, -3) },
                { '8', new Point(-1, -3) },
                { '9', new Point(0, -3) }
            };
            var arrowGrid = new Dictionary<char, Point>
            {
                { 'A', Point.Origin },
                { '^', new Point(-1, 0) },
                { '<', new Point(-2, 1) },
                { 'v', new Point(-1, 1) },
                { '>', new Point(0, 1) }
            };
            var invalid = new Point(-2, 0);

            var lines = Parser.ToArrayOfString(input);
            var output = 0;

            foreach (var line in lines)
            {
                var currentLine = line;
                var test = currentLine.Length;
                var currentPos = 'A';
                var updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(digitGrid[item].X, digitGrid[currentPos].Y);

                    var target = digitGrid[item] - digitGrid[currentPos];
                    //updatedLine += StepsToPush(target, currentPos != 'A' && currentPos != '0');
                    //updatedLine += StepsToPush(target, digitGrid.ContainsValue(testPos));
                    updatedLine += StepsToPushV2(digitGrid[currentPos], digitGrid[item]);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                    var target = arrowGrid[item] - arrowGrid[currentPos];
                    //updatedLine += StepsToPush(target, currentPos != 'A' && currentPos != '^');
                    //updatedLine += StepsToPushArrow(target, arrowGrid.ContainsValue(testPos));
                    updatedLine += StepsToPushV2(arrowGrid[currentPos], arrowGrid[item]);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                    var target = arrowGrid[item] - arrowGrid[currentPos];
                    //updatedLine += StepsToPush(target, currentPos != 'A' && currentPos != '^');
                    //updatedLine += StepsToPushArrow(target, arrowGrid.ContainsValue(testPos));
                    updatedLine += StepsToPushV2(arrowGrid[currentPos], arrowGrid[item]);
                    currentPos = item;
                }
                var test1 = int.Parse(line[..3]);
                var test2 = updatedLine.Length;

                output += (int.Parse(line[..3]) * updatedLine.Length);
            }

            return output;
        }

        [Solution(21, 2)]
        public long Solution2(string input)
        {
            var digitGrid = new Dictionary<char, Point>
            {
                { 'A', Point.Origin },
                { '0', new Point(-1, 0) },
                { '1', new Point(-2, -1) },
                { '2', new Point(-1, -1) },
                { '3', new Point(0, -1) },
                { '4', new Point(-2, -2) },
                { '5', new Point(-1, -2) },
                { '6', new Point(0, -2) },
                { '7', new Point(-2, -3) },
                { '8', new Point(-1, -3) },
                { '9', new Point(0, -3) }
            };
            var arrowGrid = new Dictionary<char, Point>
            {
                { 'A', Point.Origin },
                { '^', new Point(-1, 0) },
                { '<', new Point(-2, 1) },
                { 'v', new Point(-1, 1) },
                { '>', new Point(0, 1) }
            };
            var invalid = new Point(-2, 0);

            var lines = Parser.ToArrayOfString(input);
            var output = 0;

            foreach (var line in lines)
            {
                var currentLine = line;
                var test = currentLine.Length;
                var currentPos = 'A';
                var updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(digitGrid[item].X, digitGrid[currentPos].Y);

                    var target = digitGrid[item] - digitGrid[currentPos];
                    //updatedLine += StepsToPush(target, currentPos != 'A' && currentPos != '0');
                    //updatedLine += StepsToPush(target, digitGrid.ContainsValue(testPos));
                    updatedLine += StepsToPushV2(digitGrid[currentPos], digitGrid[item]);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;

                for(var i = 0; i < 25; i++)
                {
                    foreach (var item in currentLine)
                    {
                        var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                        var target = arrowGrid[item] - arrowGrid[currentPos];
                        //updatedLine += StepsToPush(target, currentPos != 'A' && currentPos != '^');
                        //updatedLine += StepsToPushArrow(target, arrowGrid.ContainsValue(testPos));
                        updatedLine += StepsToPushV2(arrowGrid[currentPos], arrowGrid[item]);
                        currentPos = item;
                    }

                    test = updatedLine.Length;
                    currentLine = updatedLine;
                    updatedLine = string.Empty;
                }

                output += (int.Parse(line[..3]) * currentLine.Length);
            }

            return output;
        }

        private string StepsToPush(Point pos, bool horiFirst)
        {
            var output = new List<char>();

            if (pos.X > 0)
                output.AddRange(Enumerable.Repeat('>', (int)pos.X));
            if (horiFirst && pos.X < 0)
                output.AddRange(Enumerable.Repeat('<', -(int)pos.X));
            if (pos.Y < 0)
                output.AddRange(Enumerable.Repeat('^', -(int)pos.Y));
            if (!horiFirst && pos.X < 0)
                output.AddRange(Enumerable.Repeat('<', -(int)pos.X));
            if (pos.Y > 0)
                output.AddRange(Enumerable.Repeat('v', (int)pos.Y));

            output.Add('A');

            return new string(output.ToArray());
        }

        private string StepsToPushArrow(Point pos, bool horiFirst)
        {
            var output = new List<char>();

            if (pos.Y > 0)
                output.AddRange(Enumerable.Repeat('v', (int)pos.Y));
            if (pos.X > 0)
                output.AddRange(Enumerable.Repeat('>', (int)pos.X));
            if (horiFirst && pos.X < 0)
                output.AddRange(Enumerable.Repeat('<', -(int)pos.X));
            if (pos.Y < 0)
                output.AddRange(Enumerable.Repeat('^', -(int)pos.Y));
            if (!horiFirst && pos.X < 0)
                output.AddRange(Enumerable.Repeat('<', -(int)pos.X));

            output.Add('A');

            return new string(output.ToArray());
        }

        private string StepsToPushV2(Point start, Point dest)
        {
            // same key
            if (start == dest)
                return "A";

            var invalid = new Point(-2, 0);
            var output = new List<char>();
            var diff = dest - start;

            // straight paths
            if (diff.X == 0 || diff.Y == 0)
            {
                if (diff.Y > 0)
                    output.AddRange(Enumerable.Repeat('v', (int)diff.Y));
                if (diff.X > 0)
                    output.AddRange(Enumerable.Repeat('>', (int)diff.X));
                if (diff.X < 0)
                    output.AddRange(Enumerable.Repeat('<', -(int)diff.X));
                if (diff.Y < 0)
                    output.AddRange(Enumerable.Repeat('^', -(int)diff.Y));

                output.Add('A');

                return new string(output.ToArray());
            }

            // top left
            if(diff.X < 0 && diff.Y < 0)
            {
                output.AddRange(Enumerable.Repeat('<', -(int)diff.X));
                output.AddRange(Enumerable.Repeat('^', -(int)diff.Y));
                if(ToPointList(start, output).Any(it => it == invalid))
                {
                    output.Clear();
                    output.AddRange(Enumerable.Repeat('^', -(int)diff.Y));
                    output.AddRange(Enumerable.Repeat('<', -(int)diff.X));
                }

                output.Add('A');

                return new string(output.ToArray());
            }

            // top right
            if (diff.X > 0 && diff.Y < 0)
            {
                output.AddRange(Enumerable.Repeat('^', -(int)diff.Y));
                output.AddRange(Enumerable.Repeat('>', (int)diff.X));
                if (ToPointList(start, output).Any(it => it == invalid))
                {
                    output.Clear();
                    output.AddRange(Enumerable.Repeat('>', (int)diff.X));
                    output.AddRange(Enumerable.Repeat('^', -(int)diff.Y));
                }

                output.Add('A');

                return new string(output.ToArray());
            }

            // bottom left
            if (diff.X < 0 && diff.Y > 0)
            {
                output.AddRange(Enumerable.Repeat('<', -(int)diff.X));
                output.AddRange(Enumerable.Repeat('v', (int)diff.Y));
                if (ToPointList(start, output).Any(it => it == invalid))
                {
                    output.Clear();
                    output.AddRange(Enumerable.Repeat('v', (int)diff.Y));
                    output.AddRange(Enumerable.Repeat('<', -(int)diff.X));
                }

                output.Add('A');

                return new string(output.ToArray());
            }

            // bottom right
            if (diff.X > 0 && diff.Y > 0)
            {
                output.AddRange(Enumerable.Repeat('v', (int)diff.Y));
                output.AddRange(Enumerable.Repeat('>', (int)diff.X));
                if (ToPointList(start, output).Any(it => it == invalid))
                {
                    output.Clear();
                    output.AddRange(Enumerable.Repeat('>', (int)diff.X));
                    output.AddRange(Enumerable.Repeat('v', (int)diff.Y));
                }

                output.Add('A');

                return new string(output.ToArray());
            }

            throw new Exception("invalid direction");
        }

        private List<Point> ToPointList(Point start, List<char> path)
        {
            var output = new List<Point>() { start };
            var current = start;
            var dirMap = new Dictionary<char, Orientation>
            {
                { '^', Orientation.North },
                { '<', Orientation.West },
                { 'v', Orientation.South },
                { '>' , Orientation.East }
            };

            foreach (var item in path.Where(it => it != 'A'))
            {
                current = current.MoveOrient(dirMap[item]);
                output.Add(current);
            }

            return output;
        }
    }
}
