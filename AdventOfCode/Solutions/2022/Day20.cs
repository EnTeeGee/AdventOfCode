using AdventOfCode.Common;
using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2022
{
    class Day20
    {
        [Solution(20, 1)]
        public long Solution1(string input)
        {
            var items = Parser.ToArrayOf(input, it => new Digit(int.Parse(it))).ToList();
            var i = 0;
            while(i < items.Count)
            {
                var item = items[i];

                if (item.Moved)
                {
                    i += 1;
                    continue;
                }

                items.RemoveAt(i);
                var newIndex = i + item.Value;
                newIndex = GetIndex(newIndex, items.Count);
                
                item.Moved = true;
                items.Insert((int)newIndex, item);
            }

            var startIndex = items.IndexOf(items.First(it => it.Value == 0));

            return items[GetIndex(startIndex + 1000, items.Count)].Value
                + items[GetIndex(startIndex + 2000, items.Count)].Value
                + items[GetIndex(startIndex + 3000, items.Count)].Value;
        }

        [Solution(20, 2)]
        public long Solution2(string input)
        {
            var refArray = Parser.ToArrayOf(input, it => new Digit(long.Parse(it) * 811589153));
            var items = new LinkedList<Digit>(refArray);

            for(var loop = 0; loop < 10; loop++)
            {
                foreach(var item in refArray)
                {
                    var i = IndexOf(items, item);
                    RemoveAt(items, i);
                    var newIndex = i + item.Value;
                    newIndex = GetIndex(newIndex, items.Count);
                    Insert(items, (int)newIndex, item);
                }
            }

            var startIndex = IndexOf(items, refArray.First(it => it.Value == 0));

            return GetAt(items, GetIndex(startIndex + 1000, items.Count))
                + GetAt(items, GetIndex(startIndex + 2000, items.Count))
                + GetAt(items, GetIndex(startIndex + 3000, items.Count));
        }

        private int GetIndex(long index, int length)
        {
            if(index < 0)
            {
                index += (length * -(index / length));
            }
            while (index < 0)
                index += length;
            if (index >= length)
                index %= length;

            return (int)index;
        }

        private int IndexOf(LinkedList<Digit> list, Digit item)
        {
            var i = 0;
            var current = list.First;
            while(true)
            {
                if (current.Value == item)
                    return i;

                i += 1;
                current = current.Next;
            }
        }

        private void RemoveAt(LinkedList<Digit> list, int index)
        {
            var i = 0;
            var current = list.First;
            while (i < index)
            {
                i += 1;
                current = current.Next;
            }

            list.Remove(current);
        }

        private void Insert(LinkedList<Digit> list, int index, Digit item)
        {
            var i = 0;
            var current = list.First;
            while(i < index)
            {
                i += 1;
                current = current.Next;
            }

            list.AddBefore(current, item);
        }

        private long GetAt(LinkedList<Digit> list, int index)
        {
            var i = 0;
            var current = list.First;
            while(i < index)
            {
                i += 1;
                current = current.Next;
            }

            return current.Value.Value;
        }

        private class Digit
        {
            public Digit(long value)
            {
                Value = value;
                Moved = false;
            }

            public long Value { get; }
            public bool Moved { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}
