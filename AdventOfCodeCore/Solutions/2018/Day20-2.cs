﻿using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    class Day20_2
    {
        [Solution(20, 2)]
        public int Solution2(string input)
        {
            var startingRoom = new Room(0, 0);
            startingRoom.Dist = 0;
            var seenRooms = new Dictionary<long, Room>();
            seenRooms.Add(startingRoom.GetPos(), startingRoom);
            var path = new LinkedList<char>();

            foreach (var item in input)
            {
                if (item == '^' || item == '$')
                    continue;
                path.AddLast(item);
            }

            var result = FollowRoute(seenRooms, path.First!, startingRoom, 0);

            var test = seenRooms.Values.Max(it => it.Dist);
            Console.WriteLine($"for solution 2 part 1: {test}");

            //return seenRooms.Values.Max(it => it.Dist);
            return seenRooms.Values.Where(it => it.Dist >= 1000).Count();
        }

        private LinkedListNode<char>? FollowRoute(Dictionary<long, Room> seenRooms, LinkedListNode<char> path, Room room, int dist)
        {
            var currentPath = path;
            var currentRoom = room;
            var currentDist = dist;

            while (true)
            {
                if (currentPath == null)
                    return null;
                if ("NSEW".Contains(currentPath.Value))
                {
                    Room? newRoom;
                    if (currentPath.Value == 'N')
                    {
                        if (currentRoom.North != null)
                            newRoom = currentRoom.North;
                        else
                        {
                            newRoom = GetRoom(currentRoom.Pos.X, currentRoom.Pos.Y - 1, seenRooms);
                            newRoom.South = currentRoom;
                            currentRoom.North = newRoom;
                        }
                    }
                    else if (currentPath.Value == 'E')
                    {
                        if (currentRoom.East != null)
                            newRoom = currentRoom.East;
                        else
                        {
                            newRoom = GetRoom(currentRoom.Pos.X + 1, currentRoom.Pos.Y, seenRooms);
                            newRoom.West = currentRoom;
                            currentRoom.East = newRoom;
                        }
                    }
                    else if (currentPath.Value == 'S')
                    {
                        if (currentRoom.South != null)
                            newRoom = currentRoom.South;
                        else
                        {
                            newRoom = GetRoom(currentRoom.Pos.X, currentRoom.Pos.Y + 1, seenRooms);
                            newRoom.North = currentRoom;
                            currentRoom.South = newRoom;
                        }
                    }
                    else // West
                    {
                        if (currentRoom.West != null)
                            newRoom = currentRoom.West;
                        else
                        {
                            newRoom = GetRoom(currentRoom.Pos.X - 1, currentRoom.Pos.Y, seenRooms);
                            newRoom.East = currentRoom;
                            currentRoom.West = newRoom;
                        }
                    }

                    currentDist++;
                    if (newRoom.Dist > currentDist)
                        newRoom.Dist = currentDist;
                    else
                        currentDist = newRoom.Dist;


                    currentPath = currentPath.Next;
                    currentRoom = newRoom;
                    continue;
                }
                else if (currentPath.Value == '(')
                {
                    currentPath = FollowRoute(seenRooms, currentPath.Next!, currentRoom, currentDist);
                }
                else if (currentPath.Value == '|')
                {
                    currentDist = dist;
                    currentRoom = room;
                    currentPath = currentPath.Next;
                }
                else if (currentPath.Value == ')')
                {
                    return currentPath.Next;
                }
                else
                {
                    currentPath = currentPath.Next;
                }
            }
        }

        private Room GetRoom(int x, int y, Dictionary<long, Room> seenRooms)
        {
            var newRoom = new Room(x, y);
            var key = newRoom.GetPos();
            var matching = seenRooms.ContainsKey(key);

            if (matching)
                return seenRooms[key];

            seenRooms.Add(key, newRoom);

            return newRoom;
        }

        private class Room
        {
            public int Dist { get; set; }
            public Pos Pos { get; set; }
            public Room? North { get; set; }
            public Room? East { get; set; }
            public Room? South { get; set; }
            public Room? West { get; set; }

            public Room(int x, int y)
            {
                this.Pos = new Pos
                {
                    X = x,
                    Y = y
                };
                Dist = int.MaxValue;
            }

            public long GetPos()
            {
                long res = Pos.X;
                res <<= 32;
                res |= (long)(uint)Pos.Y;

                return res;
            }
        }

        private class PosInfo
        {
            public Room? CurrentRoom { get; set; }
            public LinkedListNode<char>? CurrentPath { get; set; }
            public int CurrentDist { get; set; }
        }

        private struct Pos
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
