using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2021
{
    class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var binary = string.Join("", input.Trim().Select(it => Convert.ToByte(it.ToString(), 16)).Select(it => Convert.ToString(it, 2).PadLeft(8, '0').Substring(4, 4)));

            var packetTree = new Packet();
            var result = packetTree.Parse(binary);


            return packetTree.GetVersionSum();
        }

        [Solution(16, 2)]
        public long Solution2(string input)
        {
            var binary = string.Join("", input.Trim().Select(it => Convert.ToByte(it.ToString(), 16)).Select(it => Convert.ToString(it, 2).PadLeft(8, '0').Substring(4, 4)));

            var packetTree = new Packet();
            packetTree.Parse(binary);


            return packetTree.Caculate();
        }

        private class Packet
        {
            private int version;
            private int typeId;

            private Packet[] subPackets;
            private long value;

            public int Parse(string input)
            {
                version = Convert.ToInt32(input.Substring(0, 3), 2);
                typeId = Convert.ToInt32(input.Substring(3, 3), 2);

                if (typeId == 4)
                {
                    subPackets = new Packet[0];
                    var index = 1;
                    var fullBits = string.Empty;
                    do
                    {
                        index += 5;
                        fullBits += input.Substring(index + 1, 4);

                    } while (input[index] == '1');

                    value = Convert.ToInt64(fullBits, 2);

                    return index + 5;
                }
                else
                {
                    var subPacketList = new List<Packet>();
                    int currentIndex;

                    if (input[6] == '0')
                    {
                        var subPacketLength = Convert.ToInt32(input.Substring(7, 15), 2);
                        currentIndex = 22;
                        while (currentIndex < subPacketLength + 22)
                        {
                            var subPacket = new Packet();
                            currentIndex += subPacket.Parse(input.Substring(currentIndex));
                            subPacketList.Add(subPacket);
                        }
                    }
                    else
                    {
                        var subPacketCount = Convert.ToInt32(input.Substring(7, 11), 2);
                        currentIndex = 18;
                        for (var i = 0; i < subPacketCount; i++)
                        {
                            var subPacket = new Packet();
                            currentIndex += subPacket.Parse(input.Substring(currentIndex));
                            subPacketList.Add(subPacket);
                        }
                    }

                    subPackets = subPacketList.ToArray();

                    return currentIndex;
                }
            }

            public long Caculate()
            {
                switch(typeId)
                {
                    case 0:
                        return subPackets.Sum(it => it.Caculate());
                    case 1:
                        return subPackets.Aggregate(1L, (acc, i) => acc * i.Caculate());
                    case 2:
                        return subPackets.Min(it => it.Caculate());
                    case 3:
                        return subPackets.Max(it => it.Caculate());
                    case 4:
                        return value;
                    case 5:
                        return subPackets[0].Caculate() > subPackets[1].Caculate() ? 1 : 0;
                    case 6:
                        return subPackets[0].Caculate() < subPackets[1].Caculate() ? 1 : 0;
                    case 7:
                        return subPackets[0].Caculate() == subPackets[1].Caculate() ? 1 : 0;
                }    

                return 0;
            }

            public int GetVersionSum()
            {
                return version + subPackets.Sum(it => it.GetVersionSum());
            }
        }
    }
}
