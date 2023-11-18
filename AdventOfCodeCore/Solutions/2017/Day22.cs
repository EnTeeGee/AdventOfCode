using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2017
{
    internal class Day22
    {
        [Solution(22, 1)]
        public int Solution1(string input)
        {
            var infected = new HashSet<Point>();
            var lines = Parser.ToArrayOfString(input);
            var ant = new Point((lines[0].Length - 1) / 2, (lines.Length - 1) / 2);
            var dir = Orientation.North;
            var infectedCount = 0;
            var target = 10.Thousand();
            for(var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for(var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        infected.Add(new Point(x, y));
                }
            }

            for(var i = 0; i < target; i++)
            {
                if (infected.Contains(ant))
                {
                    infected.Remove(ant);
                    dir = dir.RotateClockwise();
                    ant = ant.MoveOrient(dir);
                }
                else
                {
                    infected.Add(ant);
                    dir = dir.RotateAntiClock();
                    ant = ant.MoveOrient(dir);
                    infectedCount++;
                }
            }

            return infectedCount;
        }

        [Solution(22, 2)]
        public int Solution2(string input)
        {
            var infected = new Dictionary<Point, State>();
            var lines = Parser.ToArrayOfString(input);
            var ant = new Point((lines[0].Length - 1) / 2, (lines.Length - 1) / 2);
            var dir = Orientation.North;
            var infectedCount = 0;
            var target = 10.Million();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        infected.Add(new Point(x, y), State.Infected);
                }
            }

            for (var i = 0; i < target; i++)
            {
                if (infected.ContainsKey(ant))
                {
                    if (infected[ant] == State.Weakened)
                    {
                        infected[ant] = State.Infected;
                        infectedCount++;
                    } 
                    else if (infected[ant] == State.Infected)
                    {
                        infected[ant] = State.Flagged;
                        dir = dir.RotateClockwise();
                    }
                    else
                    {
                        infected.Remove(ant);
                        dir = dir.Reverse();
                    }
                }
                else
                {
                    infected.Add(ant, State.Weakened);
                    dir = dir.RotateAntiClock();
                }

                ant = ant.MoveOrient(dir);
            }

            return infectedCount;
        }

        private enum State { Weakened, Infected, Flagged };

    }
}
