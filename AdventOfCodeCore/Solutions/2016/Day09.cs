using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2016
{
    class Day09
    {
        [Solution(9, 1)]
        public int Solution1(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => ExpandString(it).Length);
        }

        [Solution(9, 2)]
        public long Solution2(string input)
        {
            return Parser.ToArrayOfString(input).Sum(it => GetLengthOfSection(it));
        }

        public string ExpandString(string input)
        {
            var nextMarker = input.IndexOf('(');
            var output = new StringBuilder();

            while (nextMarker != -1)
            {
                output.Append(input.Substring(0, nextMarker));
                var markerEnd = input.IndexOf(')');
                var values = Parser.SplitOn(input.Substring(nextMarker + 1, markerEnd - nextMarker - 1), 'x').Select(it => int.Parse(it)).ToArray();
                input = input.Substring(markerEnd + 1);
                var dupeSection = input.Substring(0, values[0]);
                for (var i = 0; i < values[1]; i++)
                    output.Append(dupeSection);

                input = input.Substring(dupeSection.Length);
                nextMarker = input.IndexOf('(');
            }

            output.Append(input);

            return output.ToString();
        }

        public long GetLengthOfSection(string input)
        {
            var output = 0L;
            var nextMarker = input.IndexOf('(');

            while (nextMarker != -1)
            {
                output += nextMarker;
                var markerEnd = input.IndexOf(')');
                var values = Parser.SplitOn(input.Substring(nextMarker + 1, markerEnd - nextMarker - 1), 'x').Select(it => int.Parse(it)).ToArray();
                var subString = input.Substring(markerEnd + 1, values[0]);
                output += (GetLengthOfSection(subString) * values[1]);
                input = input.Substring(markerEnd + 1 + values[0]);
                nextMarker = input.IndexOf('(');
            }

            output += input.Length;

            return output;
        }
    }
}
