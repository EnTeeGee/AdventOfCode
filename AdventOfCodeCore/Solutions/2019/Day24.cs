using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    internal class Day24
    {
        [Solution(24, 1)]
        public long Solution1(string input)
        {
            var bugs = input.Where(it => it == '#' || it == '.').Select(it => it == '#').ToArray();
            var seen = new HashSet<string>();
            var current = ToString(bugs);

            while (!seen.Contains(current))
            {
                seen.Add(current);
                bugs = Enumerable.Range(0, bugs.Length).Select(it => NewState(it, bugs)).ToArray();
                current = ToString(bugs);
            }

            var pow = 1L;
            var output = 0L;
            foreach(var item in bugs)
            {
                if (item)
                    output += pow;
                pow *= 2;
            }

            return output;
        }

        [Solution(24, 2)]
        public long Solution2(string input)
        {
            var bugs = input
                .Where(it => it == '#' || it == '.' || it == '?')
                .Select((it, i) => it == '#' ? FromIndex(i) : (Point?)null)
                .Where(it => it != null)
                .Select(it => new Voxel(it!.Value.X, it.Value.Y, 0))
                .ToHashSet();

            //for(var i = 0; i < 10; i++)
            for (var i = 0; i < 200; i++)
                bugs = bugs.SelectMany(it => GetSurrounding(it)).Distinct().Where(it => NewState(it, bugs)).ToHashSet();

            return bugs.Count();
        }

        private Point FromIndex(int index)
        {
            return new Point(index % 5, index / 5);
        }

        private int ToIndex(Point point)
        {
            return (int)(point.X + (point.Y * 5));
        }

        private bool NewState(int index, bool[] state)
        {
            var surrounding = FromIndex(index)
                .GetSurrounding4()
                .Where(it => it.WithinBounds(0, 4, 0, 4))
                .Select(it => ToIndex(it))
                .Count(it => state[it]);

            if (state[index] && surrounding != 1)
                return false;

            if (!state[index] && (surrounding == 1 || surrounding == 2))
                return true;

            return state[index];
        }

        private Voxel[] GetSurrounding(Voxel point)
        {
            var output = new Point(point.X, point.Y)
                .GetSurrounding4()
                .Where(it => it.WithinBounds(0, 4, 0, 4) && it != new Point(2, 2))
                .Select(it => new Voxel(it.X, it.Y, point.Z))
                .ToList();

            var range = new[] { 0, 1, 2, 3, 4 };

            if (point.X == 0)
                output.Add(new Voxel(1, 2, point.Z + 1));
            else if (point.X == 4)
                output.Add(new Voxel(3, 2, point.Z + 1));
            if (point.Y == 0)
                output.Add(new Voxel(2, 1, point.Z + 1));
            else if (point.Y == 4)
                output.Add(new Voxel(2, 3, point.Z + 1));

            if (point.X == 1 && point.Y == 2)
                output.AddRange(range.Select(it => new Voxel(0, it, point.Z - 1)));
            else if (point.X == 3 && point.Y == 2)
                output.AddRange(range.Select(it => new Voxel(4, it, point.Z - 1)));
            else if (point.X == 2 && point.Y == 1)
                output.AddRange(range.Select(it => new Voxel(it, 0, point.Z - 1)));
            else if (point.X == 2 && point.Y == 3)
                output.AddRange(range.Select(it => new Voxel(it, 4, point.Z - 1)));

            return output.ToArray();
        }

        private bool NewState(Voxel point, HashSet<Voxel> existing)
        {
            var surrounding = GetSurrounding(point).Count(it => existing.Contains(it));

            if (existing.Contains(point) && surrounding != 1)
                return false;

            if (!existing.Contains(point) && (surrounding == 1 || surrounding == 2))
                return true;

            return existing.Contains(point);
        }

        private string ToString(bool[] input)
        {
            return new string(input.Select(it => it ? '#' : '.').ToArray());
        }
    }
}
