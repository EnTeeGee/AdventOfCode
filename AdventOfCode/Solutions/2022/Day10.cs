using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2022
{
    class Day10
    {
        [Solution(10, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it)).ToArray()
                .SelectMany(it => it[0] == "addx" ? new[] { 0, int.Parse(it[1]) } : new []{ 0 })
                .ToArray();

            var totalSum = 0;
            for(var i = 20; i <= 220; i+= 40)
            {
                var register = lines.Take(i - 1).Sum() + 1;

                totalSum += register * i;
            }

            return totalSum;
        }

        [Solution(10, 2)]
        public string Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input)
                .Select(it => Parser.SplitOnSpace(it)).ToArray()
                .SelectMany(it => it[0] == "addx" ? new[] { 0, int.Parse(it[1]) } : new[] { 0 })
                .ToArray();

            var output = new StringBuilder();
            for(var i = 0; i < 240; i++)
            {
                var register = lines.Take(i).Sum() + 1;

                if ((i % 40) + 1 >= register && (i % 40) + 1 <= register + 2)
                    output.Append("#");
                else
                    output.Append(".");

                if ((i + 1) % 40 == 0)
                    output.AppendLine();
            }

            return output.ToString();
        }
    }
}
