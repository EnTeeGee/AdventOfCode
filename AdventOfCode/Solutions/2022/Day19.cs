using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2022
{
    class Day19
    {
        // Try and figure out the way to get to 1 geode cracker as fast as possible.
        // If that's less than 24, figure out the way to 2 and so on

        // example costs:
        // ore: 4 ore
        // clay: 2 ore
        // obs: 2 ore, 14 clay
        // geode: 2 ore, 7 obs

        // example process:
        // ore collection stays at 1/sec
        // gets clay up to 3/sec
        // gets obsidian up to 1/sec
        // raises clay to 4/sec
        // raises obsidian to 2/sec
        // then makes geode crackers

        // to make 1 geode:
        // we need 2 minutes to make the robot and mine one
        // we need to make 2 ore (2 minutes of ore production
        // we need to make 7 obsidian


        


        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var blueprints = Parser.ToArrayOf(input, it => new Blueprint(it));
            var startingPerMinute = new[] { 1, 0, 0, 0 };

            var test1 = GetBestFor(1, blueprints[0], new int?[4], startingPerMinute);

            var best = blueprints.Select(it => new { id = it.Id, total = GetBestFor(1, it, new int?[4], startingPerMinute) })
                .OrderByDescending(it => it.total)
                .First();

            return best.id * best.total;
        }

        private int GetBestFor(int minute, Blueprint blueprint, int?[] resources, int[] perMinute)
        {
            if (minute == 24)
                return resources[3].GetValueOrDefault();

            var bestResult = 0;
            var updatedResources = resources.Zip(perMinute, (a, b) => b == 0 ? a : a.GetValueOrDefault() + b).ToArray();

            for (var i = 0; i < 4; i++)
            {
                if (blueprint.CouldBuild(i, resources))
                {
                    var tempResources = resources.ToArray();
                    var tempPerMinute = perMinute.ToArray();
                    if (blueprint.TryBuild(i, tempResources))
                    {
                        tempResources = updatedResources.ToArray();
                        blueprint.TryBuild(i, tempResources);
                        tempPerMinute[i] += 1;
                        bestResult = Math.Max(bestResult, GetBestFor(minute + 1, blueprint, tempResources, tempPerMinute));
                    }
                }
            }

            //Also do a run where nothing is created, to handle objects costing more than we currently have
            bestResult = Math.Max(bestResult, GetBestFor(minute + 1, blueprint, updatedResources, perMinute));

            return bestResult;
        }

        private class Blueprint
        {
            public int Id { get; }
            private int[][] robots;

            public Blueprint(string input)
            {
                var items = Parser.SplitOnSpace(input);
                Id = int.Parse(items[1].TrimEnd(':'));
                robots = new[] {
                    new[] { int.Parse(items[6]), 0, 0 },
                    new[] { int.Parse(items[12]), 0, 0 },
                    new[] { int.Parse(items[18]), int.Parse(items[21]), 0 },
                    new[] { int.Parse(items[27]), 0, int.Parse(items[30]) } };
            }

            public bool CouldBuild(int robot, int?[] resources)
            {
                return robots[robot].Zip(resources, (a, b) => a == 0 || b != null).All(it => it);
            }

            public bool TryBuild(int robot, int?[] resources)
            {
                if(robots[robot].Zip(resources, (a, b) => b == null ? true : b.Value - a >= 0).All(it => it))
                {
                    for (var i = 0; i < resources.Length - 1; i++)
                        resources[i] = robots[robot][i] != 0 ? resources[i] - robots[robot][i] : resources[i];
                    return true;
                }

                return false;
            }
        }
    }
}
