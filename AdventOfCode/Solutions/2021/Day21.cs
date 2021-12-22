using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day21
    {
        [Solution(21, 1)]
        public int Solution1(string input)
        {
            var info = Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it)).ToArray();
            var die = new Die();
            var player1Score = 0;
            var player1Index = int.Parse(info[0][4]) - 1;
            var player2Score = 0;
            var player2Index = int.Parse(info[1][4]) - 1;

            while(true)
            {
                var roll = die.GetNextRolls();
                player1Index = (player1Index + roll) % 10;
                player1Score += (player1Index + 1);

                if (player1Score >= 1000)
                    return player2Score * die.GetTotalRolls();

                roll = die.GetNextRolls();
                player2Index = (player2Index + roll) % 10;
                player2Score += (player2Index + 1);

                if (player2Score >= 1000)
                    return player1Score * die.GetTotalRolls();
            }
        }

        [Solution(21, 2)]
        public long Solution2(string input)
        {
            var info = Parser.ToArrayOfString(input).Select(it => Parser.SplitOnSpace(it)).ToArray();
            var remainingStates = new Dictionary<GameState, long>();
            remainingStates.Add(new GameState(0, int.Parse(info[0][4]) - 1, 0, int.Parse(info[1][4]) - 1, true), 1);
            var player1Wins = 0L;
            var player2Wins = 0L;

            while(remainingStates.Any())
            {
                var state = remainingStates.First();
                remainingStates.Remove(state.Key);
                var newStates = state.Key.GetVariants();

                foreach(var item in newStates)
                {
                    if (item.Player1Score >= 21)
                        player1Wins += state.Value;
                    else if (item.Player2Score >= 21)
                        player2Wins += state.Value;
                    else if (remainingStates.ContainsKey(item))
                        remainingStates[item] += state.Value;
                    else
                        remainingStates[item] = state.Value;
                }
            }

            return Math.Max(player1Wins, player2Wins);
        }

        private class Die
        {
            private int nextValue;
            private int rolls;

            public Die()
            {
                nextValue = 1;
            }

            public int GetNextRolls()
            {
                var values = Enumerable.Range(nextValue - 1, 3).Select(it => (it % 100) + 1).ToArray();
                nextValue = ((nextValue + 2) % 100) + 1;
                rolls += 3;

                return values.Sum();
            }

            public int GetTotalRolls()
            {
                return rolls;
            }
        }

        private class GameState
        {
            public int Player1Score { get; }
            public int Player1Index { get; }
            public int Player2Score { get; }
            public int Player2Index { get; }
            public bool Player1Turn { get; }

            public GameState(int player1Score, int player1Index, int player2Score, int player2Index, bool player1Turn)
            {
                Player1Score = player1Score;
                Player1Index = player1Index;
                Player2Score = player2Score;
                Player2Index = player2Index;
                Player1Turn = player1Turn;
            }

            public GameState[] GetVariants()
            {
                var rolls = Enumerable.Range(1, 3).ToArray();
                var fullRolls = rolls.SelectMany(it => rolls.Select(r => r + it)).SelectMany(it => rolls.Select(r => r + it)).ToArray();
                return fullRolls.Select(it => MoveForwardBy(it)).ToArray();
            }

            public GameState MoveForwardBy(int roll)
            {
                var newIndex = ((Player1Turn ? Player1Index : Player2Index) + roll) % 10;
                var newScore = (Player1Turn ? Player1Score : Player2Score) + newIndex + 1;

                return new GameState(
                    Player1Turn ? newScore : Player1Score,
                    Player1Turn ? newIndex : Player1Index,
                    !Player1Turn ? newScore : Player2Score,
                    !Player1Turn ? newIndex : Player2Index,
                    !Player1Turn);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is GameState cast))
                    return false;

                return cast.Player1Score == Player1Score
                    && cast.Player1Index == Player1Index
                    && cast.Player2Score == Player2Score 
                    && cast.Player2Index == Player2Index
                    && cast.Player1Turn == Player1Turn;
            }

            public override int GetHashCode()
            {
                return Player1Score ^ Player1Index ^ Player2Score ^ Player2Index;
            }
        }
    }
}
