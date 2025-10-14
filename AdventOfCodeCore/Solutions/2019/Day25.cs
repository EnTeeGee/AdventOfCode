using AdventOfCode2019.Common;
using AdventOfCodeCore.Core;
using System.Text;

namespace AdventOfCodeCore.Solutions._2019
{
    internal class Day25
    {
        [Solution(25, 1)]
        public string Solution(string input)
        {
            var computer = new Intcode(input);

            while (true)
            {
                var result = computer.RunToEnd();
                computer.ClearOutput();
                Console.WriteLine(Encoding.ASCII.GetString(result.Select(it => (byte)it).ToArray()));
                var command = Console.ReadLine();
                if (command == null)
                    continue;

                foreach (var item in command)
                    computer.AddInput((int)item);
                computer.AddInput(10);
            }
        }
    }
}
