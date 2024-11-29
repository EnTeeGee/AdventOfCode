using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2018
{
    internal class Day03
    {
        [Solution(3, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOf(input, it => new Claim(it))
                .SelectMany(it => Enumerable.Range(it.Left, it.Width).Select(p => new { X = p, Info = it }))
                .SelectMany(it => Enumerable.Range(it.Info.Top, it.Info.Height).Select(p => new { it.X, Y = p }))
                .GroupBy(it => it)
                .Where(it => it.Count() > 1)
                .Count();
        }

        [Solution(3, 2)]
        public int Solution2(string input)
        {
            var claims = Parser.ToArrayOf(input, it => new Claim(it));

            for (var i = 0; i < claims.Length; i++)
            {
                var overlaps = false;

                for (var j = 0; j < claims.Length; j++)
                {
                    if (i == j)
                        continue;

                    if (!Overlaps(claims[i], claims[j]))
                        continue;

                    overlaps = true;
                    break;
                }

                if (!overlaps)
                    return claims[i].Id;
            }

            throw new Exception("Found no clear claim");
        }

        private bool Overlaps(Claim a, Claim b)
        {
            return !(a.Left + a.Width < b.Left
                || b.Left + b.Width <= a.Left
                || a.Top + a.Height < b.Top
                || b.Top + b.Height <= a.Top);
        }

        private class Claim
        {
            public int Id { get; }
            public int Left { get; }
            public int Top { get; }
            public int Width { get; }
            public int Height { get; }

            public Claim(string input)
            {
                var items = Parser.SplitOn(input, '#', ' ', '@', ',', ':', 'x');
                Id = int.Parse(items[0]);
                Left = int.Parse(items[1]);
                Top = int.Parse(items[2]);
                Width = int.Parse(items[3]);
                Height = int.Parse(items[4]);
            }
        }
    }
}
