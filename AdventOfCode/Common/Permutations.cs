using System.Linq;

namespace AdventOfCode.Common
{
    static class Permutations
    {
        public static T[][] GetAllPossiblePairs<T>(T[] items)
        {
            return Enumerable.Range(0, items.Length)
                .SelectMany(it => items.Skip(it + 1).Select(b => new T[] { items[it], b }))
                .ToArray();
        }
    }
}
