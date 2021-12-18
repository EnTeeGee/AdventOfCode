using AdventOfCode.Common;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2021
{
    class Day18
    {
        [Solution(18, 1)]
        public int Solution1(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var number = new SnailNumber(lines[0]);
            for(var i = 1; i < lines.Length; i++)
                number.Add(lines[i]);

            return number.GetMagnitude();
        }

        [Solution(18, 2)]
        public int Solution2(string input)
        {
            var lines = Parser.ToArrayOfString(input);
            var max = 0;
            for(var i = 0; i < lines.Length; i++)
            {
                for(var j = 0; j < lines.Length; j++)
                {
                    if (i == j)
                        continue;

                    var number = new SnailNumber(lines[i]);
                    number.Add(lines[j]);
                    var result = number.GetMagnitude();
                    max = Math.Max(max, result);
                }
            }

            return max;
        }

        private class NumberElement
        {
            public bool IsOpen { get; }
            public bool IsClose { get; }
            public bool IsNumber { get; }
            public int Value { get; }

            public NumberElement(bool isOpen)
            {
                IsOpen = isOpen;
                IsClose = !isOpen;
            }

            public NumberElement(int value)
            {
                Value = value;
                IsNumber = true;
            }

            public static NumberElement Parse(char input)
            {
                if (char.IsDigit(input))
                    return new NumberElement(int.Parse(input.ToString()));

                return new NumberElement(input == '[');
            }
        }

        private class SnailNumber
        {
            private readonly LinkedList<NumberElement> elements;

            public SnailNumber(string input)
            {
                elements = new LinkedList<NumberElement>();
                var filtered = input.Where(it => char.IsDigit(it) || it == ']' || it == '[').ToArray();

                foreach(var item in filtered)
                    elements.AddLast(NumberElement.Parse(item));
            }

            public void Add(string input)
            {
                elements.AddFirst(new NumberElement(true));

                var filtered = input.Where(it => char.IsDigit(it) || it == ']' || it == '[').ToArray();

                foreach (var item in filtered)
                    elements.AddLast(NumberElement.Parse(item));

                elements.AddLast(new NumberElement(false));

                //Console.WriteLine("After Add:");
                //Console.WriteLine(ToString());

                // collapse down number;
                while (Explode() || Split()) ;
            }

            public int GetMagnitude()
            {
                var current = elements.First;
                while(current.Next != null)
                {
                    if(current.Value.IsNumber && current.Next.Value.IsNumber)
                    {
                        var newVal = new NumberElement((current.Value.Value * 3) + (current.Next.Value.Value * 2));
                        elements.Remove(current.Previous);
                        elements.Remove(current.Next);
                        elements.Remove(current.Next);
                        elements.AddAfter(current, newVal);
                        elements.Remove(current);

                        current = elements.First;

                        continue;
                    }

                    current = current.Next;
                }

                return current.Value.Value;
            }

            private bool Explode()
            {
                var depth = 0;
                var current = elements.First;

                while(current.Next != null)
                {
                    var item = current.Value;
                    if (item.IsOpen)
                        depth++;
                    else if (item.IsClose)
                        depth--;
                    else if(depth == 5)
                    {
                        var prior = current.Previous;
                        while(prior != null)
                        {
                            if (prior.Value.IsNumber)
                                break;

                            prior = prior.Previous;
                        }

                        if (prior != null)
                        {
                            elements.AddBefore(prior, new NumberElement(item.Value + prior.Value.Value));
                            elements.Remove(prior);
                        }

                        current = current.Next; //should be the other number of the pair
                        var next = current.Next;
                        while (next != null)
                        {
                            if (next.Value.IsNumber)
                                break;

                            next = next.Next;
                        }

                        if(next != null)
                        {
                            elements.AddBefore(next, new NumberElement(current.Value.Value + next.Value.Value));
                            elements.Remove(next);
                        }

                        // clean up exploded number
                        elements.Remove(current.Next);
                        elements.Remove(current.Previous);
                        elements.Remove(current.Previous);
                        elements.AddAfter(current, new NumberElement(0));
                        elements.Remove(current);

                        //Console.WriteLine("After Explode:");
                        //Console.WriteLine(ToString());

                        return true;
                    }

                    current = current.Next;
                }

                return false;
            }

            private bool Split()
            {
                var current = elements.First;

                while(current != null)
                {
                    var item = current.Value;
                    if(!item.IsNumber || item.Value < 10)
                    {
                        current = current.Next;
                        continue;
                    }

                    var left = item.Value / 2;
                    var right = (item.Value / 2) + (item.Value % 2);

                    elements.AddBefore(current, new NumberElement(true));
                    elements.AddBefore(current, new NumberElement(left));
                    elements.AddBefore(current, new NumberElement(right));
                    elements.AddBefore(current, new NumberElement(false));
                    elements.Remove(current);

                    //Console.WriteLine("After Split:");
                    //Console.WriteLine(ToString());

                    return true;
                }

                return false;
            }

            public override string ToString()
            {
                var output = new StringBuilder();

                foreach(var item in elements)
                {
                    if (item.IsNumber)
                        output.Append(item.Value);
                    else
                        output.Append(item.IsOpen ? "[" : "]");
                }

                return output.ToString();
            }
        }
    }
}
