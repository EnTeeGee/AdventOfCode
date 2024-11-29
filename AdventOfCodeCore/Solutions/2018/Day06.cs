using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ','))
                .Select(it => new Point(int.Parse(it[0]), int.Parse(it[1])))
                .ToArray();


            var topLeft = new Point(points.Min(it => it.X), points.Min(it => it.Y));
            var bottomRight = new Point(points.Max(it => it.X), points.Max(it => it.Y));
            var dist = new Point(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

            var surrounding = Enumerable.Range((int)topLeft.X, (int)dist.X).Select(it => new Point(it, topLeft.Y))
                .Concat(Enumerable.Range((int)topLeft.X, (int)dist.X).Select(it => new Point(it, bottomRight.Y)))
                .Concat(Enumerable.Range((int)topLeft.Y, (int)dist.Y).Select(it => new Point(topLeft.X, it)))
                .Concat(Enumerable.Range((int)topLeft.Y, (int)dist.Y).Select(it => new Point(bottomRight.X, it)))
                .ToArray();

            var edgePoints = new List<Point>();

            foreach (var point in surrounding)
            {
                var nearest = points.OrderBy(it => point.GetTaxiCabDistanceTo(it)).First();
                if (!edgePoints.Contains(nearest))
                    edgePoints.Add(nearest);
            }

            var result = Enumerable.Range((int)topLeft.X + 1, (int)dist.X - 1)
                .SelectMany(it => Enumerable.Range((int)topLeft.Y + 1, (int)dist.Y - 1).Select(y => new Point(it, y)))
                .Select(it => GetNearest(it, points))
                .Where(it => it != null && !edgePoints.Contains(it.Value))
                .GroupBy(it => it)
                .Select(it => it.Count())
                .Max();

            return result;
        }

        [Solution(6, 2)]
        public int Solution2(string input)
        {
            var maxDist = 10000;
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ' ', ','))
                .Select(it => new Point(int.Parse(it[0]), int.Parse(it[1])))
                .ToArray();

            var topLeft = new Point(points.Min(it => it.X), points.Min(it => it.Y));
            var bottomRight = new Point(points.Max(it => it.X), points.Max(it => it.Y));
            var dist = new Point(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

            var allPoints = Enumerable.Range((int)topLeft.X + 1, (int)dist.X - 1)
                .SelectMany(it => Enumerable.Range((int)topLeft.Y + 1, (int)dist.Y - 1).Select(y => new { Point = new Point(it, y), Dist = 0 }));

            foreach (var item in points)
                allPoints = allPoints.Select(it => new { it.Point, Dist = it.Dist + (int)it.Point.GetTaxiCabDistanceTo(item) }).Where(it => it.Dist < maxDist);

            return allPoints.Count();
        }

        private Point? GetNearest(Point point, Point[] points)
        {
            var nearest = points.Select(it => new { it, dist = it.GetTaxiCabDistanceTo(point) }).OrderBy(it => it.dist).Take(2).ToArray();
            if (nearest[0].dist.Equals(nearest[1].dist))
                return null;

            return nearest[0].it;
        }
    }
}
