using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2024
{
    internal class Day23
    {
        [Solution(23, 1)]
        public int Solution1(string input)
        {
            var computers = GetComputers(input);

            var compList = computers.Values.ToArray();
            var connections = new HashSet<string>();
            for(var i = 0; i < compList.Length; i++)
            {
                foreach (var connected in compList[i].Connections)
                {
                    var overlap = compList[i].Connections.Where(it => connected.Connections.Contains(it));
                    foreach (var item in overlap)
                    {
                        if (compList[i].Name[0] != 't'
                            && connected.Name[0] != 't'
                            && item.Name[0] != 't')
                            continue;

                        connections.Add(
                            string.Join(
                                string.Empty,
                                new[] { compList[i].Name, connected.Name, item.Name }
                                    .OrderBy(it => it)));
                    }
                }
            }

            return connections.Count;
        }

        [Solution(23, 2)]
        public string Solution2(string input)
        {
            var computers = GetComputers(input);
            var compArray = computers.Values.ToArray();
            var matchesPattern = new List<string>();

            foreach(var item in compArray)
            {
                var reducedArray = GetReducedComputers(input, item.Connections.Select(it => it.Name).Append(item.Name).ToArray());
                var groups = reducedArray.Values.GroupBy(it => it.Connections.Count);

                var groupStr = string.Join(',', groups.Select(it => it.Key).OrderBy(it => it));
                if (groupStr == "1,3,4" || groupStr == "1,12,13")
                    matchesPattern.Add(item.Name);

                //var targetNodes = reducedArray[item.Name].Connections.Count;
                //var unjoinedNodes = reducedArray.Values.GroupBy(it => it.Connections.Count).Sum(it => (targetNodes - it.Key) * it.Count());
                //Console.WriteLine($"for {item.Name}, unjoined nodes {unjoinedNodes} with groups {testStr}");
            }

            return string.Join(',', matchesPattern.OrderBy(it => it));
        }

        private Dictionary<string, Computer> GetComputers(string input)
        {
            var pairs = Parser.ToArrayOf(input, it => Parser.SplitOn(it, '-'));
            var computers = new Dictionary<string, Computer>();
            foreach (var item in pairs)
            {
                if (!computers.ContainsKey(item[0]))
                    computers.Add(item[0], new Computer(item[0]));
                if (!computers.ContainsKey(item[1]))
                    computers.Add(item[1], new Computer(item[1]));

                computers[item[0]].Connections.Add(computers[item[1]]);
                computers[item[1]].Connections.Add(computers[item[0]]);
            }

            return computers;
        }

        private Dictionary<string, Computer> GetReducedComputers(string input, string[] only)
        {
            var pairs = Parser.ToArrayOf(input, it => Parser.SplitOn(it, '-'));
            var computers = new Dictionary<string, Computer>();
            foreach (var item in pairs.Where(it => only.Contains(it[0]) && only.Contains(it[1])))
            {
                if (!computers.ContainsKey(item[0]))
                    computers.Add(item[0], new Computer(item[0]));
                if (!computers.ContainsKey(item[1]))
                    computers.Add(item[1], new Computer(item[1]));

                computers[item[0]].Connections.Add(computers[item[1]]);
                computers[item[1]].Connections.Add(computers[item[0]]);
            }

            return computers;
        }

        private class Computer
        {
            public string Name { get; }
            public List<Computer> Connections { get; }

            public Computer(string name)
            {
                this.Name = name;
                this.Connections = new List<Computer>();
            }
        }
    }
}
