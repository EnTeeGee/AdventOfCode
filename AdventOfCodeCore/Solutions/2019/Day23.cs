using AdventOfCode2019.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2019
{
    internal class Day23
    {
        [Solution(23, 1)]
        public long Solution1(string input)
        {
            var computers = Enumerable.Range(0, 50).Select(it => FormComputer(it, input)).ToArray();

            while (true)
            {
                foreach (var item in computers)
                {
                    item.RunStep();
                    while(item.Outputs.Count() >= 3)
                    {
                        var section = item.Outputs.Take(3).ToArray();
                        item.ClearOutput(3);
                        if (section[0] == 255)
                            return section[2];

                        if (section[0] >= 50)
                            throw new Exception("Network index out of bounds");


                        computers[section[0]].AddInput(section[1]);
                        computers[section[0]].AddInput(section[2]);
                    }
                }
            }
        }

        [Solution(23, 2)]
        public long Solution2(string input)
        {
            var computers = Enumerable.Range(0, 50).Select(it => FormComputer(it, input)).ToArray();
            (long x, long y) natPacket = (0, 0);
            HashSet<long> delivered = new HashSet<long>();

            while (true)
            {
                if (computers.All(it => !it.HasPendingInputs))
                {
                    if (delivered.Contains(natPacket.y))
                        return natPacket.y;

                    delivered.Add(natPacket.y);
                    computers[0].AddInput(natPacket.x);
                    computers[0].AddInput(natPacket.y);
                    computers[0].AddInput(-1);
                }

                foreach (var item in computers)
                {
                    item.RunStep();
                    while (item.Outputs.Count() >= 3)
                    {
                        var section = item.Outputs.Take(3).ToArray();
                        item.ClearOutput(3);
                        if (section[0] == 255)
                            natPacket = (section[1], section[2]);
                        else
                        {
                            computers[section[0]].AddInput(section[1]);
                            computers[section[0]].AddInput(section[2]);
                            computers[section[0]].AddInput(-1);
                        }
                    }
                }
            }
        }

        private Intcode FormComputer(int index, string text)
        {
            var computer = new Intcode(text);
            computer.SetConstInput(-1);
            computer.AddInput(index);
            computer.AddInput(-1);

            return computer;
        }
    }
}
