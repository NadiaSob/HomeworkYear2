using MyNUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
