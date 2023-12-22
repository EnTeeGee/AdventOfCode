using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day22
    {
        [Solution(22, 1)]
        public int Solution1(string input)
        {
            var bricks = Parser.ToArrayOf(input, it => new Brick(it)).OrderBy(it => it.Z).ToArray();

            var settled = new Dictionary<Voxel, int>();
            var nodes = new Dictionary<int, BrickNode>();

            for(var i = 0; i < bricks.Length; i++)
            {
                var current = bricks[i];
                current.Id = i;

                while (current.TryLower(settled)) ;

                foreach (var point in current.Points)
                    settled.Add(point, i);

                nodes.Add(i, new BrickNode(current));
            }

            foreach (var item in nodes.Values)
                item.Link(settled, nodes);

            return nodes.Values.Count(it => !it.Supports.Any(s => s.RestsOn.Length == 1));
        }

        [Solution(22, 2)]
        public int Solution2(string input)
        {
            var bricks = Parser.ToArrayOf(input, it => new Brick(it)).OrderBy(it => it.Z).ToArray();

            var settled = new Dictionary<Voxel, int>();
            var nodes = new Dictionary<int, BrickNode>();

            for (var i = 0; i < bricks.Length; i++)
            {
                var current = bricks[i];
                current.Id = i;

                while (current.TryLower(settled)) ;

                foreach (var point in current.Points)
                    settled.Add(point, i);

                nodes.Add(i, new BrickNode(current));
            }

            foreach (var item in nodes.Values)
                item.Link(settled, nodes);

            var total = 0;
            foreach(var item in nodes.Values)
                total += item.GetDroppingElements().Length;

            return total;
        }

        private class Brick
        {
            public Voxel[] Points { get; private set; }
            public bool IsSettled { get; private set; }
            public int Z { get; private set; }
            public int Id { get; set; }

            public Brick(string input)
            {
                var values = Parser.SplitOn(input, ',', '~').Select(it => int.Parse(it)).ToArray();

                var start = new Voxel(values[0], values[1], values[2]);
                if (values[0] != values[3])
                    Points = Enumerable.Range(Math.Min(values[0], values[3]), Math.Abs(values[0] - values[3]) + 1)
                        .Select(it => new Voxel(it, values[1], values[2]))
                        .ToArray();
                else if (values[1] != values[4])
                    Points = Enumerable.Range(Math.Min(values[1], values[4]), Math.Abs(values[1] - values[4]) + 1)
                        .Select(it => new Voxel(values[0], it, values[2]))
                        .ToArray();
                else
                    Points = Enumerable.Range(Math.Min(values[2], values[5]), Math.Abs(values[2] - values[5]) + 1)
                        .Select(it => new Voxel(values[0], values[1], it))
                        .ToArray();

                IsSettled = false;
                Z = Points.Min(it => it.Z);
            }

            public bool TryLower(Dictionary<Voxel, int> settled)
            {
                if (IsSettled)
                    return false;

                var steppedDown = Points.Select(it => new Voxel(it.X, it.Y, it.Z - 1)).ToArray();
                if (steppedDown.Any(it => it.Z <= 0))
                {
                    IsSettled = true;

                    return false;
                }

                if(steppedDown.Any(it => settled.ContainsKey(it)))
                {
                    IsSettled = true;

                    return false;
                }

                Points = steppedDown;
                Z = Points.Min(it => it.Z);

                return true;
            }

            public bool Intersects(Voxel[] points)
            {
                return Points.Any(it => points.Contains(it));
            }
        }

        private class BrickNode
        {
            public Brick SourceBrick { get; }
            public BrickNode[] RestsOn { get; private set; }
            public BrickNode[] Supports { get; private set; }

            private int[]? restsCompletlyOn;

            public BrickNode(Brick sourceBrick)
            {
                SourceBrick = sourceBrick;
                RestsOn = Array.Empty<BrickNode>();
                Supports = Array.Empty<BrickNode>();
            }

            public void Link(Dictionary<Voxel, int> settledPoints, Dictionary<int, BrickNode> nodes)
            {
                RestsOn = SourceBrick.Points
                    .Select(it => new Voxel(it.X, it.Y, it.Z - 1))
                    .Where(it => settledPoints.ContainsKey(it))
                    .Select(it => settledPoints[it])
                    .Distinct()
                    .Where(it => it != SourceBrick.Id)
                    .Select(it => nodes[it])
                    .ToArray();

                Supports = SourceBrick.Points
                    .Select(it => new Voxel(it.X, it.Y, it.Z + 1))
                    .Where(it => settledPoints.ContainsKey(it))
                    .Select(it => settledPoints[it])
                    .Distinct()
                    .Where(it => it != SourceBrick.Id)
                    .Select(it => nodes[it])
                    .ToArray();
            }

            public int[] GetDroppingElements()
            {
                if (restsCompletlyOn != null)
                    return restsCompletlyOn;

                if(RestsOn.Length == 0)
                    return Array.Empty<int>();

                var below = RestsOn.Select(it => new[] { it.SourceBrick.Id }.Concat(it.GetDroppingElements()).ToArray()).ToArray();
                var output = new List<int>();
                foreach(var item in below[0])
                {
                    if(below.All(it => it.Contains(item)))
                        output.Add(item);
                }

                restsCompletlyOn = output.ToArray();

                return restsCompletlyOn;
            }
        }
    }
}
