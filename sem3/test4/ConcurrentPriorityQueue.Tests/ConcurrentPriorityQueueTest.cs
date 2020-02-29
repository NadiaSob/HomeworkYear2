namespace ConcurrentPriorityQueue.Tests
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using test4;

    [TestClass]
    public class ConcurrentPriorityQueueTest
    {
        [TestInitialize]
        public void Initialize()
        {
            queue = new ConcurrentPriorityQueue<string>();
        }

        [TestMethod]
        public void OneElementEnqueueAndDequeueOneThreadTest()
        {
            queue.Enqueue("Test string", 1);
            Assert.AreEqual("Test string", queue.Dequeue());
        }

        [TestMethod]
        public void SeveralElementsEnqueueAndDequeueOneThreadTest()
        {
            queue.Enqueue("Test string 3", 4);
            queue.Enqueue("Test string 4", 2);
            queue.Enqueue("Test string 2", 7);
            queue.Enqueue("Test string 1", 10);
            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());
            Assert.AreEqual("Test string 3", queue.Dequeue());
            Assert.AreEqual("Test string 4", queue.Dequeue());
        }

        [TestMethod]
        public void EnqueueAndDequeueSeveralThreadsTest()
        {
            Thread[] threads = new Thread[4];
            threads[0] = new Thread(() => queue.Enqueue("Test string 3", 4));
            threads[1] = new Thread(() => queue.Enqueue("Test string 4", 2));
            threads[2] = new Thread(() => queue.Enqueue("Test string 2", 7));
            threads[3] = new Thread(() => queue.Enqueue("Test string 1", 10));

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());
            Assert.AreEqual("Test string 3", queue.Dequeue());
            Assert.AreEqual("Test string 4", queue.Dequeue());
        }

        [TestMethod]
        public void SizeTest()
        {
            Thread[] threads = new Thread[4];
            threads[0] = new Thread(() => queue.Enqueue("Test string 3", 4));
            threads[1] = new Thread(() => queue.Enqueue("Test string 4", 2));
            threads[2] = new Thread(() => queue.Enqueue("Test string 2", 7));
            threads[3] = new Thread(() => queue.Enqueue("Test string 1", 10));

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(4, queue.Size());
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            Assert.AreEqual(0, queue.Size());
        }

        [TestMethod]
        public void EnqueueAndDequeueWithTheSamePriorityOneThreadTest()
        {
            queue.Enqueue("Test string", 6);
            queue.Enqueue("Test string 1", 2);
            queue.Enqueue("Test string 2", 2);

            Assert.AreEqual("Test string", queue.Dequeue());
            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());

            queue.Enqueue("Test string 1", 7);
            queue.Enqueue("Test string 2", 7);
            queue.Enqueue("Test string", 0);

            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());
            Assert.AreEqual("Test string", queue.Dequeue());
        }

        [TestMethod]
        public void EnqueueAndDequeueWithTheSamePrioritySeveralThreadsTest()
        {
            Thread[] threads = new Thread[3];

            threads[0] = new Thread(() => queue.Enqueue("Test string", 6));
            threads[1] = new Thread(() => queue.Enqueue("Test string 1", 2));
            threads[2] = new Thread(() => queue.Enqueue("Test string 2", 2));

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual("Test string", queue.Dequeue());
            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());

            threads[0] = new Thread(() => queue.Enqueue("Test string 1", 7));
            threads[1] = new Thread(() => queue.Enqueue("Test string 2", 7));
            threads[2] = new Thread(() => queue.Enqueue("Test string", 0));

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual("Test string 1", queue.Dequeue());
            Assert.AreEqual("Test string 2", queue.Dequeue());
            Assert.AreEqual("Test string", queue.Dequeue());
        }

        [TestMethod]
        public void DequeueFromEmptyQueueTest()
        {
            Thread[] threads = new Thread[2];
            string element = " ";

            threads[0] = new Thread(() =>
            { element = queue.Dequeue(); });
            threads[1] = new Thread(() => queue.Enqueue("Test string", 1));

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual("Test string", element);
        }

        private ConcurrentPriorityQueue<string> queue;
    }
}
