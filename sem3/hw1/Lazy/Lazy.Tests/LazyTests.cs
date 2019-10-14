using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lazy.Tests
{
    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void SimpleSupplierLazyTest()
        {
            lazy = LazyFactory<int>.CreateLazy(() => 123);
            Assert.AreEqual(123, lazy.Get());
        }

        [TestMethod]
        public void AdditionLazyTest()
        {
            lazy = LazyFactory<int>.CreateLazy(() => 123 + 77);
            Assert.AreEqual(200, lazy.Get());
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DevideByZeroTest()
        {
            int zero = 0;
            lazy = LazyFactory<int>.CreateLazy(() => 1 / zero);
            lazy.Get();
        }

        [TestMethod]
        public void SupplierReturningNullTest()
        {
            var stringLazy = LazyFactory<string>.CreateLazy(() => null);
            Assert.IsNull(stringLazy.Get());
        }

        [TestMethod]
        public void ReturningSameResultSeveralTimesTest()
        {
            var stringLazy = LazyFactory<string>.CreateLazy(() => "Test string");
            var firstGet = stringLazy.Get();
            var secondGet = stringLazy.Get();
            Assert.AreEqual(firstGet, secondGet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptySupplierTest()
        {
            lazy = LazyFactory<int>.CreateLazy(null);
        }

        [TestMethod]
        public void ResultIsCalculatedOnlyOnceTest()
        {
            int count = 0;
            lazy = LazyFactory<int>.CreateLazy(() => ++count);

            Assert.AreEqual(1, lazy.Get());
            Assert.AreEqual(1, lazy.Get());
            Assert.AreEqual(1, lazy.Get());
        }

        private ILazy<int> lazy;
    }
}
