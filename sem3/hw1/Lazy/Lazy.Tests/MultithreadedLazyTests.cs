using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Lazy.Tests
{
    [TestClass]
    public class MultithreadedLazyTests
    {
        [TestMethod]
        public void OneThreadSimpleSupplierTest()
        {
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => 100);
            Assert.AreEqual(100, lazy.Get());
        }

        [TestMethod]
        public void SeveralThreadsSimpleSupplierTest()
        {
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => 100);
            threads = new Thread[4];
            for (var i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.AreEqual(100, lazy.Get());
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
        }

        [TestMethod]
        public void OneThreadComplexCalculationTest()
        {
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => 8 * 5 + 1000 - 40);
            Assert.AreEqual(1000, lazy.Get());
        }

        [TestMethod]
        public void SeveralThreadsComplexCalculationTest()
        {
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => 8 * 5 + 1000 - 40);
            threads = new Thread[4];
            for (var i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.AreEqual(1000, lazy.Get());
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
        }

        [TestMethod]
        public void OneThreadResultIsCalculatedOnlyOnceTest()
        {
            int count = 0;
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => ++count);

            Assert.AreEqual(1, lazy.Get());
            Assert.AreEqual(1, lazy.Get());
            Assert.AreEqual(1, lazy.Get());
        }

        [TestMethod]
        public void SeveralThreadsResultIsCalculatedOnlyOnceTest()
        {
            int count = 0;
            lazy = LazyFactory<int>.CreateMultithreadedLazy(() => ++count);

            threads = new Thread[4];
            for (var i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.AreEqual(1, lazy.Get());
                    Assert.AreEqual(1, lazy.Get());
                    Assert.AreEqual(1, lazy.Get());
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
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptySupplierTest()
        {
            lazy = LazyFactory<int>.CreateLazy(null);
        }

        private ILazy<int> lazy;
        private Thread[] threads;
    }
}
