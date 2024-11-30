using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day25
    {
        [Solution(25, 1)]
        public int Solution1(string input)
        {
            var points = Parser.ToArrayOf(input, it => new HyperPoint(it)).ToList();

            var totalConstellations = 0;

            while (points.Any())
            {
                totalConstellations++;
                var toCheck = new Queue<HyperPoint>();
                toCheck.Enqueue(points.First());
                points.RemoveAt(0);

                while (toCheck.Any())
                {
                    var current = toCheck.Dequeue();
                    var inRange = points.Where(it => current.TaxiCabDistanceTo(it) <= 3).ToArray();

                    foreach(var item in inRange)
                    {
                        points.Remove(item);
                        toCheck.Enqueue(item);
                    }
                }
            }

            return totalConstellations;
        }

        private class HyperPoint
        {
            public int W { get; }
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public HyperPoint(string input)
            {
                var items = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();
                W = items[0];
                X = items[1];
                Y = items[2];
                Z = items[3];
            }

            public int TaxiCabDistanceTo(HyperPoint point)
            {
                return Math.Abs(W - point.W) + Math.Abs(X - point.X) + Math.Abs(Y - point.Y) + Math.Abs(Z - point.Z);
            }
        }
    }
}
