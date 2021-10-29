using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public class PriorityQueue<T>
    {
        private Func<T, T, int> comparer;
        private LinkedList<T> queue;

        public PriorityQueue(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
            queue = new LinkedList<T>();
        }

        public void Insert(T item)
        {
            var current = queue.Last;

            while(current != null)
            {
                if(comparer(item, current.Value) > 0)
                {
                    queue.AddAfter(current, new LinkedListNode<T>(item));

                    return;
                }

                current = current.Previous;
            }

            queue.AddFirst(item);
        }

        public void InsertIfUnique(T item, Func<T, T, bool> equater)
        {
            var current = queue.Last;

            while (current != null)
            {
                if (equater(item, current.Value))
                    return;

                if (comparer(item, current.Value) > 0)
                {
                    queue.AddAfter(current, new LinkedListNode<T>(item));

                    return;
                }

                current = current.Previous;
            }

            queue.AddFirst(item);
        }

        public T Pop()
        {
            var first = queue.First;
            queue.RemoveFirst();

            return first.Value;
        }

        public bool Any()
        {
            return queue.First != null;
        }
    }
}
