namespace AdventOfCodeCore.Common
{
    internal class PriorityQueueHeap<T>
    {
        private List<Node<T>> heap;
        private Dictionary<int, Node<T>> heapMap;

        public PriorityQueueHeap()
        {
            heap = new List<Node<T>>();
            heapMap = new Dictionary<int, Node<T>>();
        }

        public void Add(T item, int priority)
        {
            if(heapMap.ContainsKey(priority))
            {
                heapMap[priority].Values.Push(item);

                return;
            }

            var node = new Node<T>(item, priority);
            heap.Add(node);
            heapMap.Add(priority, node);
            ShiftUp(heap.Count - 1);
        }

        public T? PeekMax()
        {
            return heap.Any() ? heap.First().Values.Peek() : default(T);
        }

        public T? PopMax()
        {
            if (!heap.Any())
                return default(T);

            var toReturn = heap[0].Values.Pop();
            if (!heap[0].Values.Any())
            {
                heapMap.Remove(heap[0].Priority);
                heap[0] = heap.Last();
                heap.RemoveAt(heap.Count - 1);

                ShiftDown(0);
            }

            return toReturn;
        }

        public bool Any()
        {
            return heap.Any();
        }

        private int GetParent(int index)
        {
            if (index == 0)
                return -1;

            return (index - 1) / 2;
        }

        private int GetLeftChild(int index)
        {
            return ((2 * index) + 1);
        }

        private int GetRightChild(int index)
        {
            return ((2 * index) + 2);
        }

        private void ShiftUp(int index)
        {
            while (index > 0 && heap[GetParent(index)].Priority < heap[index].Priority)
            {
                var swap = heap[index];
                heap[index] = heap[GetParent(index)];
                heap[GetParent(index)] = swap;

                index = GetParent(index);
            }
        }

        private void ShiftDown(int index)
        {
            var maxIndex = index;
            var leftIndex = GetLeftChild(index);
            if (leftIndex < heap.Count && heap[maxIndex].Priority < heap[leftIndex].Priority)
                maxIndex = leftIndex;

            var rightIndex = GetRightChild(index);
            if(rightIndex < heap.Count && heap[maxIndex].Priority < heap[rightIndex].Priority)
                maxIndex = rightIndex;

            if(index != maxIndex)
            {
                var swap = heap[index];
                heap[index] = heap[maxIndex];
                heap[maxIndex] = swap;

                ShiftDown(maxIndex);
            }

        }

        private readonly struct Node<U>
        {
            public Stack<U> Values { get; init; }
            public int Priority { get; init; }

            public Node(U value, int priority)
            {
                Values = new Stack<U>();
                Values.Push(value);
                Priority = priority;
            }
        }
    }
}
