using MyNUnit.Attributes;
using System;
using System.Threading;

namespace BeforeAndAfterTestProject
{
    public class AfterTests
    {
        private volatile int count = 0;

        [Test]
        public void AfterTest1()
        {
            if (count != 100)
            {
                throw new Exception();
            }
        }

        [Test]
        public void AfterTest2()
        {
            Thread.Sleep(200);
            if (count != 100)
            {
                throw new Exception();
            }
        }

        [After]
        public void AfterMethod() => count = 100;
    }
}
