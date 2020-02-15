using System;
using System.Collections.Generic;
using System.Threading;
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

            Assert.IsTrue(task1.IsCompleted);
            Assert.IsTrue(task2.IsCompleted);
            Assert.IsTrue(task3.IsCompleted);
        }

        [TestMethod]
        public void SeveralThreadsTest()
        {
            var pool = new MyThreadPool(2);
            var task1 = pool.AddTask(() => 300 + 700);
            var task2 = pool.AddTask(() => 367 + 367);
            var task3 = pool.AddTask(() => 2 * 2);

            Assert.AreEqual(1000, task1.Result);
            Assert.AreEqual(734, task2.Result);
            Assert.AreEqual(4, task3.Result);

            Assert.IsTrue(task1.IsCompleted);
            Assert.IsTrue(task2.IsCompleted);
            Assert.IsTrue(task3.IsCompleted);

            pool = new MyThreadPool(3);
            task1 = pool.AddTask(() => 300 + 700);
            task2 = pool.AddTask(() => 367 + 367);
            task3 = pool.AddTask(() => 2 * 2);

            Assert.AreEqual(1000, task1.Result);
            Assert.AreEqual(734, task2.Result);
            Assert.AreEqual(4, task3.Result);

            Assert.IsTrue(task1.IsCompleted);
            Assert.IsTrue(task2.IsCompleted);
            Assert.IsTrue(task3.IsCompleted);
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
            var taskList = new List<IMyTask<int>>();

            for (var i = 0; i < 4; ++i)
            {
                taskList.Add(pool.AddTask(() =>
                {
                    Thread.Sleep(1000);
                    return 2 + 8;
                }));
            }

            pool.Shutdown();

            foreach (var task in taskList)
            {
                Assert.AreEqual(10, task.Result);
                Assert.IsTrue(task.IsCompleted);
            }

            pool.AddTask(() => 31 + 30);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CatchingExceptionTest()
        {
            var pool = new MyThreadPool(2);
            var zero = 0;
            var task = pool.AddTask(() => 159 / zero);
            _ = task.Result;
        }

        [TestMethod]
        public void ContinueWithTest()
        {
            var pool = new MyThreadPool(5);
            var task = pool.AddTask(() => 1000 + 1000);

            var newTask1 = task.ContinueWith((result) => result * 5);
            var newTask2 = newTask1.ContinueWith((result) => result / 10000);

            Assert.AreEqual(2000, task.Result);
            Assert.IsTrue(task.IsCompleted);

            Assert.AreEqual(10000, newTask1.Result);
            Assert.IsTrue(newTask1.IsCompleted);

            Assert.AreEqual(1, newTask2.Result);
            Assert.IsTrue(newTask2.IsCompleted);
        }
    }
}
