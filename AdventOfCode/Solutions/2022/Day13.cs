using AdventOfCode.Common;
using AdventOfCode.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day13
    {
        [Solution(13, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfGroups(input);

            return lines.Select(it => Parser.ToArrayOfString(it).Select(l => JArray.Parse(l)).ToArray())
                .Select(it => Compare(it[0], it[1]))
                .Zip(Enumerable.Range(1, lines.Length), (a, b) => a == true ? b : 0)
                .Sum();
        }

        [Solution(13, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input).Concat(new[] { "[[2]]", "[[6]]" }).Select(it => JArray.Parse(it)).ToArray();
            var targets = lines.Skip(lines.Length - 2).ToArray();
            var sorted = lines.OrderByDescending(it => new SortablePacket(it)).ToArray();

            return (Array.IndexOf(sorted, targets[0]) + 1) * (Array.IndexOf(sorted, targets[1]) + 1);
        }

        static private bool? Compare(JArray firstArray, JArray secondArray)
        {
            for (var i = 0; i < firstArray.Count; i++)
            {
                if (secondArray.Count <= i)
                    return false;

                var item = firstArray[i];
                var target = secondArray[i];

                if (item.Type == JTokenType.Integer && target.Type == JTokenType.Integer)
                {
                    var first = item.ToObject<int>();
                    var second = target.ToObject<int>();
                    if (first < second)
                        return true;
                    else if (second < first)
                        return false;

                    continue;
                }

                if (item.Type == JTokenType.Integer)
                    item = new JArray(item);
                else if (target.Type == JTokenType.Integer)
                    target = new JArray(target);

                var arrayResult = Compare(item as JArray, target as JArray);

                if (arrayResult != null)
                    return arrayResult;
            }

            if (secondArray.Count == firstArray.Count)
                return null;

            return true;
        }

        private class SortablePacket: IComparable<SortablePacket>
        {
            public JArray Item { get; }

            public SortablePacket(JArray item)
            {
                Item = item;
            }

            public int CompareTo(SortablePacket other)
            {
                if (ReferenceEquals(Item, other.Item))
                    return 0;

                var result = Compare(Item, other.Item);
                return result == true ? 1 : result == false ? -1 : 0;
            }
        }
    }
}
