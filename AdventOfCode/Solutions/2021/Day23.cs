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
            var items = Parser.ToArrayOfString(input).Skip(2).Take(2).Select(it => it.Where(c => char.IsLetter(c)).ToArray()).ToArray();
            List<Amphipod> starting = new List<Amphipod>();
            for(var i = 0; i < 4; i++)
            {
                starting.Add(new Amphipod(items[0][i], new Point((i * 2) + 2, 1)));
                starting.Add(new Amphipod(items[1][i], new Point((i * 2) + 2, 2)));
            }
            var queue = new PriorityQueue<State>((a, b) => (int)(a.TotalCost - b.TotalCost));
            queue.Insert(new State(starting.ToArray(), 0));
            //var seen = new HashSet<State>();
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

        private class Amphipod
        {
            public char Letter { get; }
            public Point Position { get; }

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
                    var inHallway = existing.Where(it => it.Position.Y == 0).Count();

                    var leftMost = existing.Where(it => it.Position.Y == 0 && it.Position.X < Position.X).OrderByDescending(it => it.Position.X).FirstOrDefault();
                    var rightMost = existing.Where(it => it.Position.Y == 0 && it.Position.X > Position.X).OrderBy(it => it.Position.X).FirstOrDefault();

                    var leftMostInt = leftMost?.Position.X ?? -1;
                    var rightMostInt = rightMost?.Position.X ?? 11;
                    var itemsInTarget = GetTargetPoints().Select(it => existing.FirstOrDefault(p => p.Position.Equals(it))).Where(it => it != null).ToArray();
                    var targets = GetTargetPoints().Where(it => !existing.Any(p => p.Position.Equals(it)) && it.X > leftMostInt && it.X < rightMostInt).ToArray();

                    if (targets.Length == 2)
                        return new[] { (targets[1], Position.GetTaxiCabDistanceTo(targets[1]) * GetCostPerMove()) };
                    else if (targets.Length == 1 && itemsInTarget.First().Letter == Letter)
                        return new[] { (targets[0], Position.GetTaxiCabDistanceTo(targets[0]) * GetCostPerMove()) };
                    
                    return new (Point pos, long cost)[0];
                }
                else // in room
                {
                    var itemsInTarget = GetTargetPoints().Select(it => existing.FirstOrDefault(p => p.Position.Equals(it))).Where(it => it != null).ToArray();
                    if(itemsInTarget.All(it => it.Letter == Letter) && itemsInTarget.Contains(this)) // sorted, no need to leave again
                        return new (Point pos, long cost)[0];

                    var inHallway = existing.Where(it => it.Position.Y == 0).Count();
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
                switch (Letter)
                {
                    case 'A':
                        return new[] { new Point(2, 1), new Point(2, 2) };
                    case 'B':
                        return new[] { new Point(4, 1), new Point(4, 2) };
                    case 'C':
                        return new[] { new Point(6, 1), new Point(6, 2) };
                    case 'D':
                        return new[] { new Point(8, 1), new Point(8, 2) };
                }

                throw new Exception("Unexpected letter");
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
