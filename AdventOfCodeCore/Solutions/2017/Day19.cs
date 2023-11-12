using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day19
    {
        [Solution(19, 1)]
        public string Solution1(string input)
        {
            var lines = Parser.ToArrayOfStringUntrimmed(input);
            var elements = new Dictionary<Point, char>();

            for(var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for(var j = 0; j < line.Length; j++)
                {
                    if (line[j] == ' ')
                        continue;

                    elements.Add(new Point(j, i), line[j]);
                }
            }

            var seen = new List<char>();
            var pos = new Point(lines[0].IndexOf('|'), 0);
            var dir = Orientation.South;

            while (true)
            {
                var target = pos.MoveOrient(dir);
                if (!elements.ContainsKey(target))
                {
                    var newDir = new Orientation?[] { dir.RotateClockwise(), dir.RotateAntiClock() }
                        .FirstOrDefault(it => it != null && elements.ContainsKey(pos.MoveOrient(it.Value)));
                    if (newDir == null)
                        return string.Join(string.Empty, seen);
                    dir = newDir.Value;

                    continue;
                }

                if (char.IsLetter(elements[target]))
                    seen.Add(elements[target]);

                pos = target;
            }
        }

        [Solution(19, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfStringUntrimmed(input);
            var elements = new Dictionary<Point, char>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == ' ')
                        continue;

                    elements.Add(new Point(j, i), line[j]);
                }
            }

            var count = 1;
            var pos = new Point(lines[0].IndexOf('|'), 0);
            var dir = Orientation.South;

            while (true)
            {
                var target = pos.MoveOrient(dir);
                if (!elements.ContainsKey(target))
                {
                    var newDir = new Orientation?[] { dir.RotateClockwise(), dir.RotateAntiClock() }
                        .FirstOrDefault(it => it != null && elements.ContainsKey(pos.MoveOrient(it.Value)));
                    if (newDir == null)
                        return count;
                    dir = newDir.Value;

                    continue;
                }

                pos = target;
                count++;
            }
        }
    }
}
