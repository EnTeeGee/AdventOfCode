using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2015
{
    class Day14
    {
        [Solution(14, 1)]
        public int Solution1(string input)
        {
            var raindeers = Parser.ToArrayOf(input, it => new Raindeer(it)).ToArray();

            return raindeers.Max(it => it.CaculatePosAtTime(2503));
        }

        [Solution(14, 2)]
        public int Solution2(string input)
        {
            var raindeers = Parser.ToArrayOf(input, it => new Raindeer(it)).ToArray();

            for(var i = 0; i < 2503; i++)
            {
                foreach (var item in raindeers)
                    item.Step();

                var max = raindeers.Max(it => it.CurrentDistance);

                foreach (var raindeer in raindeers.Where(it => it.CurrentDistance == max))
                    raindeer.Points += 1;
            }

            return raindeers.Max(it => it.Points);
        }

        private class Raindeer
        {
            public Raindeer(string input)
            {
                var tokens = Parser.SplitOnSpace(input);
                Name = tokens[0];
                Speed = int.Parse(tokens[3]);
                SpeedDuration = int.Parse(tokens[6]);
                Rest = int.Parse(tokens[13]);
            }

            public string Name { get; }

            public int Speed { get; }

            public int SpeedDuration { get; }

            public int Rest { get; }

            public int CurrentDistance { get; private set; }

            public int Points { get; set; }

            private int currentTime;

            public int CaculatePosAtTime(int time)
            {
                var stepLength = SpeedDuration + Rest;
                var final = time % stepLength;
                var finalDistance = final >= SpeedDuration ? (SpeedDuration * Speed) : (final * Speed);

                return ((time / stepLength) * (SpeedDuration * Speed)) + finalDistance;
            }

            public void Step()
            {
                var stepLength = SpeedDuration + Rest;
                var final = currentTime % stepLength;
                if (final < SpeedDuration)
                    CurrentDistance += Speed;

                ++currentTime;
            }
        }
    }
}
