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
        [Solution(19, 1)]
        public int Solution1B(string input)
        {
            var blueprints = Parser.ToArrayOf(input, it => new Blueprint(it));
            var qualityLevel = 0;

            for(var i = 0; i < blueprints.Length; i++)
            {
                var result = CaculateForBlueprint(blueprints[i]);
                qualityLevel += (result * (i + 1));
            }

            return qualityLevel;
        }

        private int CaculateForBlueprint(Blueprint blueprint)
        {
            var initialState = new State(0, new[] { 0, 0, 0, 0 }, new[] { 1, 0, 0, 0 });
            var best = 0;
            var bounday = new Queue<State>();
            bounday.Enqueue(initialState);
            var limits = blueprint.GetCostLimits();

            while (bounday.Any())
            {
                var current = bounday.Dequeue();

                foreach(var item in blueprint.Robots)
                {
                    var updated = GetForState(current.Minute, current.Resources, current.PerMinute, item);
                    if (updated == null)
                        continue;

                    if (limits.Zip(updated.Value.newPerMin, (a, b) => a < b).Any(it => it))
                        continue;

                    if(updated.Value.newTime > 24)
                    {
                        best = Math.Max(best, GetTotalGeode(24 - current.Minute, current.Resources[3], current.PerMinute[3]));

                        continue;
                    }

                    var newState = new State(updated.Value.newTime, updated.Value.newResources, updated.Value.newPerMin);
                    newState.PriorState = current;
                    bounday.Enqueue(newState);
                }

            }

            return best;
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

        private int GetTotalGeode(int minutesLeft, int current, int perMin)
        {
            return current + (minutesLeft * perMin);
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

            public int[] GetCostLimits()
            {
                return new[]
                {
                    Robots.Select(it => it.Cost[0]).Max(),
                    Robots[2].Cost[1],
                    Robots[3].Cost[2]
                };
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

        private class State
        {
            public int Minute { get; }
            public int[] Resources { get; }
            public int[] PerMinute { get; }

            public State PriorState { get; set; }

            public State(int minute, int[] resourecs, int[] perMinute)
            {
                Minute = minute;
                Resources = resourecs;
                PerMinute = perMinute;
            }
        }
    }
}
