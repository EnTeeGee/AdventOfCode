using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day21
    {
        private readonly Dictionary<char, Point> arrowGrid = new Dictionary<char, Point>
            {
                { 'A', Point.Origin },
                { '^', new Point(-1, 0) },
                { '<', new Point(-2, 1) },
                { 'v', new Point(-1, 1) },
                { '>', new Point(0, 1) }
            };

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

            var lines = Parser.ToArrayOfString(input);
            var output = 0;
            var seen = new Dictionary<(Point, Point), string>();

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
                    updatedLine += StepsToPushV2(digitGrid[currentPos], digitGrid[item], seen);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                    var target = arrowGrid[item] - arrowGrid[currentPos];
                    updatedLine += StepsToPushV2(arrowGrid[currentPos], arrowGrid[item], seen);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;

                foreach (var item in currentLine)
                {
                    var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                    var target = arrowGrid[item] - arrowGrid[currentPos];
                    updatedLine += StepsToPushV2(arrowGrid[currentPos], arrowGrid[item], seen);
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

            var lines = Parser.ToArrayOfString(input);
            var output = 0L;

            foreach (var line in lines)
            {
                var currentLine = line;
                var test = currentLine.Length;
                var currentPos = 'A';
                var updatedLine = string.Empty;
                var seen = new Dictionary<(Point, Point), string>();

                foreach (var item in currentLine)
                {
                    var testPos = new Point(digitGrid[item].X, digitGrid[currentPos].Y);

                    var target = digitGrid[item] - digitGrid[currentPos];
                    updatedLine += StepsToPushV2(digitGrid[currentPos], digitGrid[item], seen);
                    currentPos = item;
                }

                test = updatedLine.Length;
                currentLine = updatedLine;
                updatedLine = string.Empty;
                var lineOutput = 0L;
                var seenRecurse = new Dictionary<(Point, Point, int), long>();
                Console.WriteLine($"Completed initial step, length is: {test}");

                foreach (var item in currentLine)
                {
                    var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                    var target = arrowGrid[item] - arrowGrid[currentPos];
                    lineOutput += StepsToPushRecurse(arrowGrid[currentPos], arrowGrid[item], 0, seenRecurse, seen);
                    currentPos = item;
                }

                output += (int.Parse(line[..3]) * lineOutput);
            }

            return output;
        }

        private string StepsToPushV2(Point start, Point dest, Dictionary<(Point start, Point end), string> seen)
        {
            if (seen.ContainsKey((start, dest)))
                return seen[(start, dest)];

            // same key
            if (start == dest)
            {
                seen.Add((start, dest), "A");
                return "A";
            }
                

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
                seen.Add((start, dest), new string(output.ToArray()));

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
            }

            output.Add('A');
            seen.Add((start, dest), new string(output.ToArray()));

            return new string(output.ToArray());

            throw new Exception("invalid direction");
        }


        private long StepsToPushRecurse(Point start, Point end, int depth, Dictionary<(Point, Point, int), long> seen, Dictionary<(Point, Point), string> seenStrings)
        {
            if (seen.ContainsKey((start, end, depth)))
                return seen[(start, end, depth)];

            var stage = StepsToPushV2(start, end, seenStrings);
            if(depth == 24)
            {
                seen.Add((start, end, depth), stage.Length);

                return stage.Length;
            }

            var output = 0L;
            var currentPos = 'A';

            foreach (var item in stage)
            {
                var testPos = new Point(arrowGrid[item].X, arrowGrid[currentPos].Y);

                var target = arrowGrid[item] - arrowGrid[currentPos];
                output += StepsToPushRecurse(arrowGrid[currentPos], arrowGrid[item], depth + 1, seen, seenStrings);
                currentPos = item;
            }

            seen.Add((start, end, depth), output);

            return output;
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
