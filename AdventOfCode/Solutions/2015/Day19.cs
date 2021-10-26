using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions._2015
{
    class Day19
    {
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var rules = lines.Take(lines.Length - 2).Select(it => new Rule(it)).ToArray();
            var molecule = lines.Last();

            var distinct = new HashSet<string>();

            foreach(var item in rules)
            {
                var indexes = GetAllIndexesOf(molecule, item.Source);

                foreach(var match in indexes)
                {
                    var newMolecule = $"{molecule.Substring(0, match)}{item.Result}{molecule.Substring(match + item.Source.Length)}";

                    if (!distinct.Contains(newMolecule))
                        distinct.Add(newMolecule);
                }
            }

            return distinct.Count();
        }

        [Solution(19, 2)] // https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/cy4etju/
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var molecule = lines.Last();

            var moleculeLength = molecule.Where(it => char.IsUpper(it)).Count();
            var yTypes = Regex.Matches(molecule, "Y").Count;
            var rnTypes = Regex.Matches(molecule, "Rn").Count;

            var steps = (moleculeLength - 1) - (yTypes * 2) - (rnTypes * 2);
            

            return steps;
        }


        private class Rule
        {
            public string Source { get; }

            public string Result { get; }

            public Rule(string input)
            {
                var tokens = input.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
                Source = tokens[0];
                Result = tokens[1];
            }
        }

        private int[] GetAllIndexesOf(string input, string pattern)
        {
            var index = 0;
            var output = new List<int>();

            while (true)
            {
                var next = input.IndexOf(pattern, index);
                if (next == -1)
                    return output.ToArray();

                output.Add(next);
                index = next + 1;
            }
        }



    }
}
