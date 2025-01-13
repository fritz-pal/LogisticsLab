namespace dijkstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class PriorityQueue<T>
    {
        private List<(T Item, int Priority)> elements = new List<(T, int)>();

        public int Count => elements.Count;

        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
            elements.Sort((a, b) => a.Priority.CompareTo(b.Priority)); // Min-Heap
        }

        public (T Item, int Priority) Dequeue()
        {
            var best = elements.First();
            elements.RemoveAt(0);
            return best;
        }
    }
}