using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

            return (pos % limit).ToString();
            //return $"{(pos % limit).ToString()} ({pos})";
        }

        // https://www.reddit.com/r/adventofcode/comments/etxg0o/how_do_i_solve_day_22_part_2_2019_x_times_in_a_row/
        // https://www.reddit.com/r/adventofcode/comments/engeuy/2019_day_22_part_2_i_have_no_idea_where_to_start/
        // https://codeforces.com/blog/entry/72593
        [Solution(22, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var limit = new BigInteger(119315717514047);
            //var limit = new BigInteger(10007);
            var shuffles = new BigInteger(101741582076661);
            //var shuffles = new BigInteger(1);
            var startPoint = new BigInteger(2020);
            //var startPoint = new BigInteger(2019);

            var ab = ConvertToLinear(lines[0]);

            foreach(var line in lines.Skip(1))
            {
                var cd = ConvertToLinear(line);
                ab = (Mod(ab.a * cd.a, limit), Mod((ab.b * cd.a) + cd.b, limit));
            }

            var naiveVal = startPoint;
            var naiveFound = new List<BigInteger>();
            for (var i = 0; i < 5; i++)
            {
                naiveVal = NaiveStep(naiveVal, limit, lines);
                naiveFound.Add(naiveVal);
            }

            Console.WriteLine($"Values from NaiveStep: {string.Join(", ", naiveFound.ToArray())}");

            var composeVal = startPoint;
            var composeAb = ab;
            var composeFound = new List<BigInteger>();
            for(var i = 0; i < 5; i++)
            {
                composeVal = (long)Mod((ab.a * composeVal) + ab.b, limit);
                while (composeVal < 0)
                    composeVal += (long)limit;
                composeFound.Add(composeVal);
                composeAb = (Mod(composeAb.a * ab.a, limit), Mod((composeAb.b * composeAb.a) + ab.b, limit));
            }

            Console.WriteLine($"Values from Compose: {string.Join(", ", composeFound.ToArray())}");


            var finalFound = new List<long>();
            for(var i = 1; i < 6; i++)
            {
                var test1 = (Pow(ab.a, i, limit) * startPoint);
                var test2 = Div(ab.b * (1 - Pow(ab.a, i, limit)), 1 - ab.a, limit);
                var result = Mod(test1 + test2, limit);
                while (result < 0)
                    result += limit;
                finalFound.Add((long)result);
            }

            Console.WriteLine($"Values for final: {string.Join(", ", finalFound.ToArray())}");


            var section1 = Pow(ab.a, shuffles, limit) * startPoint;
            var section2 = Div(ab.b * (1 - Pow(ab.a, shuffles, limit)), 1 - ab.a, limit);
            var forwardResult = Mod(section1 + section2, limit);

            Console.WriteLine($"Result going forward: {forwardResult}");

            var testResult = ComputeReverse(ab.a, ab.b, shuffles, forwardResult, limit);
            Console.WriteLine($"Test result is {testResult} (should be {startPoint})");

            return ComputeReverse(ab.a, ab.b, shuffles, startPoint, limit).ToString();
        }
        // run on 2019, results in 2604
        private BigInteger NaiveStep(BigInteger pos, BigInteger length, string[] directions)
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


        private BigInteger Mod(BigInteger input, BigInteger mod)
        {
            var div = BigInteger.DivRem(input, mod, out var result);

            return result;
        }

        private BigInteger Pow(BigInteger value, BigInteger pow, BigInteger limit)
        {
            if (pow == 0)
                return BigInteger.One;

            var result = Pow(value, pow / 2, limit);
            if (pow % 2 == 0)
                return Mod(result * result, limit);

            return Mod(result * result * value, limit);

        }

        private BigInteger Div(BigInteger num, BigInteger den, BigInteger limit)
        {
            // need to find a value x such that mod(x * den) = num
            // using code from https://www.geeksforgeeks.org/dsa/modular-division/
            var x = BigInteger.Zero;
            var y = BigInteger.Zero;
            var check = GcdExtended(den, limit, out x, out y);
            if (check != 1)
                throw new Exception("Can't find valid value");

            var modInverse = Mod((x % limit) + limit, limit);

            return Mod(num * modInverse, limit);
        }

        private BigInteger GcdExtended(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = BigInteger.Zero;
                y = BigInteger.One;
                return b;
            }

            var step = GcdExtended(b % a, a, out var x1, out var y1);
            x = y1 - (BigInteger.Divide(b, a) * x1);
            y = x1;

            return step;
        }

        private (BigInteger a, BigInteger b) ConvertToLinear(string line)
        {
            var chunks = Parser.SplitOnSpace(line);

            if (chunks[1] == "into")
                return (BigInteger.MinusOne, BigInteger.MinusOne);

            if (chunks[0] == "cut")
            {
                var value = BigInteger.Parse(chunks[1]);
                return (BigInteger.One, -value);
            }

            return (BigInteger.Parse(chunks[3]), BigInteger.Zero);
        }

        private BigInteger ComputeReverse(BigInteger a, BigInteger b, BigInteger shuffles, BigInteger endPoint, BigInteger limit)
        {
            var subSection = Div(b * (1 - Pow(a, shuffles, limit)), 1 - a, limit);
            var fSection = endPoint - subSection;
            var result = Div(fSection, Pow(a, shuffles, limit), limit);

            while (result < 0)
                result += limit;

            return result;
        }
    }
}
