﻿using AdventOfCode2019.Common;
using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    class Day07
    {
        [Solution(7, 1)]
        public int Problem1(string input)
        {
            var code = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();
            var phaseSettings = new List<int> { 0, 1, 2, 3, 4 };

            return GetHighest(code, 0, phaseSettings);
        }

        private int GetHighest(int[] input, int fromPrevious, List<int> remainingPhases)
        {
            var found = new List<int>();

            foreach(var item in remainingPhases)
            {
                var program = new Intcode(input);
                program.AddInput(item);
                program.AddInput(fromPrevious);
                var result = program.RunToEnd()[0];

                if (remainingPhases.Count == 1)
                {
                    found.Add((int)result);
                    continue;
                } 

                var leftover = remainingPhases.Where(it => it != item).ToList();

                found.Add(leftover.Select(it => GetHighest(input, (int)result, leftover)).Max());
            }

            return found.Max();
        }

        [Solution(7, 2)]
        public int Problem2(string input)
        {
            var code = Parser.SplitOn(input, ',').Select(it => int.Parse(it)).ToArray();
            var phaseSettings = new List<int> { 5, 6, 7, 8, 9 };
            var permutations = GetPermutations(phaseSettings);

            var foundMax = 0;

            foreach(var item in permutations)
            {
                var forPermutation = RunForPhases(code, item);
                foundMax = Math.Max(foundMax, forPermutation);
            }

            return foundMax;
        }

        private List<List<int>> GetPermutations(List<int> remaining)
        {
            if (remaining.Count == 1)
                return new List<List<int>> { remaining };

            var output = new List<List<int>>();

            foreach (var item in remaining)
            {
                var leftover = remaining.Where(it => it != item).ToList();
                var permutations = GetPermutations(leftover);
                foreach (var perm in permutations)
                    perm.Add(item);

                output.AddRange(permutations);
            }

            return output;
        }

        private int RunForPhases(int[] input, List<int> phases)
        {
            var programs = phases.Select(it =>
            {
                var program = new Intcode(input);
                program.AddInput(it);

                return program;
            }).ToArray();

            var currentResult = 0;

            while(!programs[0].HasHalted)
            {
                foreach(var item in programs)
                {
                    item.AddInput(currentResult);
                    var generated = item.RunToEnd();
                    currentResult = (int)generated.Last();
                }
            }

            return currentResult;
        }
    }
}
