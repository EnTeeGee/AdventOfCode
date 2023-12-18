using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day18
    {
        [Solution(18, 1)]
        public long Solution1(string input)
        {
            var moves = Parser.ToArrayOf(input, it => new Dig(it));

            return Compute(moves);
        }

        [Solution(18, 2)]
        public long Solution2(string input)
        {
            var moves = Parser.ToArrayOf(input, it => new Dig(it, true));

            return Compute(moves);
        }

        private long Compute(Dig[] path)
        {
            var asPoints = new List<Point>();
            var current = Point.Origin;
            foreach(var item in path)
            {
                current = current.MoveOrient(item.Dir, item.Dist);
                asPoints.Add(current);
            }
            var area = asPoints.Zip(asPoints.Skip(1).Concat(new[] { asPoints[0] }), (a, b) => (a.X * b.Y) - (a.Y * b.X)).Sum() / 2;

            return area + (path.Sum(it => it.Dist) / 2) + 1;
        }

        private class Dig
        {
            public Orientation Dir { get; }
            public int Dist { get; }

            public Dig(string input, bool isExpanded = false)
            {
                var chunks = Parser.SplitOnSpace(input);
                if (!isExpanded)
                {
                    Dir = chunks[0] == "U" ? Orientation.North
                        : chunks[0] == "R" ? Orientation.East
                        : chunks[0] == "D" ? Orientation.South
                        : Orientation.West;
                    Dist = int.Parse(chunks[1]);
                }
                else
                {
                    Dir = ((Orientation)int.Parse(chunks[2][7].ToString())).RotateClockwise();
                    Dist = Convert.ToInt32(chunks[2].Substring(2, 5), 16);
                }
            }
        }
    }
}
