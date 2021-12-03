using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day02
    {
        [Solution(2, 1)]
        public long Solution1(string input)
        {
            var finalPos = Parser.ToArrayOf(input, it => ToPoint(it))
                .Aggregate(new Point(), (acc, i) => new Point(acc.X + i.X, acc.Y + i.Y));

            return finalPos.X * finalPos.Y;
        }

        [Solution(2, 2)]
        public long Solution2(string input)
        {
            var finalPos = Parser.ToArrayOf(input, it => ToPoint(it))
                .Aggregate(new { pos = new Point(), aim = 0L }, (acc, i) => new { pos = GetNewPos(acc.pos, i, acc.aim), aim = acc.aim + i.Y });

            return finalPos.pos.X * finalPos.pos.Y;
        }

        private Point ToPoint(string input)
        {
            var items = Parser.SplitOnSpace(input);
            var dist = int.Parse(items[1]);
            if (items[0] == "forward")
                return new Point(dist, 0);
            else if (items[0] == "down")
                return new Point(0, dist);
            else
                return new Point(0, -dist);
        }

        private Point GetNewPos(Point old, Point next, long aim)
        {
            return new Point(
                old.X + next.X,
                old.Y + (aim * next.X));
        }
    }
}
