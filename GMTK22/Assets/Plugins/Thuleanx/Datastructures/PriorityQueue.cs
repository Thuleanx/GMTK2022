using System;
using System.Collections;
using System.Collections.Generic;
using Advanced.Algorithms.DataStructures;

// Using outside code will have to rewrite later
namespace Thuleanx.DS {
	public class PriorityQueue<T> : IEnumerable<T>
    {
        private readonly BHeap<T> heap;
        public PriorityQueue()
        {
            heap = new BHeap<T>();
        }

		public PriorityQueue(IComparer<T> comparer) {
			heap = new BHeap<T>(comparer);
		}

        /// <summary>
        /// Time complexity:O(log(n)).
        /// </summary>
        public void Enqueue(T item)
        {
            heap.Insert(item);
        }

        /// <summary>
        /// Time complexity:O(log(n)).
        /// </summary>
        public T Dequeue()
        {
            return heap.Extract();
        }

        /// <summary>
        /// Time complexity:O(1).
        /// </summary>
        public T Peek()
        {
            return heap.Peek();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return heap.GetEnumerator();
        }

		public int Count => heap.Count;
    }
}