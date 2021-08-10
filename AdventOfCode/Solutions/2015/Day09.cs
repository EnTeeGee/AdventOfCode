using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day09
    {
        [Solution(9, 1)]
        public int Solution1(string input)
        {
            var paths = Parser.ToArrayOf(input, it => new Path(it));
            var startingPoints = paths.SelectMany(it => it.Locations).Distinct().ToList();

            var allPaths = startingPoints.SelectMany(it => GetSubPaths(it, startingPoints.Where(sp => sp != it).ToList(), paths, new List<Path>())).ToList();

            return allPaths.Select(it => it.Sum(p => p.Distance)).Min();
        }

        [Solution(9, 2)]
        public int Solution2(string input)
        {
            var paths = Parser.ToArrayOf(input, it => new Path(it));
            var startingPoints = paths.SelectMany(it => it.Locations).Distinct().ToList();

            var allPaths = startingPoints.SelectMany(it => GetSubPaths(it, startingPoints.Where(sp => sp != it).ToList(), paths, new List<Path>())).ToList();

            return allPaths.Select(it => it.Sum(p => p.Distance)).Max();
        }

        private class Path
        {
            public Path(string input)
            {
                var tokens = Parser.SplitOnSpace(input);
                Locations = new[] { tokens[0], tokens[2] };
                Distance = int.Parse(tokens.Last());
            }

            public string[] Locations { get; }

            public int Distance { get; }
        }

        private List<List<Path>> GetSubPaths(string current, List<string> remainingDestinations, Path[] paths, List<Path> upToNow)
        {
            if(remainingDestinations.Count == 1)
            {
                return new List<List<Path>> { upToNow.Append(paths.First(it => it.Locations.Contains(remainingDestinations[0]) && it.Locations.Contains(current))).ToList() };
            }

            var output = new List<List<Path>>();
            foreach(var item in remainingDestinations)
            {
                var pathTo = paths.First(it => it.Locations.Contains(item) && it.Locations.Contains(current));

                output = output.Concat(GetSubPaths(item, remainingDestinations.Where(it => it != item).ToList(), paths, upToNow.Append(pathTo).ToList())).ToList();
            }

            return output;
        }
    }
}
