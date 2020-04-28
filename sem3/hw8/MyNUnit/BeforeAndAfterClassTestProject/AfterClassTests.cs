using Attributes;
using System;

namespace BeforeAndAfterClassTestProject
{
    public class AfterClassTests
    {
        private static int count = 0;

        [BeforeClass]
        public static void BeforeClassMethod() => count = 0;

        [Test]
        public void AfterClassTest1()
        {
            if (count != 0)
            {
                throw new Exception();
            }
        }

        [Test]
        public void AfterClassTest2()
        {
            if (count != 0)
            {
                throw new Exception();
            }
        }

        [AfterClass]
        public static void AfterClassMethod()
        {
            count = 1;
            AdditionalAfterClassTest.StartTest();
        }
    }
}
