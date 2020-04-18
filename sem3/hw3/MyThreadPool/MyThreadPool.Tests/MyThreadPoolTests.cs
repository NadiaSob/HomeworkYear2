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
        public void IncorrectNumberOfThreadsTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new MyThreadPool(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new MyThreadPool(-3));
        }

        [TestMethod]
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

            Assert.ThrowsException<InvalidOperationException>(() => pool.AddTask(() => 31 + 30));
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

        [TestMethod]
        public void ThreadSafetyTest()
        {
            var pool = new MyThreadPool(3);
            var threads = new Thread[4];
            var tasks = new IMyTask<int>[4];

            for (var i = 0; i < 4; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    tasks[localI] = pool.AddTask(() => (localI + 1) * 3);
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (var i = 0; i < 4; ++i)
            {
                Assert.AreEqual((i + 1) * 3, tasks[i].Result);
                Assert.IsTrue(tasks[i].IsCompleted);
            }
        }

        [TestMethod]
        public void ThreadSafetyShutDownTest()
        {
            var pool = new MyThreadPool(4);
            var threads = new Thread[7];
            var tasks = new IMyTask<int>[7];
            var manualResetEvent = new ManualResetEvent(false);

            for (var i = 0; i < 7; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    tasks[localI] = pool.AddTask(() =>
                    {
                        manualResetEvent.WaitOne();
                        Thread.Sleep(50);
                        return localI + 10; 
                    });
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Thread.Sleep(500);
            manualResetEvent.Set();
            pool.Shutdown();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (var i = 0; i < 7; ++i)
            {
                Assert.AreEqual(i + 10, tasks[i].Result);
                Assert.IsTrue(tasks[i].IsCompleted);
            }

            Assert.ThrowsException<InvalidOperationException>(() => pool.AddTask(() => 10 + 10));
        }

        [TestMethod]
        public void ThreadSafetyContinueWithTest()
        {
            var pool = new MyThreadPool(3);
            var threads = new Thread[5];
            var tasks = new IMyTask<int>[5];
            var manualResetEvent = new ManualResetEvent(false);
            threads[0] = new Thread(() => tasks[0] = pool.AddTask(() => 10));

            for (var i = 1; i < 5; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    tasks[localI] = tasks[localI - 1].ContinueWith((result) =>
                    {
                        manualResetEvent.WaitOne();
                        return result + localI;
                    });
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
                Thread.Sleep(60);
            }

            Thread.Sleep(500);
            manualResetEvent.Set();
            pool.Shutdown();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            var temp = 10;
            for (var i = 0; i < 5; ++i)
            {
                Assert.AreEqual(temp + i, tasks[i].Result);
                Assert.IsTrue(tasks[i].IsCompleted);
                temp += i;
            }

            Assert.ThrowsException<InvalidOperationException>(
                () => tasks[4].ContinueWith((result) => result + 1));
        }
    }
}
