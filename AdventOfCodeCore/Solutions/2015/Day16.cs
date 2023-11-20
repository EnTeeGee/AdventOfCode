using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var aunts = Parser.ToArrayOf(input, it => new Aunt(it));
            var target = new Dictionary<string, int>
            {
                { "children", 3 },
                { "cats", 7 },
                { "samoyeds", 2 },
                { "pomeranians", 3 },
                { "akitas", 0 },
                { "vizslas", 0 },
                { "goldfish", 5 },
                { "trees", 3 },
                { "cars", 2 },
                { "perfumes", 1 }
            };

            foreach (var item in aunts)
            {
                var matched = true;

                foreach (var pair in item.Items)
                {
                    if (target[pair.Key] != pair.Value)
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                    return item.Id;
            }

            throw new Exception("No matching aunt found");
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            var aunts = Parser.ToArrayOf(input, it => new Aunt(it));
            var readings = new Dictionary<string, int>
            {
                { "children", 3 },
                { "cats", 7 },
                { "samoyeds", 2 },
                { "pomeranians", 3 },
                { "akitas", 0 },
                { "vizslas", 0 },
                { "goldfish", 5 },
                { "trees", 3 },
                { "cars", 2 },
                { "perfumes", 1 }
            };

            foreach (var item in aunts)
            {
                var matched = true;

                foreach (var pair in item.Items)
                {
                    if (pair.Key == "cats" || pair.Key == "trees")
                    {
                        if (pair.Value <= readings[pair.Key])
                        {
                            matched = false;
                            break;
                        }
                    }
                    else if (pair.Key == "pomeranians" || pair.Key == "goldfish")
                    {
                        if (pair.Value >= readings[pair.Key])
                        {
                            matched = false;
                            break;
                        }
                    }
                    else if (readings[pair.Key] != pair.Value)
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                    return item.Id;
            }

            throw new Exception("No matching aunt found");
        }

        private class Aunt
        {
            public int Id { get; }

            public Dictionary<string, int> Items { get; }

            public Aunt(string input)
            {
                var tokens = Parser.SplitOn(input, ' ', ':', ',');
                Id = int.Parse(tokens[1]);
                Items = new Dictionary<string, int>();
                tokens = tokens.Skip(2).ToArray();

                while (tokens.Any())
                {
                    Items.Add(tokens[0], int.Parse(tokens[1]));
                    tokens = tokens.Skip(2).ToArray();
                }
            }
        }
    }
}
