using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day24
    {
        [Solution(24, 1)]
        public int Solution1(string input)
        {
            var groups = Parser.ToArrayOfGroups(input);
            var immuneSystem = Parser.ToArrayOfString(groups[0]).Skip(1).Select(it => new Unit(it, false)).ToList();
            var infection = Parser.ToArrayOfString(groups[1]).Skip(1).Select(it => new Unit(it, true)).ToList();

            while(immuneSystem.Any() && infection.Any())
            {
                var targetOrder = immuneSystem.Concat(infection).OrderByDescending(it => it.EffectivePower).ThenByDescending(it => it.Initiative).ToArray();
                var attackTargets = targetOrder.ToList();
                var attacks = new List<(Unit attacker, Unit target, int amount, int initiative)>();

                foreach(var item in targetOrder)
                {
                    var toAttack = attackTargets
                        .Where(it => it.IsInfection != item.IsInfection)
                        .OrderByDescending(it => it.GetDamageFrom(item.EffectivePower, item.DamageType))
                        .ThenByDescending(it => it.EffectivePower)
                        .ThenByDescending(it => it.Initiative)
                        .FirstOrDefault();

                    if(toAttack != null && toAttack.GetDamageFrom(item.EffectivePower, item.DamageType) != 0)
                    {
                        attacks.Add((item, toAttack, toAttack.GetDamageFrom(item.EffectivePower, item.DamageType), item.Initiative));
                        attackTargets.Remove(toAttack);
                    }
                }

                attacks = attacks.OrderByDescending(it => it.initiative).ToList();

                foreach(var attack in attacks)
                {
                    attack.target.ApplyDamage(attack.target.GetDamageFrom(attack.attacker.EffectivePower, attack.attacker.DamageType));
                    if(attack.target.Size == 0)
                        (attack.target.IsInfection ? infection : immuneSystem).Remove(attack.target);
                }
            }

            return immuneSystem.Concat(infection).Sum(it => it.Size);
        }

        // 5411 too low
        [Solution(24, 2)]
        public int Solution2(string input)
        {
            var groups = Parser.ToArrayOfGroups(input);
            var tooLow = 0;
            var tooHigh = 2000;

            while (true)
            {
                var boost = (tooLow + tooHigh) / 2;

                var immuneSystem = Parser.ToArrayOfString(groups[0]).Skip(1).Select(it => new Unit(it, false)).ToList();
                var infection = Parser.ToArrayOfString(groups[1]).Skip(1).Select(it => new Unit(it, true)).ToList();

                var result = RunWithBoost(immuneSystem, infection, boost);
                if (result > 0)
                    tooHigh = boost;
                else
                    tooLow = boost;

                if (tooHigh - tooLow == 1)
                {
                    immuneSystem = Parser.ToArrayOfString(groups[0]).Skip(1).Select(it => new Unit(it, false)).ToList();
                    infection = Parser.ToArrayOfString(groups[1]).Skip(1).Select(it => new Unit(it, true)).ToList();
                    return RunWithBoost(immuneSystem, infection, tooHigh);
                }

            }
        }

        private int RunWithBoost(List<Unit> immuneSystem, List<Unit> infection, int boost)
        {
            foreach (var item in immuneSystem)
                item.ApplyBoost(boost);

            while (immuneSystem.Any() && infection.Any())
            {
                var targetOrder = immuneSystem.Concat(infection).OrderByDescending(it => it.EffectivePower).ThenByDescending(it => it.Initiative).ToArray();
                var attackTargets = targetOrder.ToList();
                var attacks = new List<(Unit attacker, Unit target, int amount, int initiative)>();

                foreach (var item in targetOrder)
                {
                    var toAttack = attackTargets
                        .Where(it => it.IsInfection != item.IsInfection)
                        .OrderByDescending(it => it.GetDamageFrom(item.EffectivePower, item.DamageType))
                        .ThenByDescending(it => it.EffectivePower)
                        .ThenByDescending(it => it.Initiative)
                        .FirstOrDefault();

                    if (toAttack != null && toAttack.GetDamageFrom(item.EffectivePower, item.DamageType) != 0)
                    {
                        attacks.Add((item, toAttack, toAttack.GetDamageFrom(item.EffectivePower, item.DamageType), item.Initiative));
                        attackTargets.Remove(toAttack);
                    }
                }

                attacks = attacks.OrderByDescending(it => it.initiative).ToList();
                var totalKills = 0;

                foreach (var attack in attacks)
                {
                    totalKills += attack.target.ApplyDamage(attack.target.GetDamageFrom(attack.attacker.EffectivePower, attack.attacker.DamageType));
                    if (attack.target.Size == 0)
                        (attack.target.IsInfection ? infection : immuneSystem).Remove(attack.target);
                }

                if (totalKills == 0) // Stalemate
                    return 0;
            }

            return immuneSystem.Sum(it => it.Size);
        }

        private class Unit
        {
            public int Size { get; private set; }
            public int Hp { get; }
            public string[] WeakTo { get; }
            public string[] ImmuneTo { get; }
            public int Damage { get; private set; }
            public string DamageType { get; }
            public int Initiative { get; }
            public bool IsInfection { get; }

            public int EffectivePower { get { return Size * Damage; } }

            public Unit(string input, bool isInfection)
            {
                var baseChunks = Parser.SplitOn(input, "units each with ", " hit points", " with an attack that does ", " damage at initiative");
                Size = int.Parse(baseChunks[0]);
                Hp = int.Parse(baseChunks[1]);
                var damageChunks = Parser.SplitOnSpace(baseChunks[^2]);
                Damage = int.Parse(damageChunks[0]);
                DamageType = damageChunks[1];
                Initiative = int.Parse(baseChunks[^1]);

                if (baseChunks.Length == 5)
                {
                    var modChunks = Parser.SplitOn(baseChunks[2].Trim(), ';', ')', '(').Select(it => it.Trim()).ToArray();
                    if (modChunks.Any(it => it.StartsWith("weak")))
                        WeakTo = Parser.SplitOn(modChunks.First(it => it.StartsWith("weak")), ' ', ',').Skip(2).ToArray();
                    else
                        WeakTo = Array.Empty<string>();

                    if (modChunks.Any(it => it.StartsWith("immune")))
                        ImmuneTo = Parser.SplitOn(modChunks.First(it => it.StartsWith("immune")), ' ', ',').Skip(2).ToArray();
                    else
                        ImmuneTo = Array.Empty<string>();
                }
                else
                {
                    WeakTo = Array.Empty<string>();
                    ImmuneTo = Array.Empty<string>();
                }
                IsInfection = isInfection;
            }

            public int GetDamageFrom(int attackPower, string damageType)
            {
                if (ImmuneTo.Contains(damageType))
                    return 0;
                else if (WeakTo.Contains(damageType))
                    return attackPower * 2;

                return attackPower;
            }

            public int ApplyDamage(int toApply)
            {
                var toRemove = (toApply / Hp);

                Size -= toRemove;
                if (Size < 0)
                    Size = 0;

                return toRemove;
            }

            public void ApplyBoost(int boost)
            {
                Damage += boost;
            }
        }
    }
}
