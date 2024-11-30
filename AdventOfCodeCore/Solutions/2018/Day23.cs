using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            var bots = Parser.ToArrayOf(input, it => new Nanobot(it));

            var maxBot = bots.OrderByDescending(it => it.Radius).First();

            return bots.Count(it => maxBot.Pos.GetTaxicabDistanceTo(it.Pos) <= maxBot.Radius);
        }

        [Solution(23, 2)]
        public long Solution2(string input)
        {
            var bots = Parser.ToArrayOf(input, it => new Nanobot(it));

            var result = CheckCube(
                new Cube(
                    new Voxel(bots.Max(it => it.Pos.X), bots.Max(it => it.Pos.Y), bots.Max(it => it.Pos.Z)),
                    new Voxel(bots.Min(it => it.Pos.X), bots.Min(it => it.Pos.Y), bots.Min(it => it.Pos.Z))),
                bots,
                0);

            return result.dist;
        }

        private (long dist, int count) CheckCube(Cube input, Nanobot[] bots, int toBeat)
        {
            if (input.IsPoint())
                return (input.PosExtent.GetTaxicabDistanceTo(Voxel.Origin), bots.Count(it => it.Pos.GetTaxicabDistanceTo(input.PosExtent) <= it.Radius));

            var subCubes = input.SplitIntoSubCubes()
                .Select(it => new { cube = it, bots = it.BotsInRange(bots)})
                .GroupBy(it => it.bots.Length)
                .OrderByDescending(it => it.Key);

            (long dist, int count) best = (long.MaxValue, toBeat);

            foreach (var group in subCubes)
            {
                if (group.Key <= best.count)
                    break;

                var orderedGroup = group.OrderBy(it => it.cube.DistToPoint(Voxel.Origin));

                foreach(var item in orderedGroup)
                {
                    if (best.count == group.Key)
                        break;

                    var itemResult = CheckCube(item.cube, item.bots, best.count);
                    if (itemResult.count > best.count
                    || itemResult.count == best.count && itemResult.dist < best.dist)
                        best = itemResult;
                }
            }

            return best;
        }

        private class Nanobot
        {
            public Voxel Pos { get; }
            public int Radius { get; }

            public Nanobot(string input)
            {
                var chunks = Parser.SplitOn(input, '<', '>', ',', '=');

                Pos = new Voxel(int.Parse(chunks[1]), int.Parse(chunks[2]), int.Parse(chunks[3]));
                Radius = int.Parse(chunks[5]);
            }
        }

        private class Cube
        {
            public Voxel PosExtent { get; }
            public Voxel NegExtent { get; }

            public Cube(Voxel posExtent, Voxel negExtent)
            {
                PosExtent = posExtent;
                NegExtent = negExtent;
            }

            public bool IsPoint()
            {
                return PosExtent.Equals(NegExtent);
            }

            public long DistToPoint(Voxel point)
            {
                var xDist = Math.Max(0, Math.Max(point.X - PosExtent.X, NegExtent.X - point.X));
                var yDist = Math.Max(0, Math.Max(point.Y - PosExtent.Y, NegExtent.Y - point.Y));
                var zDist = Math.Max(0, Math.Max(point.Z - PosExtent.Z, NegExtent.Z - point.Z));

                return xDist + yDist + zDist;
            }

            public bool IsBotInRange(Nanobot bot)
            {
                return DistToPoint(bot.Pos) <= bot.Radius;
            }

            public Nanobot[] BotsInRange(Nanobot[] bots)
            {
                return bots.Where(it => IsBotInRange(it)).ToArray();
            }

            public Cube[] SplitIntoSubCubes()
            {
                var output = new List<Cube> { this };
                if(PosExtent.X != NegExtent.X)
                {
                    var midX = (PosExtent.X + NegExtent.X) / 2;
                    output = output.SelectMany(it => new[] {
                        new Cube(PosExtent, new Voxel(midX + 1, NegExtent.Y, NegExtent.Z)),
                        new Cube(new Voxel(midX, PosExtent.Y, PosExtent.Z), NegExtent) }).ToList();
                }

                if(PosExtent.Y != NegExtent.Y)
                {
                    var midY = (PosExtent.Y + NegExtent.Y) / 2;
                    output = output.SelectMany(it => new[] {
                        new Cube(it.PosExtent, new Voxel(it.NegExtent.X, midY + 1, it.NegExtent.Z)),
                        new Cube(new Voxel(it.PosExtent.X, midY, it.PosExtent.Z), it.NegExtent) }).ToList();
                }

                if (PosExtent.Z != NegExtent.Z)
                {
                    var midZ = (PosExtent.Z + NegExtent.Z) / 2;
                    output = output.SelectMany(it => new[] {
                        new Cube(it.PosExtent, new Voxel(it.NegExtent.X, it.NegExtent.Y, midZ + 1)),
                        new Cube(new Voxel(it.PosExtent.X, it.PosExtent.Y, midZ), it.NegExtent) }).ToList();
                }

                return output.ToArray();
            }
        }
    }
}
