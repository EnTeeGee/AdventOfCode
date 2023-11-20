using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day18
    {
        [Solution(18, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var activeLights = SetUpInitial(lines);
            var width = lines[0].Length;
            var height = lines.Length;

            for (var step = 0; step < 100; step++)
            {
                var newGrid = new HashSet<Point>();

                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        var point = new Point(j, i);
                        var isActive = activeLights.Contains(point);
                        var activeSurrounding = point.GetSurrounding8().Where(it => activeLights.Contains(it)).Count();

                        if (isActive && (activeSurrounding == 2 | activeSurrounding == 3))
                            newGrid.Add(point);
                        else if (!isActive && activeSurrounding == 3)
                            newGrid.Add(point);
                    }
                }

                activeLights = newGrid;
            }

            return activeLights.Count();
        }

        [Solution(18, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var activeLights = SetUpInitial(lines);
            var width = lines[0].Length;
            var height = lines.Length;

            var corners = new[]
            {
                new Point(0, 0),
                new Point(width - 1, 0),
                new Point(width - 1, height - 1),
                new Point(0, height - 1)
            };

            foreach (var item in corners)
            {
                if (!activeLights.Contains(item))
                    activeLights.Add(item);
            }

            for (var step = 0; step < 100; step++)
            {
                var newGrid = new HashSet<Point>();

                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        var point = new Point(j, i);
                        var isActive = activeLights.Contains(point);
                        var activeSurrounding = point.GetSurrounding8().Where(it => activeLights.Contains(it)).Count();

                        if (isActive && (activeSurrounding == 2 | activeSurrounding == 3))
                            newGrid.Add(point);
                        else if (!isActive && activeSurrounding == 3)
                            newGrid.Add(point);
                    }
                }

                activeLights = newGrid;

                foreach (var item in corners)
                {
                    if (!activeLights.Contains(item))
                        activeLights.Add(item);
                }
            }

            return activeLights.Count();
        }

        private HashSet<Point> SetUpInitial(string[] input)
        {
            var output = new HashSet<Point>();

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                        output.Add(new Point(j, i));
                }
            }

            return output;
        }
    }
}
