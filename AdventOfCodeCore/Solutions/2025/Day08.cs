using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2025
{
    internal class Day08
    {
        [Solution(8, 1)]
        public long Solution1(string input)
        {
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ','))
                .Select(it => new Voxel(int.Parse(it[0]), int.Parse(it[1]), int.Parse(it[2])))
                .ToArray();
            var pairs = PairUp(points).Take(1000);

            var groups = new List<HashSet<Voxel>>();
            foreach(var pair in pairs)
            {
                var newGroup = new HashSet<Voxel> { pair.a, pair.b };
                for(var i = 0; i < groups.Count; i++)
                {
                    if (!groups[i].Overlaps(newGroup))
                        continue;

                    newGroup.UnionWith(groups[i]);
                    groups.RemoveAt(i);
                    i--;
                }
                groups.Add(newGroup);
            }

            return groups.OrderByDescending(it => it.Count).Take(3).Aggregate(1L, (a, b) => a * b.Count);
        }

        [Solution(8, 2)]
        public long Solution2(string input)
        {
            var points = Parser.ToArrayOf(input, it => Parser.SplitOn(it, ','))
                .Select(it => new Voxel(int.Parse(it[0]), int.Parse(it[1]), int.Parse(it[2])))
                .ToArray();
            var pairs = PairUp(points);

            var groups = points.Select(it => new HashSet<Voxel> { it }).ToList();
            foreach (var pair in pairs)
            {
                var newGroup = new HashSet<Voxel> { pair.a, pair.b };
                for (var i = 0; i < groups.Count; i++)
                {
                    if (!groups[i].Overlaps(newGroup))
                        continue;

                    newGroup.UnionWith(groups[i]);
                    groups.RemoveAt(i);
                    i--;
                }

                if (groups.Count == 0)
                    return pair.a.X * pair.b.X;

                groups.Add(newGroup);
            }

            throw new Exception("Failed to form single group");
        }

        private long SqDist(Voxel a, Voxel b)
        {
            var diff = a - b;
            return (diff.X * diff.X) + (diff.Y * diff.Y) + (diff.Z * diff.Z);
        }

        private List<(Voxel a, Voxel b, long distSq)> PairUp(Voxel[] points)
        {
            var pairs = new List<(Voxel a, Voxel b, long distSq)>();
            for (var i = 0; i < points.Length; i++)
                for (var j = i + 1; j < points.Length; j++)
                    pairs.Add((points[i], points[j], SqDist(points[i], points[j])));

            return pairs.OrderBy(it => it.distSq).ToList();
        }
    }
}
