using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            var numbers = new Dictionary<Point, char>();
            var symbols = new HashSet<Point>();
            var lines = Parser.ToArrayOfString(input);

            for(var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for(var x = 0; x < line.Length; x++)
                {
                    if (char.IsDigit(line[x]))
                        numbers.Add(new Point(x, y), line[x]);
                    else if (line[x] != '.')
                        symbols.Add(new Point(x, y));
                }
            }

            var sum = 0;

            while (numbers.Any())
            {
                var points = GetNumberPoints(numbers, numbers.First().Key, null);
                var number = points.OrderByDescending(it => it.X).Select((it, i) => int.Parse(numbers[it].ToString()) * Math.Pow(10, i)).Select(it => (int)it).Sum();
                foreach (var item in points)
                    numbers.Remove(item);

                if (points.SelectMany(it => it.GetSurrounding8()).Distinct().Any(it => symbols.Contains(it)))
                    sum += number;
            }

            return sum;
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var numbers = new Dictionary<Point, char>();
            var gears = new HashSet<Point>();
            var lines = Parser.ToArrayOfString(input);

            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (char.IsDigit(line[x]))
                        numbers.Add(new Point(x, y), line[x]);
                    else if (line[x] == '*')
                        gears.Add(new Point(x, y));
                }
            }

            var sum = 0;
            var matched = new Dictionary<Point, int>();

            while (numbers.Any())
            {
                var points = GetNumberPoints(numbers, numbers.First().Key, null);
                var number = points.OrderByDescending(it => it.X).Select((it, i) => int.Parse(numbers[it].ToString()) * Math.Pow(10, i)).Select(it => (int)it).Sum();
                foreach (var item in points)
                    numbers.Remove(item);

                var gear = points.SelectMany(it => it.GetSurrounding8()).Distinct().Cast<Point?>().FirstOrDefault(it => gears.Contains(it!.Value));
                if (gear == null)
                    continue;

                if (matched.ContainsKey(gear.Value))
                    sum += (matched[gear.Value] * number);
                else
                    matched.Add(gear.Value, number);
            }

            return sum;
        }

        private Point[] GetNumberPoints(Dictionary<Point, char> points, Point source, Point? toIgnore)
        {
            var matching = new[] { source.MoveEast(-1), source.MoveEast() }
                .Where(it => (toIgnore == null || !it.Equals(toIgnore.Value)) && points.ContainsKey(it)).ToArray();

            return matching.SelectMany(it => GetNumberPoints(points, it, source)).Concat(new[] { source }).ToArray();
        }
    }
}
