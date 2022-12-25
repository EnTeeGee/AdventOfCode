using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day25
    {
        [Solution(25, 1)]
        public string Solution(string input)
        {
            return ToSnafu(Parser.ToArrayOfString(input).Sum(it => ToDecimal(it)));
        }

        private long ToDecimal(string input)
        {
            var multiple = 1L;
            var total = 0L;
            while (input.Any())
            {
                var item = input.Last();
                input = input.Substring(0, input.Length - 1);

                total += Enumerable.Range(-2, 5)
                    .Select(it => it * multiple)
                    .Zip("=-012", (a, b) => new { value = a, symbol = b })
                    .First(it => it.symbol == item).value;

                multiple *= 5;
            }

            return total;
        }

        private string ToSnafu(long input)
        {
            var output = new List<char>();
            var targetMultiple = 5L;
            var step = 1L;

            while (input != 0)
            {
                var result = Enumerable.Range(-2, 5)
                    .Select(it => it * step)
                    .Zip("210-=", (a, b) => new { value = a, symbol = b })
                    .First(it => (input + it.value) % targetMultiple == 0);

                output.Insert(0, result.symbol);
                input += result.value;
                targetMultiple *= 5;
                step *= 5;
            }

            return new string(output.ToArray());
        }
    }
}
