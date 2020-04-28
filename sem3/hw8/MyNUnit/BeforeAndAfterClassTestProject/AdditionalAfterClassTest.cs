using Attributes;
using System;
using System.Threading;

namespace BeforeAndAfterClassTestProject
{
    public class AdditionalAfterClassTest
    {
        private static bool testStarted = false;

        private int count = 0;

        public static void StartTest() => testStarted = true;

        [Test]
        public void AfterClassTest3()
        {
            while (!testStarted)
            {
                Thread.Sleep(100);
                Interlocked.Increment(ref count);
                if (count >= 60)
                {
                    throw new Exception();
                }
            }
        }
    }
}
