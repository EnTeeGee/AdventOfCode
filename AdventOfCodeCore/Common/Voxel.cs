namespace AdventOfCodeCore.Common
{
    internal class Voxel
    {
        public long X { get; }
        public long Y { get; }
        public long Z { get; }

        public static Voxel Origin { get { return new Voxel(0, 0, 0); } }

        public Voxel(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Voxel OffsetTo(Voxel target)
        {
            return new Voxel(target.X - X, target.Y - Y, target.Z - Z);
        }

        public Voxel Add(Voxel amount)
        {
            return new Voxel(X + amount.X, Y + amount.Y, Z + amount.Z);
        }

        public Voxel RotateAroundX()
        {
            return new Voxel(X, -Z, Y);
        }

        public Voxel RotateAroundY()
        {
            return new Voxel(Z, Y, -X);
        }

        public Voxel RotateAroundZ()
        {
            return new Voxel(Y, -X, Z);
        }

        public Voxel[] GetAllOrientations()
        {
            var directions = new[]
            {
                this,
                this.RotateAroundZ(),
                this.RotateAroundZ().RotateAroundZ(),
                this.RotateAroundZ().RotateAroundX(),
                this.RotateAroundZ().RotateAroundX().RotateAroundX(),
                this.RotateAroundZ().RotateAroundX().RotateAroundX().RotateAroundX()
            };

            return directions.SelectMany(it => new[]
            {
                it,
                it.RotateAroundY(),
                it.RotateAroundY().RotateAroundY(),
                it.RotateAroundY().RotateAroundY().RotateAroundY()
            }).ToArray();
        }

        public long GetTaxicabDistanceTo(Voxel target)
        {
            return Math.Abs(X - target.X) + Math.Abs(Y - target.Y) + Math.Abs(Z - target.Z);
        }

        public Voxel[] GetSurrounding6()
        {
            return new[]
            {
                new Voxel(X, Y, Z + 1),
                new Voxel(X, Y, Z - 1),
                new Voxel(X, Y + 1, Z),
                new Voxel(X, Y - 1, Z),
                new Voxel(X + 1, Y, Z),
                new Voxel(X - 1, Y, Z)
            };
        }

        public bool WithinBounds(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            return X >= minX && X <= maxX && Y >= minY && Y <= maxY && Z >= minZ && Z <= maxZ;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Voxel cast))
                return false;

            return cast.X == X && cast.Y == Y && cast.Z == Z;
        }

        public override int GetHashCode()
        {
            return (X ^ Y ^ Z).GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static Voxel operator -(Voxel a, Voxel b)
        {
            return new Voxel(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Voxel operator +(Voxel a, Voxel b)
        {
            return new Voxel(a.X + b.X, a.Y + b.Y, a.Z - b.Z);
        }
    }
}
