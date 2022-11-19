using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2016
{
    class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it)).ToList();
            var maxBot = lines.Where(it => it[0] == "bot").Select(it => int.Parse(it[1])).Max();

            var bots = Enumerable.Range(0, maxBot + 1).Select(it => new Bot(it)).ToArray();

            var values = lines.Where(it => it[0] == "value").ToArray();
            var commands = lines.Where(it => it[0] == "bot").OrderBy(it => int.Parse(it[1])).ToArray();
            var outputs = new Dictionary<int, int>();

            foreach(var item in values)
            {
                var target = int.Parse(item[5]);
                var value = int.Parse(item[1]);
                bots[target].Values.Add(value);

                foreach(var bot in bots.Where(it => it.Values.Count == 2))
                {
                    var result = ApplyBotCommand(commands, bot, bots, outputs);

                    if (result != null)
                        return result.Value;
                }
            }

            throw new Exception("No bot contained values");
        }

        [Solution(10, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it)).ToList();
            var maxBot = lines.Where(it => it[0] == "bot").Select(it => int.Parse(it[1])).Max();

            var bots = Enumerable.Range(0, maxBot + 1).Select(it => new Bot(it)).ToArray();

            var values = lines.Where(it => it[0] == "value").ToArray();
            var commands = lines.Where(it => it[0] == "bot").OrderBy(it => int.Parse(it[1])).ToArray();
            var outputs = new Dictionary<int, int>();

            foreach (var item in values)
            {
                var target = int.Parse(item[5]);
                var value = int.Parse(item[1]);
                bots[target].Values.Add(value);

                foreach (var bot in bots.Where(it => it.Values.Count == 2))
                    ApplyBotCommand(commands, bot, bots, outputs, false);
            }

            return outputs[0] * outputs[1] * outputs[2];
        }

        private int? ApplyBotCommand(string[][] allCommands, Bot bot, Bot[] allBots, Dictionary<int, int> outputs, bool returnMatch = true)
        {
            var command = allCommands[bot.Id];
            var target1 = command[5];
            var value1 = int.Parse(command[6]);
            var target2 = command[10];
            var value2 = int.Parse(command[11]);
            var low = bot.Values.Min();
            var high = bot.Values.Max();
            var updatedBots = new List<Bot>();
            bot.Values.Clear();

            if(target1 == "output")
                outputs[value1] = low;
            else
            {
                allBots[value1].Values.Add(low);
                updatedBots.Add(allBots[value1]);
            }

            if (target2 == "output")
                outputs[value2] = high;
            else
            {
                allBots[value2].Values.Add(high);
                updatedBots.Add(allBots[value2]);
            }
            
            foreach(var item in updatedBots.Where(it => it.Values.Count == 2))
            {
                if (item.IsPart1Match() && returnMatch)
                    return item.Id;

                var subResult = ApplyBotCommand(allCommands, item, allBots, outputs, returnMatch);
                if (subResult != null && returnMatch)
                    return subResult;
            }

            return null;
        }

        private class Bot
        {
            public List<int> Values { get; }
            public int Id { get; }

            public Bot(int id)
            {
                Values = new List<int>();
                Id = id;
            }

            

            public bool IsPart1Match()
            {
                return Values.Contains(61) && Values.Contains(17);
            }
        }
    }
}
