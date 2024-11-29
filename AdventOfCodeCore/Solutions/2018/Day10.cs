using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day10
    {
        [Solution(10, 1)]
        public string Solution1(string input)
        {
            var lights = Parser.ToArrayOf(input, it => new Light(it));

            var highest = lights.Max(it => it.X);
            var seconds = 0;

            while (true)
            {
                foreach (var item in lights)
                    item.StepForward();


                var newHighest = lights.Max(it => it.X);
                if (newHighest > highest)
                {
                    foreach (var item in lights)
                        item.StepBack();
                    return DumpLights(lights, seconds);
                }

                highest = newHighest;
                ++seconds;
            }
        }

        [Solution(10, 2)]
        public int Solution2(string input)
        {
            var lights = Parser.ToArrayOf(input, it => new Light(it));

            var highest = lights.Max(it => it.X);
            var seconds = 0;

            while (true)
            {
                foreach (var item in lights)
                    item.StepForward();


                var newHighest = lights.Max(it => it.X);
                if (newHighest > highest)
                {
                    foreach (var item in lights)
                        item.StepBack();
                    return seconds;
                }

                highest = newHighest;
                ++seconds;
            }
        }

        private string DumpLights(Light[] lights, int seconds)
        {
            var minX = lights.Min(it => it.X);
            var minY = lights.Min(it => it.Y);
            var maxX = lights.Max(it => it.X);
            var maxY = lights.Max(it => it.Y);
            var output = new StringBuilder();

            for (var i = minY; i <= maxY; i++)
            {
                for (var j = minX; j <= maxX; j++)
                {
                    if (lights.Any(it => it.Y == i && it.X == j))
                        output.Append('#');
                    else
                        output.Append('.');
                }

                output.Append('\n');
            }

            output.Append($"Total seconds: {seconds}");

            return output.ToString();
        }

        private class Light
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int MoveX { get; set; }
            public int MoveY { get; set; }

            public Light(string input)
            {
                var sections = Parser.SplitOn(input, '<', '>', ' ', ',');
                X = int.Parse(sections[1]);
                Y = int.Parse(sections[2]);
                MoveX = int.Parse(sections[4]);
                MoveY = int.Parse(sections[5]);
            }

            public void StepForward()
            {
                X += MoveX;
                Y += MoveY;
            }

            public void StepBack()
            {
                X -= MoveX;
                Y -= MoveY;
            }
        }
    }
}
