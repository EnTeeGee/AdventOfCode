using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2015
{
    internal class Day22
    {
        private const int PlayerStartHp = 50;
        private const int PlayerStartMp = 500;

        private static class Spells
        {
            public static class MagicMissile
            {
                public const int ManaCost = 53;
                public const int Damage = 4;
            }
            public static class Drain
            {
                public const int ManaCost = 73;
                public const int Damage = 2;
                public const int Recharge = 2;
            }
            public static class Shield
            {
                public const int ManaCost = 113;
                public const int Duration = 6;
                public const int Defence = 7;
            }
            public static class Poison
            {
                public const int ManaCost = 173;
                public const int Duration = 6;
                public const int Damage = 3;
            }
            public static class Recharge
            {
                public const int ManaCost = 229;
                public const int Duration = 5;
                public const int Amount = 101;
            }
        }

        [Solution(22, 1)]
        public int Solution1(string input)
        {
            var bossValues = Parser
                .ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .Select(it => int.Parse(it.Last()))
                .ToArray();

            var boundary = new PriorityQueue<State>((a, b) => a.SpentMana - b.SpentMana);
            boundary.Insert(new State(PlayerStartHp, PlayerStartMp, bossValues[0], 0, 0, 0, 0));
            var equater = new Func<State, State, bool>((a, b) => a.IsSame(b));

            var minSpentMana = int.MaxValue;

            while (boundary.Any())
            {
                var current = boundary.Pop()!;
                if (current.SpentMana >= minSpentMana)
                    return minSpentMana;

                // apply effects to player and boss
                current = ApplyEffects(current);
                // check if boss is dead, check with min and continue if true
                if (current.BossHp <= 0)
                {
                    if (current.SpentMana < minSpentMana)
                        minSpentMana = current.SpentMana;

                    continue;
                }
                // go through each spell
                var newCurrents = new List<State?>();
                newCurrents.Add(ApplyMagicMissile(current));
                newCurrents.Add(ApplyDrain(current));
                newCurrents.Add(ApplyShield(current));
                newCurrents.Add(ApplyPosion(current));
                newCurrents.Add(ApplyRecharge(current));

                // if it defeats the boss, check and posibly update minSpentMana
                // apply effect to player + boss again and check
                newCurrents = newCurrents.Where(it => it != null).Select(it => it != null ? ApplyEffects(it) : null).ToList();
                var defeatedBoss = newCurrents.Where(it => it != null && it.BossHp <= 0).ToList();
                foreach (var item in defeatedBoss)
                {
                    if (item != null && item.SpentMana < minSpentMana)
                        minSpentMana = item.SpentMana;
                }
                newCurrents = newCurrents.Where(it => it != null && it.BossHp > 0).ToList();

                // perform boss move, If it defeats player, continue
                newCurrents = newCurrents.Select(it => it != null ? ApplyBossAttack(it, bossValues[1]) : null).ToList();
                newCurrents = newCurrents.Where(it => it != null && it.Hp > 0).ToList();
                // otherwise add to the boundary
                foreach (var item in newCurrents)
                    if(item != null)
                        boundary.InsertIfUnique(item, equater);
            }

            return minSpentMana;
        }

        [Solution(22, 2)]
        public int Solution2(string input)
        {
            var bossValues = Parser
                .ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it))
                .Select(it => int.Parse(it.Last()))
                .ToArray();

            var boundary = new PriorityQueue<State>((a, b) => a.SpentMana - b.SpentMana);
            boundary.Insert(new State(PlayerStartHp, PlayerStartMp, bossValues[0], 0, 0, 0, 0));
            var equater = new Func<State, State, bool>((a, b) => a.IsSame(b));

            var minSpentMana = int.MaxValue;

            while (boundary.Any())
            {
                var current = boundary.Pop()!;
                if (current.SpentMana >= minSpentMana)
                    return minSpentMana;

                // Apply hard mode
                current = ApplyHardMode(current);
                if (current.Hp <= 0)
                    continue;
                // apply effects to player and boss
                current = ApplyEffects(current);
                // check if boss is dead, check with min and continue if true
                if (current.BossHp <= 0)
                {
                    if (current.SpentMana < minSpentMana)
                        minSpentMana = current.SpentMana;

                    continue;
                }
                // go through each spell
                var newCurrents = new List<State?>();
                newCurrents.Add(ApplyMagicMissile(current));
                newCurrents.Add(ApplyDrain(current));
                newCurrents.Add(ApplyShield(current));
                newCurrents.Add(ApplyPosion(current));
                newCurrents.Add(ApplyRecharge(current));

                // if it defeats the boss, check and posibly update minSpentMana
                // apply effect to player + boss again and check
                newCurrents = newCurrents.Where(it => it != null).Select(it => it != null ? ApplyEffects(it) : null).ToList();
                var defeatedBoss = newCurrents.Where(it => it != null && it.BossHp <= 0).ToList();
                foreach (var item in defeatedBoss)
                {
                    if (item != null && item.SpentMana < minSpentMana)
                        minSpentMana = item.SpentMana;
                }
                newCurrents = newCurrents.Where(it => it != null && it.BossHp > 0).ToList();

                // perform boss move, If it defeats player, continue
                newCurrents = newCurrents.Select(it => it != null ? ApplyBossAttack(it, bossValues[1]) : null).ToList();
                newCurrents = newCurrents.Where(it => it != null && it.Hp > 0).ToList();
                // otherwise add to the boundary
                foreach (var item in newCurrents)
                    if(item != null)
                        boundary.InsertIfUnique(item, equater);
            }

            return minSpentMana;
        }

        private State ApplyEffects(State current)
        {
            var newMp = current.RechargeTurns > 0 ? current.Mp + Spells.Recharge.Amount : current.Mp;
            var newBossHp = current.PoisonTurns > 0 ? current.BossHp - Spells.Poison.Damage : current.BossHp;

            return new State(
                current.Hp,
                newMp,
                newBossHp,
                Math.Max(0, current.ShieldTurns - 1),
                Math.Max(0, current.PoisonTurns - 1),
                Math.Max(0, current.RechargeTurns - 1),
                current.SpentMana);
        }

        // methods of each spell
        private State? ApplyMagicMissile(State current)
        {
            if (current.Mp < Spells.MagicMissile.ManaCost)
                return null;

            return new State(
                current.Hp,
                current.Mp - Spells.MagicMissile.ManaCost,
                current.BossHp - Spells.MagicMissile.Damage,
                current.ShieldTurns,
                current.PoisonTurns,
                current.RechargeTurns,
                current.SpentMana + Spells.MagicMissile.ManaCost);
        }

        private State? ApplyDrain(State current)
        {
            if (current.Mp < Spells.Drain.ManaCost)
                return null;

            return new State(
                current.Hp + Spells.Drain.Recharge,
                current.Mp - Spells.Drain.ManaCost,
                current.BossHp - Spells.Drain.Damage,
                current.ShieldTurns,
                current.PoisonTurns,
                current.RechargeTurns,
                current.SpentMana + Spells.Drain.ManaCost);
        }

        private State? ApplyShield(State current)
        {
            if (current.Mp < Spells.Shield.ManaCost || current.ShieldTurns > 0)
                return null;

            return new State(
                current.Hp,
                current.Mp - Spells.Shield.ManaCost,
                current.BossHp,
                Spells.Shield.Duration,
                current.PoisonTurns,
                current.RechargeTurns,
                current.SpentMana + Spells.Shield.ManaCost);
        }

        private State? ApplyPosion(State current)
        {
            if (current.Mp < Spells.Poison.ManaCost || current.PoisonTurns > 0)
                return null;

            return new State(
                current.Hp,
                current.Mp - Spells.Poison.ManaCost,
                current.BossHp,
                current.ShieldTurns,
                Spells.Poison.Duration,
                current.RechargeTurns,
                current.SpentMana + Spells.Poison.ManaCost);
        }

        private State? ApplyRecharge(State current)
        {
            if (current.Mp < Spells.Recharge.ManaCost || current.RechargeTurns > 0)
                return null;

            return new State(
                current.Hp,
                current.Mp - Spells.Recharge.ManaCost,
                current.BossHp,
                current.ShieldTurns,
                current.PoisonTurns,
                Spells.Recharge.Duration,
                current.SpentMana + Spells.Recharge.ManaCost);
        }

        private State ApplyBossAttack(State current, int bossAttack)
        {
            var damage = current.ShieldTurns > 0 ? Math.Max(1, bossAttack - Spells.Shield.Defence) : bossAttack;

            return new State(
                current.Hp - damage,
                current.Mp,
                current.BossHp,
                current.ShieldTurns,
                current.PoisonTurns,
                current.RechargeTurns,
                current.SpentMana);
        }

        private State ApplyHardMode(State current)
        {
            return new State(
                current.Hp - 1,
                current.Mp,
                current.BossHp,
                current.ShieldTurns,
                current.PoisonTurns,
                current.RechargeTurns,
                current.SpentMana);
        }

        private class State
        {
            public int Hp { get; }
            public int Mp { get; }
            public int BossHp { get; }
            public int ShieldTurns { get; }
            public int PoisonTurns { get; }
            public int RechargeTurns { get; }

            public int SpentMana { get; }

            public State(int hp, int mp, int bossHp, int shieldTurns, int poisonTurns, int rechargeTurns, int spentMana)
            {
                Hp = hp;
                Mp = mp;
                BossHp = bossHp;
                ShieldTurns = shieldTurns;
                PoisonTurns = poisonTurns;
                RechargeTurns = rechargeTurns;
                SpentMana = spentMana;
            }

            public bool IsSame(State input)
            {
                return Hp == input.Hp
                    && Mp == input.Mp
                    && BossHp == input.BossHp
                    && ShieldTurns == input.ShieldTurns
                    && PoisonTurns == input.PoisonTurns
                    && RechargeTurns == input.RechargeTurns
                    && SpentMana == input.SpentMana;
            }
        }
    }
}
