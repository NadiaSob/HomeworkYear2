using MyNUnit.Attributes;
using System;
using System.Threading;

namespace TestProject.Tests
{
    public class TestProjectTests
    {
        private static int count = 0;
        private static int count1 = 0;

        [BeforeClass]
        public static void BeforeClassMethod()
        {
            Interlocked.Increment(ref count);
        }

        [Test]
        public void PassedTest()
        {
        }

        [Test]
        public void FailedTest()
        {
            throw new InvalidOperationException();
        }

        [Test]
        public void PassedTestAfterBeforeClassMethod()
        {
            if (count != 1)
            {
                throw new InvalidOperationException();
            }
        }

        [AfterClass]
        public static void AfterClassMethod()
        {
            Interlocked.Decrement(ref count);
        }

        [Before]
        public void BeforeMethod()
        {
            Thread.Sleep(1000);
            count1 = 8;
        }

        [After]
        public void AfterMethod()
        {
            Thread.Sleep(1000);
            count1 = 0;
        }

        [Test]
        public void PassedTestAfterBeforeMethod()
        {
            if (count1 != 8)
            {
                throw new InvalidOperationException();
            }
        }

        [Test]
        public void PassedTestAfterAfterMethod()
        {
            if (count1 != 8)
            {
                throw new InvalidOperationException();
            }
        }

        [Test("Because I want to")]
        public void IgnoringTest()
        {
            throw new InvalidOperationException();
        }

        [Test(typeof(InvalidOperationException))]
        public void ExceptionTest()
        {
            throw new InvalidOperationException();
        }
    }
}
