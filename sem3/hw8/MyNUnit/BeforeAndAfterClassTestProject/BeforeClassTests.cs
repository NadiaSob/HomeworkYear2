using MyNUnit.Attributes;
using System;

namespace BeforeAndAfterClassTestProject
{
    public class BeforeClassTests
    {
        private static int count = 0;

        [BeforeClass]
        public static void BeforeClassMethod() => count = 20;

        [Test]
        public void BeforeClassTest1()
        {
            if (count != 20)
            {
                throw new Exception();
            }
        }

        [Test]
        public void BeforeClassTest2()
        {
            if (count != 20)
            {
                throw new Exception();
            }
        }

        [Test]
        public void BeforeClassTest3()
        {
            if (count != 20)
            {
                throw new Exception();
            }
        }
    }
}
