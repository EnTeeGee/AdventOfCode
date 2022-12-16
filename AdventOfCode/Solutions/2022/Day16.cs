using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var nodes = Parser.ToArrayOf(input, it => new Node(it));
            var keyNodes = nodes.Where(it => it.FlowRate > 0 || it.Valve == "AA").ToArray();
            foreach (var item in keyNodes)
                item.GetDistsTo(nodes, keyNodes);
            var nodeDict = keyNodes.ToDictionary(it => it.Valve);

            var remaining = new Queue<State>();
            remaining.Enqueue(new State(30, 0, new[] { "AA" }));
            var bestScore = 0;

            while (remaining.Any())
            {
                var item = remaining.Dequeue();
                var source = nodeDict[item.NodePath.Last()];
                var validTargets = source.DistTo.Where(it => !item.NodePath.Contains(it.Key) && (item.Remaining >= it.Value)).ToArray();

                if (!validTargets.Any())
                {
                    bestScore = Math.Max(bestScore, item.TotalFlow);
                    continue;
                }

                foreach(var target in validTargets)
                {
                    var newRemaining = item.Remaining - target.Value;
                    var newState = new State(newRemaining, item.TotalFlow + (newRemaining * nodeDict[target.Key].FlowRate), item.NodePath.Concat(new[] { target.Key }).ToArray());
                    remaining.Enqueue(newState);
                }
            }

            return bestScore;
        }

        [Solution(16, 2)]
        public int Solution2(string input)
        {
            var nodes = Parser.ToArrayOf(input, it => new Node(it));
            var keyNodes = nodes.Where(it => it.FlowRate > 0 || it.Valve == "AA").ToArray();
            foreach (var item in keyNodes)
                item.GetDistsTo(nodes, keyNodes);
            var nodeDict = keyNodes.ToDictionary(it => it.Valve);
            var peakPerMin = keyNodes.Sum(it => it.FlowRate);

            var remaining = new Queue<DoubleState>();
            remaining.Enqueue(new DoubleState(26, 26, new[] { "AA" }, new[] { "AA" }, 0));
            var bestScore = 0;

            while (remaining.Any())
            {
                var item = remaining.Dequeue();
                var youSource = nodeDict[item.YouPath.Last()];
                var elephantSource = nodeDict[item.ElephantPath.Last()];
                var allVisited = item.YouPath.Concat(item.ElephantPath).ToArray();
                var validYouTargets = youSource.DistTo.Where(it => !allVisited.Contains(it.Key) && (item.YouRemaining >= it.Value)).ToArray();
                var validElephantTargets = elephantSource.DistTo.Where(it => !allVisited.Contains(it.Key) && (item.ElephantRemaining >= it.Value)).ToArray();

                var hypMax = item.TotalFlow + (validYouTargets.Concat(validElephantTargets).Sum(it => nodeDict[it.Key].FlowRate) * Math.Max(item.YouRemaining, item.ElephantRemaining));

                if (hypMax <= bestScore)
                    continue;

                if (!validYouTargets.Any() && !validElephantTargets.Any() && item.TotalFlow > bestScore)
                {
                    Console.WriteLine("New best score " + item.TotalFlow);
                    bestScore = item.TotalFlow;
                    continue;
                }
                if(item.YouRemaining >= item.ElephantRemaining)
                {
                    foreach (var target in validYouTargets.OrderByDescending(it => it.Value))
                    {
                        var newRemaining = item.YouRemaining - target.Value;
                        var newState = new DoubleState(
                            newRemaining,
                            item.ElephantRemaining,
                            item.YouPath.Concat(new[] { target.Key }).ToArray(),
                            item.ElephantPath,
                            item.TotalFlow + (newRemaining * nodeDict[target.Key].FlowRate));
                        remaining.Enqueue(newState);
                    }
                }
                else
                {
                    foreach (var target in validElephantTargets.OrderByDescending(it => it.Value))
                    {
                        var newRemaining = item.ElephantRemaining - target.Value;
                        var newState = new DoubleState(
                            item.YouRemaining,
                            newRemaining,
                            item.YouPath,
                            item.ElephantPath.Concat(new[] { target.Key }).ToArray(),
                            item.TotalFlow + (newRemaining * nodeDict[target.Key].FlowRate));
                        remaining.Enqueue(newState);
                    }
                }
            }

            return bestScore;
        }


        private class Node
        {
            public string Valve { get; }
            public int FlowRate { get; }
            public string[] LinksTo { get; }
            public Dictionary<string, int> DistTo { get; }

            public Node(string input)
            {
                var items = Parser.SplitOn(input, ' ', '=', ';', ',');
                Valve = items[1];
                FlowRate = int.Parse(items[5]);
                LinksTo = items.Skip(10).ToArray();
                DistTo = new Dictionary<string, int>();
            }

            public void GetDistsTo(Node[] allNodes, Node[] toTarget)
            {
                var dict = allNodes.ToDictionary(it => it.Valve);
                var targetDict = toTarget.Select(it => it.Valve).Where(it => it != "AA").ToHashSet();
                var seenDists = new Dictionary<string, int>();
                var front = new[] { this };

                var currentDist = 1;
                while (front.Any())
                {
                    currentDist += 1;
                    front = front.SelectMany(it => it.LinksTo).Where(it => !seenDists.ContainsKey(it)).Distinct().Select(it => dict[it]).ToArray();
                    foreach(var item in front)
                    {
                        seenDists.Add(item.Valve, currentDist);
                        if (targetDict.Contains(item.Valve))
                            DistTo.Add(item.Valve, currentDist);
                    }
                }
            }
        }

        private class State
        {
            public int Remaining { get; }
            public int TotalFlow { get; }
            public string[] NodePath { get; }

            public State(int remaining, int totalFlow, string[] nodePath)
            {
                Remaining = remaining;
                TotalFlow = totalFlow;
                NodePath = nodePath;
            }
        }

        private class DoubleState
        {
            public int YouRemaining { get; }
            public int ElephantRemaining { get; }
            public string[] YouPath { get; }
            public string[] ElephantPath { get; }
            public int TotalFlow { get; }

            public DoubleState(int youRemaining, int elephantRemaining, string[] youPath, string[] elephantPath, int totalFlow)
            {
                YouRemaining = youRemaining;
                ElephantRemaining = elephantRemaining;
                YouPath = youPath;
                ElephantPath = elephantPath;
                TotalFlow = totalFlow;
            }
        }
    }
}
