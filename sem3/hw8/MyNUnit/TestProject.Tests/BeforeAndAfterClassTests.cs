using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;

namespace TestProject.Tests
{
    public class BeforeAndAfterClassTests
    {
        public static ConcurrentQueue<int> TestQueue { get; private set; }
            = new ConcurrentQueue<int>();

        [BeforeClass]
        public static void BeforeClassMethod()
        {
            TestQueue = new ConcurrentQueue<int>();
            TestQueue.Enqueue(1);
        }

        [Test]
        public void BeforeAndAfterClassTest1() => TestQueue.Enqueue(2);

        [Test]
        public void BeforeAndAfterClassTest2() => TestQueue.Enqueue(2);

        [AfterClass]
        public static void AfterClassMethod() => TestQueue.Enqueue(3);
    }
}
