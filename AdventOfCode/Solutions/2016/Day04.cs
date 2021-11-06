using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2016
{
    class Day04
    {
        [Solution(4, 1)]
        public int Solution1(string input)
        {
            var rooms = Parser.ToArrayOf(input, it => new RoomData(it));

            return rooms.Where(it => it.IsValid).Sum(it => it.SectorId);
        }

        [Solution(4, 2)]
        public int Solution2(string input)
        {
            var rooms = Parser.ToArrayOf(input, it => new RoomData(it));

            return rooms.First(it => it.IsNorthPoleRoom()).SectorId;
        }

        private class RoomData
        {
            public bool IsValid { get; }
            public int SectorId { get; }

            private string[] items;

            public RoomData(string input)
            {
                items = Parser.SplitOn(input, '-', '[', ']');

                var expectedChecksum = new string(items
                    .Take(items.Length - 2)
                    .Aggregate("", (a, b) => a + b)
                    .GroupBy(it => it)
                    .OrderByDescending(it => it.Count())
                    .ThenBy(it => it.Key)
                    .Select(it => it.Key)
                    .Take(5)
                    .ToArray());

                IsValid = expectedChecksum == items.Last();
                SectorId = int.Parse(items[items.Length - 2]);
            }

            public bool IsNorthPoleRoom()
            {
                var decrypted = items.Take(items.Length - 2).Select(it => new string(it.Select(c => DecryptLetter(c)).ToArray())).ToArray();

                return decrypted.Any(it => it == "northpole") && decrypted.Any(it => it == "object");
            }

            private char DecryptLetter(char input)
            {
                return (char)(((((int)(input - 'a')) + SectorId) % 26) + 'a');
            }
        }
    }
}
