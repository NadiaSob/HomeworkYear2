using MyNUnit.Attributes;
using System.Collections.Concurrent;
using System.Threading;

namespace TestProject.Tests
{
    public class BeforeAndAfterTests
    {
        public static int[] TestArray { get; private set; }
        public static int Count { get; set; }
        private static readonly object lockObject = new object();

        [BeforeClass]
        public static void EmptyTestQueue()
        {
            lock (lockObject)
            {
                TestArray = new int[3];
                Count = 0;
            }
        }

        [Before]
        public static void BeforeMethod()
        {
            lock (lockObject)
            {
                TestArray[0] = 1;
                ++Count;
            }
        }

        [Test]
        public void BeforeAndAfterTest1()
        {
            lock (lockObject)
            {
                TestArray[1] = 2;
                ++Count;
            }
        }

        [Test]
        public void BeforeAndAfterTest2()
        {
            lock (lockObject)
            {
                ++Count;
            }
        }

        [After]
        public void AfterMethod()
        {
            lock (lockObject)
            {
                TestArray[2] = 3;
                ++Count;
            }
        }
    }
}
