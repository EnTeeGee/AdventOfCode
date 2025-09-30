using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeCore.Solutions._2019
{
    internal class Day22
    {
        const int shortLength = 10007;

        [Solution(22, 1)]
        public string Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var limit = new BigInteger(10007);
            var pos = new BigInteger(2019);
            //var pos = new BigInteger(2604);
            //var pos = new BigInteger(0);
            //var pos = BigInteger.Parse("42458351019165779973993866656084876509930441619403829179985557001786");

            var test = NaiveStep(2019, shortLength, lines);

            return test.ToString();

            foreach(var line in lines)
            {
                if (line == "deal into new stack")
                {
                    pos *= -1;
                    pos -= 1;
                }
                else if (line.StartsWith("cut"))
                {
                    var shift = int.Parse(Parser.SplitOnSpace(line)[1]);
                    pos += (-shift);
                }
                else
                {
                    var shift = int.Parse(Parser.SplitOnSpace(line)[3]);
                    pos *= shift;
                }
            }

            //return (pos % limit).ToString();
            return $"{(pos % limit).ToString()} ({pos})";
        }

        // https://www.reddit.com/r/adventofcode/comments/etxg0o/how_do_i_solve_day_22_part_2_2019_x_times_in_a_row/
        // https://www.reddit.com/r/adventofcode/comments/engeuy/2019_day_22_part_2_i_have_no_idea_where_to_start/
        // https://codeforces.com/blog/entry/72593
        [Solution(22, 2)]
        public string Solution2(string input)
        {




            //var lines = Parser.ToArrayOfString(input);
            //var test = NaiveReverseStep(2604, shortLength, lines);

            return string.Empty;

        }
        // run on 2019, results in 2604
        private int NaiveStep(int pos, int length, string[] directions)
        {
            foreach(var line in directions)
            {
                if (line == "deal into new stack")
                {
                    pos *= -1;
                    pos -= 1;
                }
                else if (line.StartsWith("cut"))
                {
                    var shift = int.Parse(Parser.SplitOnSpace(line)[1]);
                    pos += (-shift);
                }
                else
                {
                    var shift = int.Parse(Parser.SplitOnSpace(line)[3]);
                    pos *= shift;
                }

                pos %= length;
                while (pos < 0)
                    pos += length;
            }

            return pos;
        }

        // run on 2604, should result in 2019
        private int NaiveReverseStep(int pos, int length, string[] directions)
        {
            foreach (var line in directions)
            {
                if (line == "deal into new stack")
                {
                    pos *= -1;
                    pos -= 1;
                }
                else if (line.StartsWith("cut"))
                {
                    var shift = int.Parse(Parser.SplitOnSpace(line)[1]);
                    pos += shift;
                }
                else
                {
                    //var shift = int.Parse(Parser.SplitOnSpace(line)[3]);
                    //pos *= shift;
                    var shift = int.Parse(Parser.SplitOnSpace(line)[3]);
                    while (pos % shift != 0)
                        pos += length;
                    pos /= shift;
                }

                pos %= length;
                while (pos < 0)
                    pos += length;
            }

            return pos;
        }

        




        private long Mod(BigInteger input, BigInteger mod)
        {
            var div = BigInteger.DivRem(input, mod, out var result);

            return (long)result;
        }
    }
}
