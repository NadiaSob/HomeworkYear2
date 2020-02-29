using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace test4
{
    /// <summary>
    /// Class implementing thread-safe blocking priority queue.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    public class ConcurrentPriorityQueue<T>
    {
        private object lockObject = new object();
        
        private SortedList<int, Queue<T>> list = new SortedList<int, Queue<T>>();

        /// <summary>
        /// Returns size of the queue.
        /// </summary>
        /// <returns>Size of the queue.</returns>
        public int Size()
        {
            int size;

            lock (lockObject)
            {
                size = list.Count();
            }
            return size;
        }

        /// <summary>
        /// Adds new queue element in the correct place depending on the given priority.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <param name="priority">Priority of the element.</param>
        public void Enqueue(T value, int priority)
        {
            lock (lockObject)
            {
                if (list.ContainsKey(priority))
                {
                    list[priority].Enqueue(value);
                }
                else
                {
                    var queue = new Queue<T>();
                    queue.Enqueue(value);
                    list.Add(priority, queue);
                }

                Monitor.PulseAll(lockObject);
            }
        }

        /// <summary>
        /// Returns element with the highest priority and removes it from the queue.
        /// </summary>
        /// <returns>Queue element with the highest priority.</returns>
        public T Dequeue()
        {
            T element;
            lock (lockObject)
            {
                if (list.Count == 0)
                {
                    Monitor.Wait(lockObject);
                }

                element = list.Last().Value.Dequeue();
                if (list.Last().Value.Count == 0)
                {
                    list.Remove(list.Last().Key);
                }
            }
            return element;
        }
    }
}
