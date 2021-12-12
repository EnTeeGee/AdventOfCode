using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day12
    {
        [Solution(12, 1)]
        public int Solution1(string input)
        {
            var caves = GetLinkedCaves(input);

            return FindRoutes(new[] { caves["start"] }, true);
        }

        [Solution(12, 2)]
        public int Solution2(string input)
        {
            var caves = GetLinkedCaves(input);

            return FindRoutes(new[] { caves["start"] }, false);
        }

        private Dictionary<string, Cave> GetLinkedCaves(string input)
        {
            var caveIds = Parser.ToArrayOfString(input).Select(it => Parser.SplitOn(it, '-')).ToArray();
            var caves = caveIds.SelectMany(it => it).Distinct().Select(it => new Cave(it)).ToDictionary(it => it.Id, it => it);

            foreach (var item in caveIds)
            {
                var first = caves[item[0]];
                var second = caves[item[1]];
                first.Links.Add(second);
                second.Links.Add(first);
            }

            return caves;
        }

        private int FindRoutes(Cave[] progress, bool doubleVisit)
        {
            if (progress.Last().Id == "end")
                return 1;

            var routes = progress.Last().Links.Where(it => (it.IsBig || !doubleVisit || !progress.Contains(it)) && it.Id != "start").ToArray();
            if (!routes.Any())
                return 0;

            return routes
                .Select(it => progress.Concat(new[] { it }).ToArray())
                .Sum(it => FindRoutes(it, doubleVisit || it.Where(c => !c.IsBig).GroupBy(c => c.Id).Any(c => c.Count() > 1)));
        }

        private class Cave
        {
            public string Id { get; }
            public bool IsBig { get { return char.IsUpper(Id[0]); } }
            public List<Cave> Links { get; }

            public Cave(string id)
            {
                Id = id;
                Links = new List<Cave>();
            }
        }
    }
}
