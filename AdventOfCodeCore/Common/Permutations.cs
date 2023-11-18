namespace AdventOfCodeCore.Common
{
    internal static class Permutations
    {
        public static T[][] GetPermutations<T>(T[] items) where T : notnull
        {
            if (items.Length == 0)
                return Array.Empty<T[]>();

            if (items.Length == 1)
                return new[] { items };

            var output = new List<T[]>();

            foreach (var item in items)
            {
                var tail = items.Where(it => !item.Equals(it)).ToArray();
                var perms = GetPermutations(tail);
                output.AddRange(perms.Select(it => new[] { item }.Concat(it).ToArray()));
            }

            return output.ToArray();
        }

        public static T[][] GetAllPossiblePairs<T>(T[] items)
        {
            return Enumerable.Range(0, items.Length)
                .SelectMany(it => items.Skip(it + 1).Select(b => new T[] { items[it], b }))
                .ToArray();
        }
    }
}
