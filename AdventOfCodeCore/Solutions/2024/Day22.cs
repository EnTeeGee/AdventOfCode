using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day22
    {
        private const int pruneValue = 16777216;

        [Solution(22, 1)]
        public long Solution1(string input)
        {
            var values = Parser.ToArrayOf(input, it => GenerateSequence(long.Parse(it)));

            return values.Select(it => it.Skip(2000).First()).Sum();
        }

        [Solution(22, 2)]
        public int Solution2(string input)
        {
            var values = Parser.ToArrayOf(input, it => long.Parse(it));
            var allOptions = new HashSet<string>();
            var optionDicts = new List<Dictionary<string, int>>();

            foreach(var item in values)
            {
                var options = GenerateSequence(item)
                    .Zip(GenerateSequence(item).Skip(1), (a, b) => new[] { a % 10, b % 10 })
                    .Zip(GenerateSequence(item).Skip(2), (a, b) => a.Concat(new[] { b % 10 }))
                    .Zip(GenerateSequence(item).Skip(3), (a, b) => a.Concat(new[] { b % 10 }))
                    .Zip(GenerateSequence(item).Skip(4), (a, b) => a.Concat(new[] { b % 10 }))
                    .Take(1996)
                    .Select(it => new { seq = string.Join(',', it.Zip(it.Skip(1), (a, b) => a - b)), result = (int)it.Last() });

                var newDict = new Dictionary<string, int>();
                foreach(var option in options)
                {
                    if(!newDict.ContainsKey(option.seq))
                        newDict.Add(option.seq, option.result);
                }

                foreach(var key in newDict.Keys)
                    allOptions.Add(key);

                optionDicts.Add(newDict);
            }

            var best = 0;

            foreach(var key in allOptions)
            {
                var result = optionDicts.Sum(it => it.ContainsKey(key) ? it[key] : 0);

                if(result > best)
                    best = result;
            }

            return best;
        }

        private IEnumerable<long> GenerateSequence(long input)
        {
            while (true)
            {
                yield return input;

                input = (input ^ (input * 64)) % pruneValue;
                input = (input ^ (input / 32)) % pruneValue;
                input = (input ^ (input * 2048)) % pruneValue;
            }
        }
    }
}
