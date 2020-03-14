using MyNUnit.Attributes;
using System.Collections.Concurrent;

namespace TestProject.Tests
{
    public class BeforeAndAfterClassTests
    {
        public static int[] TestArray { get; private set; }
        public static int Count { get; set; }
        private static readonly object lockObject = new object();

        [BeforeClass]
        public static void BeforeClassMethod()
        {
            lock (lockObject)
            {
                TestArray = new int[3];
                TestArray[0] = 1;
                Count = 1;
            }
        }

        [Test]
        public void BeforeAndAfterClassTest1()
        {
            lock (lockObject)
            {
                TestArray[1] = 2;
                ++Count;
            }
        }

        [Test]
        public void BeforeAndAfterClassTest2()
        {
            lock (lockObject)
            {
                ++Count;
            }
        }

        [AfterClass]
        public static void AfterClassMethod()
        {
            lock (lockObject)
            {
                TestArray[2] = 3;
                ++Count;
            }
        }
    }
}
