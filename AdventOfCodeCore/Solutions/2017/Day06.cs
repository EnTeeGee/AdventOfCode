using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day06
    {
        [Solution(6, 1)]
        public int Solution1(string input)
        {
            var values = Parser.SplitOn(input, '\t').Select(it => int.Parse(it)).ToArray();
            var seen = new HashSet<string>{ string.Join("", values) };
            var step = 0;

            while (true)
            {
                var maxIndex = Array.IndexOf(values, values.Max());
                var div = values[maxIndex] / values.Length;
                var mod = values[maxIndex] % values.Length;
                var modTargets = Enumerable.Range(maxIndex + 1, mod).Select(it => it % values.Length).ToArray();
                values[maxIndex] = 0;
                for(var i = 0; i < values.Length; i++)
                {
                    values[i] += div;
                    if (modTargets.Contains(i))
                        values[i]++;
                }

                step++;
                var str = string.Join("", values);
                if (seen.Contains(str))
                    return step;

                seen.Add(str);
            }
        }

        [Solution(6, 2)]
        public int Solution2(string input)
        {
            var values = Parser.SplitOn(input, '\t').Select(it => int.Parse(it)).ToArray();
            var seen = new Dictionary<string, int> { { string.Join("", values), 0 } };
            var step = 0;

            while (true)
            {
                var maxIndex = Array.IndexOf(values, values.Max());
                var div = values[maxIndex] / values.Length;
                var mod = values[maxIndex] % values.Length;
                var modTargets = Enumerable.Range(maxIndex + 1, mod).Select(it => it % values.Length).ToArray();
                values[maxIndex] = 0;
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] += div;
                    if (modTargets.Contains(i))
                        values[i]++;
                }

                step++;
                var str = string.Join("", values);
                if (seen.ContainsKey(str))
                    return step - seen[str];

                seen.Add(str, step);
            }
        }
    }
}
