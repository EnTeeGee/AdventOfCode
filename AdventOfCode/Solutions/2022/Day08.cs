using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var trees = Parser.ToArrayOfString(input)
                .Select(it => it.Select(t => int.Parse(t.ToString())).ToArray())
                .ToArray();

            var visible = 0;
            for(var y = 0; y < trees.Length; y++)
            {
                for(var x = 0; x < trees[0].Length; x++)
                {
                    var tree = trees[y][x];

                    if (Enumerable.Range(0, y).All(it => trees[it][x] < tree)
                        || Enumerable.Range(y + 1, trees.Length - y - 1).All(it => trees[it][x] < tree)
                        || Enumerable.Range(0, x).All(it => trees[y][it] < tree)
                        || Enumerable.Range(x + 1, trees[0].Length - x - 1).All(it => trees[y][it] < tree))
                        visible += 1;
                }
            }

            return visible;
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            var trees = Parser.ToArrayOfString(input)
                .Select(it => it.Select(t => int.Parse(t.ToString())).ToArray())
                .ToArray();

            for(var i = 0; i < trees.Length; i++)
            {
                trees[i][0] = 10;
                trees[i][trees[i].Length - 1] = 10;
            }
            for(var i = 0; i < trees[0].Length; i++)
            {
                trees[0][i] = 10;
                trees[trees.Length - 1][i] = 10;
            }

            var best = 0;

            for (var y = 1; y < trees.Length - 1; y++)
            {
                for (var x = 1; x < trees[0].Length - 1; x++)
                {
                    var tree = trees[y][x];
                    var score = (Enumerable.Range(0, y).Reverse().Select(it => trees[it][x]).TakeWhile(it => it < tree).Count() + 1)
                        * (Enumerable.Range(y + 1, trees.Length - y - 1).Select(it => trees[it][x]).TakeWhile(it => it < tree).Count() + 1)
                        * (Enumerable.Range(0, x).Reverse().Select(it => trees[y][it]).TakeWhile(it => it < tree).Count() + 1)
                        * (Enumerable.Range(x + 1, trees[0].Length - x - 1).Select(it => trees[y][it]).TakeWhile(it => it < tree).Count() + 1);

                    if (score > best)
                        best = score;
                }
            }

            return best;
        }
    }
}
