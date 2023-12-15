using AdventOfCodeCore.Common;
using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2023
{
    internal class Day15
    {
        [Solution(15, 1)]
        public int Solution1(string input)
        {
            return Parser.SplitOn(input, ',').Sum(it => ComputeHash(it));
        }

        [Solution(15, 2)]
        public int Solution2(string input)
        {
            var boxes = Enumerable.Range(0, 256).Select(it => new List<Lens>()).ToArray();
            var items = Parser.SplitOn(input, ',');

            foreach(var item in items)
            {
                var chunks = Parser.SplitOn(item, '=', '-');
                var hash = ComputeHash(chunks[0]);
                if(item.Last() == '-' && boxes[hash].Any(it => it.Label == chunks[0]))
                {
                    var toRemove = boxes[hash].IndexOf(boxes[hash].First(it => it.Label == chunks[0]));
                    boxes[hash].RemoveAt(toRemove);
                }
                else if (item.Contains('='))
                {
                    var existing = boxes[hash].IndexOf(boxes[hash].FirstOrDefault(it => it.Label == chunks[0]));
                    if (existing != -1)
                        boxes[hash][existing] = new Lens(chunks[0], int.Parse(chunks[1]));
                    else
                        boxes[hash].Add(new Lens(chunks[0], int.Parse(chunks[1])));
                }
            }

            return boxes.Select((it, i) => it.Select((l, i2) => l.FocalLength * (i2 + 1) * (i + 1)).Sum()).Sum();
        }

        private int ComputeHash(string input)
        {
            return input.Aggregate(0, (it, acc) => ((acc + it) * 17) % 256);
        }

        private struct Lens
        {
            public string Label { get; }
            public int FocalLength { get; }
        
            public Lens(string label, int focalLength)
            {
                Label = label;
                FocalLength = focalLength;
            }
        }
    }
}
