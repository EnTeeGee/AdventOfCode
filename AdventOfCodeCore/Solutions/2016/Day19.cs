using AdventOfCodeCore.Core;

namespace AdventOfCodeCore.Solutions._2016
{
    internal class Day19
    {
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var elves = int.Parse(input);
            var last = new Elf(elves);
            var current = last;
            for(var i = elves - 1; i > 0; i--)
                current = new Elf(i, current);
            last.Next = current;

            while (current.Next != current)
            {
                current.Next = current.Next.Next;
                current = current.Next;
            }
                

            return current.Id;
        }

        [Solution(19, 2)]
        public int Solution2(string input)
        {
            var elves = int.Parse(input);
            var last = new Elf(elves);
            var current = last;
            var removeInt = (elves / 2);
            var removeAt = current;
            for (var i = elves - 1; i > 0; i--)
            {
                current = new Elf(i, current);
                if (i == removeInt)
                    removeAt = current;
            }
                
            last.Next = current;
            var remaining = elves;

            while(current.Next != current)
            {
                removeAt.Next = removeAt.Next.Next;
                if (remaining % 2 != 0)
                    removeAt = removeAt.Next;
                remaining--;
                current = current.Next;
            }

            return current.Id;
        }

        private class Elf
        {
            public int Id { get; }
            public Elf Next { get; set; }

            public Elf(int id)
            {
                Id = id;
                Next = this;
            }

            public Elf(int id, Elf next)
            {
                Id = id;
                Next = next;
            }
        }
        
    }
}
