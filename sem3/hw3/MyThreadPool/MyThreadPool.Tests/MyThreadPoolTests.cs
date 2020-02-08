using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyThreadPool.Tests
{
    [TestClass]
    public class MyThreadPoolTests
    {
        [TestMethod]
        public void OneThreadTest()
        {
            var pool = new MyThreadPool(1);
            var task1 = pool.AddTask(() => 30 + 70);
            var task2 = pool.AddTask(() => 123 + 123);
            var task3 = pool.AddTask(() => 31 + 30);

            Assert.AreEqual(100, task1.Result);
            Assert.AreEqual(246, task2.Result);
            Assert.AreEqual(61, task3.Result);
        }

        [TestMethod]
        public void SeveralThreadsTest()
        {
            var pool = new MyThreadPool(2);
            var task1 = pool.AddTask(() => 30 + 70);
            var task2 = pool.AddTask(() => 123 + 123);
            var task3 = pool.AddTask(() => 31 + 30);

            Assert.AreEqual(100, task1.Result);
            Assert.AreEqual(246, task2.Result);
            Assert.AreEqual(61, task3.Result);

            pool = new MyThreadPool(3);
            task1 = pool.AddTask(() => 30 + 70);
            task2 = pool.AddTask(() => 123 + 123);
            task3 = pool.AddTask(() => 31 + 30);

            Assert.AreEqual(100, task1.Result);
            Assert.AreEqual(246, task2.Result);
            Assert.AreEqual(61, task3.Result);
        }

        [TestMethod]
        public void NumberOfThreadsTest()
        {
            Assert.IsTrue(new MyThreadPool(7).NumberOfThreads >= 7);
            Assert.IsTrue(new MyThreadPool(1).NumberOfThreads >= 1);
            Assert.IsTrue(new MyThreadPool(15).NumberOfThreads >= 15);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IncorrectNumberOfThreadsTest()
        {
            new MyThreadPool(0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShutDownTest()
        {
            var pool = new MyThreadPool(5);
            var task1 = pool.AddTask(() => 30 + 70);
            var task2 = pool.AddTask(() => 123 + 123);
            pool.Shutdown();

            Assert.AreEqual(100, task1.Result);
            Assert.AreEqual(246, task2.Result);

            var task3 = pool.AddTask(() => 31 + 30);
        }
    }
}
