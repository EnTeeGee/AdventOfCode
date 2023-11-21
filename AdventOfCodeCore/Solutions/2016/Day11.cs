using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day11
    {
        // not 37
        [Solution(11, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var dict = Enumerable.Range(1, 3).ToDictionary(it => it, it => Parser.SplitOn(lines[it - 1], ",", " and").Select(l => new Component(l)).ToArray());
            dict.Add(4, new Component[0]);

            return Run(dict);
            //return Run2(dict);
            //return Run3(dict);
        }

        [Solution(11, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var dict = Enumerable.Range(1, 3).ToDictionary(it => it, it => Parser.SplitOn(lines[it - 1], ",", " and").Select(l => new Component(l)).ToArray());
            dict.Add(4, new Component[0]);
            var additionalItems = new[] { "An elerium generator", "An elerium-compatible microchip", "A dilithium generator", "A dilithium-compatible microchip" }.Select(it => new Component(it));
            var updatedFloor = dict[1].Concat(additionalItems).ToArray();
            dict[1] = updatedFloor;

            //return Run(dict);
            //return Run2(dict);
            return Run3(dict);
        }

        private int Run(Dictionary<int, Component[]> input)
        {
            var start = new State(0, input, 1);
            //var queue = new Queue<State>();
            //queue.Enqueue(start);
            var queue = new LinkedList<State>();
            queue.AddLast(start);
            //var seen = new HashSet<State>();
            var seen = new HashSet<string>();
            seen.Add(start.Uid);

            var currentStep = 0;

            while (queue.Any())
            {
                //var current = queue.Dequeue();
                var current = queue.First();
                queue.RemoveFirst();
                if(current.Steps > currentStep)
                {
                    Console.WriteLine($"Updated steps to {current.Steps}, items in queue: {queue.Count}");
                    currentStep = current.Steps;
                }
                if (current.IsFinished())
                {
                    foreach (var item in current.PriorStates.Concat(new[] { current }))
                    {
                        //Console.WriteLine(item.Draw());
                        //Console.WriteLine();
                        //Console.WriteLine(item.Uid);
                    }

                    return current.Steps;
                }


                var options = current.GetMoves().Where(it => !seen.Contains(it.Uid)).ToArray();
                foreach (var item in options)
                {
                    //queue.Enqueue(item);
                    queue.AddLast(item);
                    seen.Add(item.Uid);
                    //item.PriorStates.AddRange(current.PriorStates.Concat(new[] { current }));
                }
            }

            throw new Exception("Found no paths");
        }

        private int Run2(Dictionary<int, Component[]> input)
        {
            var start = new State(0, input, 1);
            var queue = new PriorityQueue<State>((a, b) => a.DistanceToEnd - b.DistanceToEnd);
            queue.Insert(start);
            var seen = new Dictionary<State, int>();

            while(queue.Any())
            {
                var current = queue.Pop()!;
                //if (current.IsFinished())
                //    return current.Steps;
                if (current.IsFinished())
                {
                    //foreach (var item in current.PriorStates.Concat(new[] { current }))
                    //{
                    //    Console.WriteLine(item.Draw());
                    //    Console.WriteLine();
                    //}

                    return current.Steps;
                }


                var options = current.GetMoves().Where(it => !seen.ContainsKey(it) || seen[it] > it.Steps).ToArray();
                foreach (var item in options)
                {
                    queue.Insert(item);
                    if (!seen.ContainsKey(item))
                        seen.Add(item, item.Steps);
                    else
                        seen[item] = item.Steps;
                    item.PriorStates.AddRange(current.PriorStates.Concat(new[] { current }));
                }
            }

            throw new Exception("Found no paths");
        }

        private int Run3(Dictionary<int, Component[]> input)
        {
            var start = new State(0, input, 1);
            var endState = Enumerable.Range(1, 4).ToDictionary(it => it, it => new Component[0]);
            endState[4] = input.SelectMany(it => it.Value).ToArray();
            var end = new State(0, endState, 4);
            var startQueue = new Queue<State>();
            var endQueue = new Queue<State>();
            startQueue.Enqueue(start);
            endQueue.Enqueue(end);
            //var startSeen = new HashSet<State> { start };
            //var endSeen = new HashSet<State> { end };
            var startSeen = new Dictionary<string, int> { { start.Uid, 0 } };
            var endSeen = new Dictionary<string, int> { { end.Uid, 0 } };

            var currentStep = 0;

            while (startQueue.Any() && endQueue.Any())
            {
                var currentStart = startQueue.Dequeue();
                if (currentStart.Steps > currentStep)
                {
                    Console.WriteLine($"Updated steps to {currentStart.Steps}, items in queue: {startQueue.Count}");
                    currentStep = currentStart.Steps;
                }

                if (endSeen.ContainsKey(currentStart.Uid))
                {
                    //endSeen.TryGetValue(currentStart, out var matching);
                    var matching = endSeen[currentStart.Uid];

                    return currentStart.Steps + matching;
                }

                var startOptions = currentStart.GetMoves().Where(it => !startSeen.ContainsKey(it.Uid)).ToArray();
                foreach(var item in startOptions)
                {
                    startQueue.Enqueue(item);
                    startSeen.Add(item.Uid, item.Steps);
                }

                var currentEnd = endQueue.Dequeue();
                if (startSeen.ContainsKey(currentEnd.Uid))
                {
                    //startSeen.TryGetValue(currentEnd, out var matching);
                    var matching = startSeen[currentEnd.Uid];

                    return currentEnd.Steps + matching;
                }

                var endOptions = currentEnd.GetMoves(true).Where(it => !endSeen.ContainsKey(it.Uid)).ToArray();
                foreach(var item in endOptions)
                {
                    endQueue.Enqueue(item);
                    endSeen.Add(item.Uid, item.Steps);
                }
            }

            throw new Exception("Found no paths");
        }

        private class Component
        {
            public string Kind { get; }
            public bool IsGenerator { get; }

            public Component(string input)
            {
                var chunks = Parser.SplitOn(input, ' ', '-', '.');
                if(chunks.Contains("generator"))
                {
                    IsGenerator = true;
                    Kind = chunks[Array.IndexOf(chunks, "generator") - 1];
                }
                else
                    Kind = chunks[Array.IndexOf(chunks, "microchip") - 2];
            }

            public override string ToString()
            {
                return $"{Kind.Substring(0, 2)}-{(IsGenerator ? "G" : "M")}";
            }
        }

        private class State
        {
            public int Steps { get; }
            public Dictionary<int, Component[]> OnFloors { get; }
            public int LiftFloor { get; }

            public List<State> PriorStates { get; }

            public string Uid { get; }
            public int DistanceToEnd { get; }

            private int hash;

            public State(int steps, Dictionary<int, Component[]> onFloors, int liftFloor)
            {
                Steps = steps;
                OnFloors = onFloors;
                LiftFloor = liftFloor;

                Uid = $"F:{LiftFloor}|I:{DictToString()}";
                //DistanceToEnd = OnFloors.Select(it => (4 - it.Key) * it.Value.Length).Sum();
                //DistanceToEnd = OnFloors.Select(it => (4 - it.Key) * it.Value.Length).Sum() + steps;
                PriorStates = new List<State>();
                hash = Uid.GetHashCode();
            }

            public State[] GetMoves(bool reversed = false)
            {
                var itemsOnFloor = OnFloors[LiftFloor];
                var floorsToMoveTo = new[] { LiftFloor - 1, LiftFloor + 1 }.Where(it => OnFloors.ContainsKey(it)).ToArray();
                var singleItems = itemsOnFloor.Select(it => new[] { it });
                var combos = Permutations.GetAllPossiblePairs(itemsOnFloor).Where(it => (it[0].IsGenerator == it[1].IsGenerator) || (it[0].Kind == it[1].Kind));
                var possibleMoves = singleItems.Concat(combos).ToArray();
                var output = new List<State>();

                foreach(var floor in floorsToMoveTo)
                {
                    //var listToUse = new Component[0][];
                    //if (floor == LiftFloor + 1)
                    //    listToUse = possibleMoves;
                    //else
                    //    listToUse = singleItems.ToArray();
                    //foreach(var move in listToUse)

                    //var altFloor = reversed ? LiftFloor - 1 : LiftFloor + 1;
                    //if(floor == altFloor)
                    //{
                    //    possibleMoves = possibleMoves.Where(it => !it.Any(c => c.IsGenerator)).ToArray();
                    //}

                    foreach (var move in possibleMoves)
                    {
                        var newDict = new Dictionary<int, Component[]>();
                        foreach(var target in OnFloors.Keys)
                        {
                            if (floor == target)
                                newDict[target] = OnFloors[target].Concat(move).ToArray();
                            else if (target == LiftFloor)
                                newDict[target] = OnFloors[target].Where(it => !move.Contains(it)).ToArray();
                            else
                                newDict[target] = OnFloors[target];
                        }

                        output.Add(new State(Steps + 1, newDict, floor));
                    }
                }
                
                foreach(var item in output)
                {
                    var test = item.IsValidState();
                }

                return output.Where(it => it.IsValidState()).ToArray();
            }

            public bool IsFinished()
            {
                return Enumerable.Range(1, 3).All(it => OnFloors[it].Length == 0);
            }

            public string Draw()
            {
                var output = new StringBuilder();
                output.AppendLine($"Step: {Steps}");
                foreach(var floor in OnFloors.Keys.OrderByDescending(it => it))
                {
                    output.Append(floor == LiftFloor ? "L" : ".");
                    foreach(var item in OnFloors[floor])
                        output.Append($"{item.Kind.Substring(0, 2)}-{(item.IsGenerator ? "G" : "M")} ");

                    output.AppendLine();
                }

                return output.ToString();
            }

            private bool IsValidState()
            {
                foreach(var floor in OnFloors)
                {
                    if (floor.Value.Where(it => it.IsGenerator).Count() == 0)
                        continue;

                    var chips = floor.Value.Where(it => !it.IsGenerator).ToArray();

                    foreach(var chip in chips)
                    {
                        if (!floor.Value.Any(it => it.IsGenerator && it.Kind == chip.Kind))
                            return false;
                    }
                }

                return true;
            }

            private string DictToString()
            {
                return string.Join(
                    "|",
                    OnFloors
                        .OrderBy(it => it.Key)
                        .Select(it => $"{it.Key}({string.Join("|", it.Value.OrderBy(v => v.Kind).ThenBy(v => v.IsGenerator).Select(v => v.ToString()))})"));
            }

            public override int GetHashCode()
            {
                //return Uid.GetHashCode();
                return hash;
            }

            public override bool Equals(object? obj)
            {
                var cast = obj as State;
                if (cast == null)
                    return false;

                return Uid == cast.Uid;
            }
        }
    }
}
