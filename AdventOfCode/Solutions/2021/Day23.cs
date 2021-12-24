using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2021
{
    class Day23
    {
        [Solution(23, 1)]
        public long Solution1(string input)
        {
            Amphipod.Depth = 2;
            var items = Parser.ToArrayOfString(input).Skip(2).Take(2).Select(it => it.Where(c => char.IsLetter(c)).ToArray()).ToArray();
            List<Amphipod> starting = new List<Amphipod>();
            for(var i = 0; i < 4; i++)
            {
                starting.Add(new Amphipod(items[0][i], new Point((i * 2) + 2, 1)));
                starting.Add(new Amphipod(items[1][i], new Point((i * 2) + 2, 2)));
            }
            var queue = new PriorityQueue<State>((a, b) => (int)(a.TotalCost - b.TotalCost));
            queue.Insert(new State(starting.ToArray(), 0));
            var seen = new Dictionary<State, long>();
            seen.Add(new State(starting.ToArray(), 0), 0);

            while (queue.Any())
            {
                var current = queue.Pop();
                if (current.Items.All(it => it.InRightPlace()))
                    return current.TotalCost;
                foreach(var item in current.Items)
                {
                    var possiblePaths = item.GetAvailableMoves(current.Items);
                    foreach(var path in possiblePaths)
                    {
                        var newItems = current.Items.Where(it => it != item).Concat(new[] { new Amphipod(item.Letter, path.pos) }).ToArray();
                        var newItem = new State(newItems, current.TotalCost + path.cost);
                        if (seen.ContainsKey(newItem) && seen[newItem] <= newItem.TotalCost)
                            continue;

                        queue.Insert(newItem);
                        if (seen.ContainsKey(newItem))
                            seen[newItem] = newItem.TotalCost;
                        else
                            seen.Add(newItem, newItem.TotalCost);
                    }
                        
                }
            }

            throw new Exception("Found no path");
        }

        [Solution(23, 2)]
        public long Solution2(string input)
        {
            Amphipod.Depth = 4;
            var items = Parser.ToArrayOfString(input).Skip(2).Take(2).Select(it => it.Where(c => char.IsLetter(c)).ToArray()).ToArray();
            List<Amphipod> starting = new List<Amphipod>();
            for (var i = 0; i < 4; i++)
            {
                starting.Add(new Amphipod(items[0][i], new Point((i * 2) + 2, 1)));
                starting.Add(new Amphipod(items[1][i], new Point((i * 2) + 2, 4)));
            }

            starting.Add(new Amphipod('D', new Point(2, 2)));
            starting.Add(new Amphipod('D', new Point(2, 3)));
            starting.Add(new Amphipod('C', new Point(4, 2)));
            starting.Add(new Amphipod('B', new Point(4, 3)));
            starting.Add(new Amphipod('B', new Point(6, 2)));
            starting.Add(new Amphipod('A', new Point(6, 3)));
            starting.Add(new Amphipod('A', new Point(8, 2)));
            starting.Add(new Amphipod('C', new Point(8, 3)));

            var queue = new PriorityQueue<State>((a, b) => (int)(a.TotalCost - b.TotalCost));
            var startingState = new State(starting.ToArray(), 0);
            startingState.History = new State[0];
            queue.Insert(startingState);
            var seen = new Dictionary<State, long>();
            seen.Add(startingState, 0);

            while (queue.Any())
            {
                var current = queue.Pop();
                if (current.Items.All(it => it.InRightPlace()))
                {
                    // print history
                    //foreach(var item in current.History)
                    //{
                    //    item.Print();
                    //    Console.WriteLine();
                    //}

                    return current.TotalCost;
                }
                    
                foreach (var item in current.Items)
                {
                    var possiblePaths = item.GetAvailableMoves(current.Items);
                    foreach (var path in possiblePaths)
                    {
                        var newItems = current.Items.Where(it => it != item).Concat(new[] { new Amphipod(item.Letter, path.pos) }).ToArray();
                        var newItem = new State(newItems, current.TotalCost + path.cost);
                        if (seen.ContainsKey(newItem) && seen[newItem] <= newItem.TotalCost)
                            continue;

                        //newItem.History = current.History.Concat(new[] { current }).ToArray();

                        queue.Insert(newItem);
                        if (seen.ContainsKey(newItem))
                            seen[newItem] = newItem.TotalCost;
                        else
                            seen.Add(newItem, newItem.TotalCost);
                    }

                }
            }

            throw new Exception("Found no path");
        }

        private class Amphipod
        {
            public char Letter { get; }
            public Point Position { get; }
            public static int Depth { get; set; }

            public Amphipod(char letter, Point position)
            {
                Letter = letter;
                Position = position;
            }

            public (Point pos, long cost)[] GetAvailableMoves(Amphipod[] existing)
            {
                if(Position.Y > 1)
                {
                    var above = existing.Any(it => it.Position.X == Position.X && it.Position.Y < Position.Y);
                    if(above)
                        return new (Point pos, long cost)[0]; // blocked in
                }

                if(Position.Y == 0) // in hallway
                {
                    var leftMost = existing.Where(it => it.Position.Y == 0 && it.Position.X < Position.X).OrderByDescending(it => it.Position.X).FirstOrDefault();
                    var rightMost = existing.Where(it => it.Position.Y == 0 && it.Position.X > Position.X).OrderBy(it => it.Position.X).FirstOrDefault();

                    var leftMostInt = leftMost?.Position.X ?? -1;
                    var rightMostInt = rightMost?.Position.X ?? 11;
                    var itemsInTarget = GetTargetPoints().Select(it => existing.FirstOrDefault(p => p.Position.Equals(it))).Where(it => it != null).ToArray();
                    var allTargets = GetTargetPoints().Where(it => it.X > leftMostInt && it.X < rightMostInt).ToArray();
                    if(!allTargets.Any()) // target unreachable
                        return new (Point pos, long cost)[0];

                    if(itemsInTarget.Length == allTargets.Length || itemsInTarget.Any(it => it.Letter != Letter)) // full or contains others
                        return new (Point pos, long cost)[0];

                    var topInTarget = itemsInTarget.OrderBy(it => it.Position.Y).FirstOrDefault();
                    if (topInTarget != null && topInTarget.Letter != Letter) // contains other item
                        return new (Point pos, long cost)[0];

                    var newPos = new Point(allTargets.First().X, allTargets.Length - itemsInTarget.Length);
                    return new[] { (newPos, Position.GetTaxiCabDistanceTo(newPos) * GetCostPerMove()) };
                }
                else // in room
                {
                    var itemsInTarget = GetTargetPoints().Select(it => existing.FirstOrDefault(p => p.Position.Equals(it))).Where(it => it != null).ToArray();
                    if(itemsInTarget.All(it => it.Letter == Letter) && itemsInTarget.Contains(this)) // sorted, no need to leave again
                        return new (Point pos, long cost)[0];

                    //var inHallway = existing.Where(it => it.Position.Y == 0).Count();
                    //if (inHallway == 1)
                    //    Console.WriteLine("test");

                    var leftMost = existing.Where(it => it.Position.Y == 0 && it.Position.X < Position.X).OrderByDescending(it => it.Position.X).FirstOrDefault();
                    var rightMost = existing.Where(it => it.Position.Y == 0 && it.Position.X > Position.X).OrderBy(it => it.Position.X).FirstOrDefault();
                    var leftMostInt = leftMost?.Position.X ?? -1;
                    var rightMostInt = rightMost?.Position.X ?? 11;
                    var targets = GetValidHallwayPositions().Where(it => it.X > leftMostInt && it.X < rightMostInt).ToArray();

                    return targets.Select(it => (it, it.GetTaxiCabDistanceTo(Position) * GetCostPerMove())).ToArray();
                }
            }

            public bool InRightPlace()
            {
                return GetTargetPoints().Contains(Position);
            }

            public bool IsSame(Amphipod item)
            {
                return item.Position.Equals(Position) && item.Letter == Letter;
            }

            public string GetSerialString()
            {
                return Letter.ToString() + Position.ToString();
            }

            private int GetCostPerMove()
            {
                switch(Letter)
                {
                    case 'A':
                        return 1;
                    case 'B':
                        return 10;
                    case 'C':
                        return 100;
                    case 'D':
                        return 1000;
                }

                throw new Exception("Unexpected letter");
            }

            private Point[] GetTargetPoints()
            {
                int x = 0;

                switch (Letter)
                {
                    case 'A':
                        x = 2;
                        break;
                    case 'B':
                        x = 4;
                        break;
                    case 'C':
                        x = 6;
                        break;
                    case 'D':
                        x = 8;
                        break;
                }

                return Enumerable.Range(1, Depth).Select(it => new Point(x, it)).ToArray();
            }

            private Point[] GetValidHallwayPositions()
            {
                var x = new[] { 0, 1, 3, 5, 7, 9, 10 };

                return x.Select(it => new Point(it, 0)).ToArray();
            }
        }

        private class State
        {
            public Amphipod[] Items { get; }
            public long TotalCost { get; }
            private string serial;

            public State[] History { get; set; }

            public State(Amphipod[] items, long totalCost)
            {
                //Items = items;
                Items = items.OrderBy(it => it.Position.Y).ThenBy(it => it.Position.X).ToArray();
                TotalCost = totalCost;
                serial = string.Join("", Items.Select(it => it.GetSerialString()));
                
            }

            public bool AreSame(State state)
            {
                var SameItems = Items.Zip(state.Items, (a, b) => a.IsSame(b)).All(it => it);

                return SameItems && TotalCost == state.TotalCost;
            }

            public void Print()
            {
                for(var i = 0; i < 5; i++)
                {
                    var builder = new StringBuilder();
                    for(var j = 0; j < 11; j++)
                    {
                        var point = new Point(j, i);
                        var matching = Items.FirstOrDefault(it => it.Position.Equals(point));
                        if (matching == null)
                            builder.Append(".");
                        else
                            builder.Append(matching.Letter);
                    }
                    Console.WriteLine(builder.ToString());
                }
            }

            public override int GetHashCode()
            {
                return serial.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (!(obj is State))
                    return false;

                var cast = (State)obj;

                return serial == cast.serial;
            }
        }
    }
}
