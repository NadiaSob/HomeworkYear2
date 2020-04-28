using Attributes;
using System;
using System.Threading;

namespace PassedTestProject
{
    public class PassedTests
    {
        private readonly int count = 10;

        [Test]
        public void PassedTest1()
        {
        }

        [Test]
        public void PassedTest2()
        {
            if (count != 10)
            {
                throw new Exception();
            }
        }

        [Test]
        public void PassedTest3()
        {
            var mustBeTrue = true;
            if (!mustBeTrue)
            {
                throw new Exception();
            }
        }

        [Test]
        public void PassedTest4() => Thread.Sleep(1000);
    }
}
