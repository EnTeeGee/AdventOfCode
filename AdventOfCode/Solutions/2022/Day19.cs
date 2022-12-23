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
            var startingResources = new[] { 0, 0, 0, 0 };
            var bestGeodeCount = 0;

            foreach(var item in blueprints)
            {
                //var currentMinute = 1;
                var currentMinute = 0;
                var currentResources = startingResources;
                var currentPerMin = startingPerMinute;
                while (true)
                {
                    var bestOption = CheckBestOption(currentResources, currentPerMin, item);

                    var newValues = GetForState(currentMinute, currentResources, currentPerMin, item.Robots[bestOption]).Value;
                    if(newValues.newTime >= 24)
                    {
                        bestGeodeCount = Math.Max(bestGeodeCount, currentResources[3]);

                        break;
                    }

                    currentMinute = newValues.newTime;
                    currentResources = newValues.newResources;
                    currentPerMin = newValues.newPerMin;
                }
                //var bestOption = CheckBestOption()
                //var bestOption = 
            }

            return bestGeodeCount;
        }

        private int CheckBestOption(int[] resources, int[] perMin, Blueprint blueprint)
        {
            var options = Permutations.GetPermutations(blueprint.Robots);

            foreach(var item in options)
            {
                var test1 = TimeToProduce(resources, perMin, item);
            }

            var result = options.Select(it => new { robArray = it, result = TimeToProduce(resources, perMin, it) })
                .Where(it => it.result != null)
                .OrderBy(it => it.result)
                .First();

            return result.robArray.First().Id;
        }

        private int? TimeToProduce(int[] resources, int[] perMin, Robot[] robots)
        {
            var totalTime = 0;
            foreach(var item in robots)
            {
                var result = GetForState(totalTime, resources, perMin, item);
                if (result == null)
                    return null;

                totalTime += result.Value.newTime;
                resources = result.Value.newResources;
                perMin = result.Value.newPerMin;
                if (item.Id == 3)
                    return totalTime;
            }

            return totalTime;
        }
        private (int newTime, int[] newResources, int[] newPerMin)? GetForState(int currentMin, int[] resources, int[] perMin, Robot robot)
        {
            if (robot.Cost.Zip(perMin, (a, b) => a != 0 && b == 0).Any(it => it))
                return null;

            var timeForRobot = 0;
            for (var i = 0; i < robot.Cost.Length; i++)
            {
                var result = TimeNeeded(robot.Cost[i], perMin[i], resources[i]);
                timeForRobot = Math.Max(timeForRobot, result);
            }
            timeForRobot += 1;

            resources = resources.Zip(perMin, (a, b) => a + (b * timeForRobot)).ToArray();
            resources = resources.Zip(robot.Cost, (a, b) => a - b).ToArray();
            perMin = perMin.ToArray();
            perMin[robot.Id] += 1;

            return (currentMin + timeForRobot, resources, perMin);
        }

        private int TimeNeeded(int cost, int perMin, int resources)
        {
            var required = cost - resources;
            if (required <= 0)
                return 0;

            return ((required - 1) / perMin) + 1;
        }
        

        private class Blueprint
        {
            public int Id { get; }
            public Robot[] Robots { get; }

            public Blueprint(string input)
            {
                var items = Parser.SplitOnSpace(input);
                Id = int.Parse(items[1].TrimEnd(':'));
                Robots = new[] {
                    new Robot(0, new[]{ int.Parse(items[6]), 0, 0, 0 }),
                    new Robot(1, new[] { int.Parse(items[12]), 0, 0, 0 }),
                    new Robot(2, new[] { int.Parse(items[18]), int.Parse(items[21]), 0, 0 }),
                    new Robot(3, new[] { int.Parse(items[27]), 0, int.Parse(items[30]), 0 }) };
            }
        }

        private class Robot
        {
            public int Id { get; }
            public int[] Cost { get; }

            public Robot(int id, int[] cost)
            {
                Id = id;
                Cost = cost;
            }
        }
    }
}
