using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions._2015
{
    class Day21
    {
        private static Item[] weapons =
        {
            new Item(8, 4, 0),
            new Item(10, 5, 0),
            new Item(25, 6, 0),
            new Item(40, 7, 0),
            new Item(74, 8, 0)
        };

        private static Item[] armour =
        {
            new Item(0, 0, 0),
            new Item(13, 0, 1),
            new Item(31, 0, 2),
            new Item(53, 0, 3),
            new Item(75, 0, 4),
            new Item(102, 0, 5)
        };

        private static Item[] rings =
        {
            new Item(0, 0, 0),
            new Item(0, 0, 0),
            new Item(25, 1, 0),
            new Item(50, 2, 0),
            new Item(100, 3, 0),
            new Item(20, 0, 1),
            new Item(40, 0, 2),
            new Item(80, 0, 3)
        };

        private const int PlayerHitPoints = 100;

        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var bossValues = Parser
                .ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .Select(it => int.Parse(it.Last()))
                .ToArray();
            var bossDamage = bossValues[1];
            var bossArmour = bossValues[2];

            var bestDeal = int.MaxValue;

            foreach(var weapon in weapons)
            {
                foreach(var cover in armour)
                {
                    for(var i = 1; i < rings.Length; i++)
                    {
                        for(var j = 0; j < i; j++)
                        {
                            var bossHp = bossValues[0];
                            var totalCost = weapon.Cost + cover.Cost + rings[i].Cost + rings[j].Cost;
                            if (totalCost >= bestDeal)
                                continue;

                            var playerArmour = cover.Armour + rings[i].Armour + rings[j].Armour;
                            var playerDamage = weapon.Damage + rings[i].Damage + rings[j].Damage;
                            var bossDamagePerTurn = Math.Max(1, bossDamage - playerArmour);
                            var playerDamagePerTurn = Math.Max(1, playerDamage - bossArmour);
                            var turnsForBossDeath = (int)Math.Ceiling(bossHp / (double)playerDamagePerTurn);
                            var turnsForPlayerDeath = (int)Math.Ceiling(PlayerHitPoints / (double)bossDamagePerTurn);

                            if (turnsForPlayerDeath < turnsForBossDeath)
                                continue;

                            bestDeal = totalCost;
                        }
                    }
                }
            }

            return bestDeal;
        }

        [Solution(21, 2)]
        public int Solution2(string input)
        {
            var bossValues = Parser
                .ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .Select(it => int.Parse(it.Last()))
                .ToArray();
            var bossDamage = bossValues[1];
            var bossArmour = bossValues[2];

            var worstDeal = int.MinValue;

            foreach (var weapon in weapons)
            {
                foreach (var cover in armour)
                {
                    for (var i = 1; i < rings.Length; i++)
                    {
                        for (var j = 0; j < i; j++)
                        {
                            var bossHp = bossValues[0];
                            var totalCost = weapon.Cost + cover.Cost + rings[i].Cost + rings[j].Cost;
                            if (totalCost <= worstDeal)
                                continue;

                            var playerArmour = cover.Armour + rings[i].Armour + rings[j].Armour;
                            var playerDamage = weapon.Damage + rings[i].Damage + rings[j].Damage;
                            var bossDamagePerTurn = Math.Max(1, bossDamage - playerArmour);
                            var playerDamagePerTurn = Math.Max(1, playerDamage - bossArmour);
                            var turnsForBossDeath = (int)Math.Ceiling(bossHp / (double)playerDamagePerTurn);
                            var turnsForPlayerDeath = (int)Math.Ceiling(PlayerHitPoints / (double)bossDamagePerTurn);

                            if (turnsForPlayerDeath >= turnsForBossDeath)
                                continue;

                            worstDeal = totalCost;
                        }
                    }
                }
            }

            return worstDeal;
        }

        private class Item
        {
            public int Cost { get; }
            public int Damage { get; }
            public int Armour { get; }

            public Item(int cost, int damage, int armour)
            {
                Cost = cost;
                Damage = damage;
                Armour = armour;
            }
        }
    }
}
