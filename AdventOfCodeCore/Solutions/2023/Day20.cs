using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day20
    {
        [Solution(20, 1)]
        public int Solution1(string input)
        {
            var modules = Parser.ToArrayOf(input, it => new Module(it)).ToDictionary(it => it.Name);
            foreach (var item in modules.Values)
                item.SetSourceModules(modules.Values);

            var pending = new Queue<Pulse>();
            var totalHigh = 0;
            var totalLow = 0;
            for(var i = 0; i < 1000; i++)
            {
                pending.Enqueue(new Pulse("button", false, "broadcaster"));
                totalLow++;

                while (pending.Any())
                {
                    var current = pending.Dequeue();
                    if (modules.ContainsKey(current.Dest))
                    {
                        var result = modules[current.Dest].HandleInput(current);
                        totalHigh += result.Count(it => it.IsHigh);
                        totalLow += result.Count(it => !it.IsHigh);
                        foreach (var item in result)
                            pending.Enqueue(item);
                    }
                }
            }

            return totalHigh * totalLow;
        }

        [Solution(20, 2)]
        public long Solution2(string input)
        {
            var modules = Parser.ToArrayOf(input, it => new Module(it)).ToDictionary(it => it.Name);
            foreach (var item in modules.Values)
                item.SetSourceModules(modules.Values);

            var rootModule = modules.Values.First(it => it.TargetModuleNames.Contains("rx"));
            var subTreeRoots = modules.Values.Where(it => it.TargetModuleNames.Contains(rootModule.Name)).ToArray();
            var cycles = new List<int>();

            foreach (var item in subTreeRoots)
            {
                var tree = GetSubTree(item, modules);
                var info = FindCycle(tree, item.Name);
                cycles.Add(info);
            }

            var current = (long)cycles.First();
            foreach(var item in cycles.Skip(1))
            {
                var next = current;
                while (next % item != 0)
                    next += current;

                current = next;
            }

            return current;
        }

        private Dictionary<string, Module> GetSubTree(Module root, Dictionary<string, Module> modules)
        {
            var output = new Dictionary<string, Module>();
            var uncovered = new[] { root };
            while (uncovered.Any())
            {
                foreach (var item in uncovered)
                    output.Add(item.Name, item);

                uncovered = output.Values.SelectMany(it => it.SourceModuleNames)
                    .Distinct()
                    .Where(it => !output.ContainsKey(it))
                    .Select(it => modules[it])
                    .ToArray();
            }

            return output;
        }

        private int FindCycle(Dictionary<string, Module> modules, string root)
        {
            var cycle = 1;
            var pending = new Queue<Pulse>();

            while (true)
            {
                pending.Enqueue(new Pulse("button", false, "broadcaster"));
                while (pending.Any())
                {
                    var current = pending.Dequeue();
                    if (current.Dest == root && current.IsHigh == false)
                        return cycle;

                    if (modules.ContainsKey(current.Dest))
                    {
                        var result = modules[current.Dest].HandleInput(current);
                        foreach (var item in result)
                            pending.Enqueue(item);
                    }
                }

                cycle++;
            }
        }

        private enum ModuleType { FlipFlop, Conjunction, Broadcast, Output }

        private class Module
        {
            public string Name { get; }
            public string[] TargetModuleNames { get; }
            public ModuleType ModuleType { get; }
            public bool IsOn { get; private set; }
            public string[] SourceModuleNames { get; private set; }

            private Dictionary<string, bool> sourceHistory;

            public Module(string input)
            {
                var chunks = Parser.SplitOn(input, " -> ", ", ");
                Name = chunks[0].TrimStart('%', '&');
                TargetModuleNames = chunks.Skip(1).ToArray();
                ModuleType = chunks[0] == "broadcaster" ? ModuleType.Broadcast
                    : chunks[0][0] == '%' ? ModuleType.FlipFlop
                    : chunks[0][0] == '&' ? ModuleType.Conjunction
                    : ModuleType.Output;

                IsOn = false;
                sourceHistory = new Dictionary<string, bool>();
                SourceModuleNames = Array.Empty<string>();
            }

            public void SetSourceModules(IEnumerable<Module> modules)
            {
                var output = new List<string>();
                foreach(var item in modules)
                {
                    if (item.TargetModuleNames.Contains(Name))
                    {
                        sourceHistory.Add(item.Name, false);
                        output.Add(item.Name);
                    }
                }

                SourceModuleNames = output.ToArray();
            }

            public Pulse[] HandleInput(Pulse input)
            {
                if(ModuleType == ModuleType.FlipFlop)
                {
                    if (input.IsHigh)
                        return Array.Empty<Pulse>();

                    IsOn = !IsOn;

                    return TargetModuleNames.Select(it => new Pulse(Name, IsOn, it)).ToArray();
                }
                else if (ModuleType == ModuleType.Conjunction)
                {
                    sourceHistory[input.Source] = input.IsHigh;
                    if (sourceHistory.Values.All(it => it))
                        return TargetModuleNames.Select(it => new Pulse(Name, false, it)).ToArray();
                        

                    return TargetModuleNames.Select(it => new Pulse(Name, true, it)).ToArray();
                }
                else if (ModuleType == ModuleType.Broadcast)
                    return TargetModuleNames.Select(it => new Pulse(Name, input.IsHigh, it)).ToArray();

                return Array.Empty<Pulse>();
            }
        }

        private class Pulse
        {
            public string Source { get; }
            public bool IsHigh { get; }
            public string Dest { get; }

            public Pulse(string source, bool isHigh, string dest)
            {
                Source = source;
                IsHigh = isHigh;
                Dest = dest;
            }

            public override string ToString()
            {
                return $"{Source} -{(IsHigh ? "high" : "low")}-> {Dest}";
            }
        }
    }
}
