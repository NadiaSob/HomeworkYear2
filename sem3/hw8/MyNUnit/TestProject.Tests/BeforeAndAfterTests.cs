using MyNUnit.Attributes;
using System.Collections.Concurrent;

namespace TestProject.Tests
{
    public class BeforeAndAfterTests
    {
        public static ConcurrentQueue<int> TestQueue { get; private set; }
            = new ConcurrentQueue<int>();

        [BeforeClass]
        public static void EmptyTestQueue() => TestQueue = new ConcurrentQueue<int>();

        [Before]
        public static void BeforeMethod() => TestQueue.Enqueue(1);

        [Test]
        public void BeforeAndAfterTest1() => TestQueue.Enqueue(2);

        [Test]
        public static void BeforeAndAfterTest2() => TestQueue.Enqueue(2);

        [After]
        public void AfterMethod() => TestQueue.Enqueue(3);
    }
}
