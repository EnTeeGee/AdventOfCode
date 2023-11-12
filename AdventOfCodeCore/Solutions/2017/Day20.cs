using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day20
    {
        [Solution(20, 1)]
        public int Solution1(string input)
        {
            var particles = Parser.ToArrayOf(input, it => new Particle(it));
            var minAccel = int.MaxValue;
            var minIndex = 0;

            for(var i = 0; i < particles.Length; i++)
            {
                var accel = particles[i].GetAccelerationMagnitude();
                if(accel < minAccel)
                {
                    minAccel = accel;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        [Solution(20, 2)]
        public int Solution2(string input)
        {
            var particles = Parser.ToArrayOf(input, it => new Particle(it));
            var potentialCollisions = new HashSet<int>();

            for(var i = 0; i < particles.Length; i++)
            {
                var sourceParticle = particles[i];
                for(var j = i + 1; j < particles.Length; j++)
                {
                    var targetPartice = particles[j];
                    var collision = sourceParticle.IntersectsAt(targetPartice);
                    if (collision != null && !potentialCollisions.Contains(collision.Value))
                        potentialCollisions.Add(collision.Value);
                }
            }

            var MaxSteps = potentialCollisions.Max();

            for(var i = 0; i <  MaxSteps; i++)
            {
                particles = particles.Select(it => it.Step())
                    .GroupBy(it => it.Pos)
                    .Where(it => it.Count() == 1)
                    .SelectMany(it => it)
                    .ToArray();
            }

            return particles.Length;
        }

        private class Particle
        {
            public Voxel Pos { get; }
            public Voxel Velocity { get; }
            public Voxel Accel { get; }

            public Particle(string input)
            {
                var chunks = Parser.SplitOnSpace(input);
                var posChunks = Parser.SplitOn(chunks[0], 'p', '=', '<', '>', ',');
                Pos = new Voxel(int.Parse(posChunks[0]), int.Parse(posChunks[1]), int.Parse(posChunks[2]));
                var VelocityChunks = Parser.SplitOn(chunks[1], 'v', '=', '<', '>', ',');
                Velocity = new Voxel(int.Parse(VelocityChunks[0]), int.Parse(VelocityChunks[1]), int.Parse(VelocityChunks[2]));
                var accelChunks = Parser.SplitOn(chunks[2], 'a', '=', '<', '>', ',');
                Accel = new Voxel(int.Parse(accelChunks[0]), int.Parse(accelChunks[1]), int.Parse(accelChunks[2]));
            }

            private Particle(Voxel pos, Voxel vel, Voxel acc)
            {
                Pos = pos;
                Velocity = vel;
                Accel = acc;
            }

            public int GetAccelerationMagnitude()
            {
                return Accel.GetTaxicabDistanceTo(Voxel.Origin);
            }

            public int? IntersectsAt(Particle toCheck)
            {
                var intersects = new[]
                {
                    IntersectCheck(Pos.X, toCheck.Pos.X, Velocity.X, toCheck.Velocity.X, Accel.X, toCheck.Accel.X),
                    IntersectCheck(Pos.X, toCheck.Pos.X, Velocity.X, toCheck.Velocity.X, Accel.X, toCheck.Accel.X),
                    IntersectCheck(Pos.X, toCheck.Pos.X, Velocity.X, toCheck.Velocity.X, Accel.X, toCheck.Accel.X),
                };

                if (intersects.Any(it => it == null) || intersects.Distinct().Count() > 1)
                    return null;

                return intersects[0];
            }

            public Particle Step()
            {
                var newVel = new Voxel(Velocity.X + Accel.X, Velocity.Y + Accel.Y, Velocity.Z + Accel.Z);
                var newPos = new Voxel(Pos.X + newVel.X, Pos.Y + newVel.Y, Pos.Z + newVel.Z);

                return new Particle(newPos, newVel, Accel);
            }

            private int? IntersectCheck(int posA, int posB, int velA, int velB, int accA, int accB)
            {
                var step = 0;

                while (true)
                {
                    if ((posA > posB && velA >= velB && accA >= accB) || (posA < posB && velA <= velB && accA <= accB))
                        return null;

                    if (posA == posB)
                        return step;

                    velA += accA;
                    velB += accB;
                    posA += velA;
                    posB += velB;
                    step++;
                }
            }
        }
    }
}
