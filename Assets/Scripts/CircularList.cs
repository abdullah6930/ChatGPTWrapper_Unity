using System.Collections.Generic;
using System;

namespace AbdullahQadeer.helper
{
    public class CircularList<T>
    {
        private Queue<T> queue;
        private int maxSize;

        public CircularList(int maxSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max size must be greater than 0.");
            }
            this.maxSize = maxSize;
            queue = new Queue<T>(maxSize);
        }

        public void Add(T item)
        {
            if (queue.Count >= maxSize)
            {
                queue.Dequeue(); // Remove the oldest element
            }
            queue.Enqueue(item);
        }

        public void Resize(int newSize)
        {
            if (newSize <= 0)
            {
                throw new ArgumentException("New size must be greater than 0.");
            }

            // Dequeue elements until the size matches the new size
            while (queue.Count > newSize)
            {
                queue.Dequeue();
            }

            maxSize = newSize;
        }

        public List<T> ToList()
        {
            return new List<T>(queue);
        }

        public void Clear()
        {
            queue.Clear();
        }

        public int Count => queue.Count;

        public IEnumerable<T> Items => queue;
    }
}