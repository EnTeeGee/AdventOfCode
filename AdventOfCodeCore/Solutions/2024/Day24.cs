using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day24
    {
        [Solution(24, 1)]
        public long Solution1(string input)
        {
            var (initialValues, mappings) = GetMappings(input);

            var result = mappings.Keys.Where(it => it[0] == 'z')
                .OrderBy(it => it)
                .Select(it => GetValue(it, initialValues, mappings))
                .Select((it, index) => it ? (long)Math.Pow(2, index) : 0)
                .Sum();

            return result;
        }

        [Solution(24, 2)]
        public string Solution2(string input)
        {
            var (initialValues, mappings) = GetMappings(input);

            var toCheck2 = mappings.Keys.Where(it => it[0] == 'z').OrderBy(it => it).Skip(2).ToArray();
            toCheck2 = toCheck2.Take(toCheck2.Length - 1).ToArray();
            var output = new List<string>();

            foreach (var item in toCheck2)
            {
                output.AddRange(InspectTree(item[1..], initialValues, mappings));
            }

            return string.Join(',', output.OrderBy(it => it));
        }

        private (Dictionary<string, bool>,Dictionary<string, (string op, string[] inputs)>) GetMappings(string input)
        {
            var chunks = Parser.ToArrayOfGroups(input);
            var initialValues = Parser.ToArrayOf(chunks[0], it => Parser.SplitOn(it, ": "))
                .ToDictionary(it => it[0], it => it[1] == "1");
            var mappings = Parser.ToArrayOf(chunks[1], it => Parser.SplitOnSpace(it))
                .ToDictionary(it => it[4], it => (it[1], new[] { it[0], it[2] }));

            return (initialValues, mappings);
        }

        private bool GetValue(
            string key,
            Dictionary<string, bool> initialValues,
            Dictionary<string, (string op, string[] inputs)> mappings)
        {
            if (initialValues.ContainsKey(key))
                return initialValues[key];

            var test = mappings[key];
            var values = mappings[key].inputs.Select(it => GetValue(it, initialValues, mappings)).ToArray();
            if (mappings[key].op == "AND")
                return values[0] & values[1];
            if (mappings[key].op == "OR")
                return values[0] | values[1];
            if (mappings[key].op == "XOR")
                return values[0] ^ values[1];

            throw new Exception("Unexpected operator");
        }

        private string[] InspectTree(
            string digit,
            Dictionary<string, bool> initialValues,
            Dictionary<string, (string op, string[] inputs)> mappings)
        {
            var zKey = $"z{digit}";

            if (mappings[zKey].op != "XOR")
            {
                //Console.WriteLine($"Mapping to {zKey} incorrect");

                return new[] { zKey }; 
            }

            var output = new List<string>();

            var unexpectedAnds = mappings[zKey].inputs.Where(it => mappings[it].op == "AND").ToArray();
            if (unexpectedAnds.Any())
                output.AddRange(unexpectedAnds);

            var xKey = $"x{digit}";
            var yKey = $"y{digit}";
            var unexpectedXors = mappings[zKey].inputs.Where(
                it => mappings[it].op == "XOR"
                && string.Join(',', mappings[it].inputs.OrderBy(it => it)) != $"{xKey},{yKey}");

            if (unexpectedXors.Any())
                output.AddRange(unexpectedXors);

            var carryXor = mappings[zKey].inputs.FirstOrDefault(it => mappings[it].op == "XOR" && !unexpectedXors.Contains(it));
            if(carryXor == null)
            {
                //Console.WriteLine($"Failed to find carry xor for {digit}");

                return output.ToArray();
            }

            var carryKey = mappings[zKey].inputs.FirstOrDefault(it => mappings.ContainsKey(it) && mappings[it].op == "OR");

            if (carryKey == null)
                return output.ToArray();

            var invalidCarry = mappings[carryKey].inputs.Where(it => mappings[it].op != "AND");
            if (invalidCarry.Any())
                output.AddRange(invalidCarry);

            return output.ToArray();
        }
    }
}
