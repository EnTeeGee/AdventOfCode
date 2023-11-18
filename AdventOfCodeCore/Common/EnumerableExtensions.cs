namespace AdventOfCodeCore.Common
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> input, Func<T, T, bool> compareFunc)
        {
            var matches = new List<T>();

            foreach(var item in input)
            {
                if (matches.Any(it => compareFunc(item, it)))
                    continue;

                matches.Add(item);
                yield return item;
            }
        }
    }
}
