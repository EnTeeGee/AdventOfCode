using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day07
    {
        [Solution(7, 1)]
        public long Solution1(string input)
        {
            var cards = Parser.ToArrayOf(input, it => new Hand(it)).OrderByDescending(it => it).ToArray();

            return Enumerable.Range(1, cards.Length).Zip(cards, (a, b) => (long)a * b.Bid).Sum();
        }

        [Solution(7, 2)]
        public long Solution2(string input)
        {
            var cards = Parser.ToArrayOf(input, it => new Hand(it, true)).OrderByDescending(it => it).ToArray();

            return Enumerable.Range(1, cards.Length).Zip(cards, (a, b) => (long)a * b.Bid).Sum();
        }

        private class Hand: IComparable<Hand>
        {
            public string Cards { get; }
            public int[] CardValues { get; }
            public int Bid { get; }
            public int HandType { get; }

            public Hand(string input, bool withJoker = false)
            {
                var items = Parser.SplitOnSpace(input);
                Cards = items[0];
                CardValues = Cards.Select(it => withJoker ? JokerCardRanks[it] : CardRanks[it]).ToArray();
                Bid = int.Parse(items[1]);

                if (withJoker && Cards == "JJJJJ")
                {
                    HandType = 0;

                    return;
                }

                var toGroupCards = Cards.Where(it => !withJoker || it != 'J').ToArray();
                var groups = toGroupCards.GroupBy(it => it).OrderByDescending(it => it.Count()).ToArray();
                if(withJoker)
                {
                    toGroupCards = toGroupCards.Concat(Cards.Where(it => it == 'J').Select(it => groups[0].Key)).ToArray();
                    groups = toGroupCards.GroupBy(it => it).OrderByDescending(it => it.Count()).ToArray();
                }

                if (groups.Length == 1)
                    HandType = 0;
                else if (groups.Length == 2 && groups[0].Count() == 4)
                    HandType = 1;
                else if (groups.Length == 2)
                    HandType = 2;
                else if (groups.Length == 3 && groups[0].Count() == 3)
                    HandType = 3;
                else if (groups.Length == 3)
                    HandType = 4;
                else if (groups.Length == 4)
                    HandType = 5;
                else
                    HandType = 6;
            }

            private static Dictionary<char, int> CardRanks = new Dictionary<char, int>
            {
                { 'A', 0 },
                { 'K', 1 },
                { 'Q', 2 },
                { 'J', 3 },
                { 'T', 4 },
                { '9', 5 },
                { '8', 6 },
                { '7', 7 },
                { '6', 8 },
                { '5', 9 },
                { '4', 10 },
                { '3', 11 },
                { '2', 12 }
            };

            private static Dictionary<char, int> JokerCardRanks = new Dictionary<char, int>
            {
                { 'A', 0 },
                { 'K', 1 },
                { 'Q', 2 },
                { 'T', 3 },
                { '9', 4 },
                { '8', 5 },
                { '7', 6 },
                { '6', 7 },
                { '5', 8 },
                { '4', 9 },
                { '3', 10 },
                { '2', 11 },
                { 'J', 12 }
            };

            public int CompareTo(Hand? toCompare)
            {
                if (toCompare == null)
                    return 0;

                if (HandType != toCompare.HandType)
                    return HandType - toCompare.HandType;

                return CardValues.Zip(toCompare.CardValues, (a, b) => a - b).FirstOrDefault(it => it != 0);
            }
        }
    }
}
