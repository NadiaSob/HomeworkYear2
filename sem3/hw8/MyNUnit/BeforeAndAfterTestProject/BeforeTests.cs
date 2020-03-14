using MyNUnit.Attributes;
using System;

namespace BeforeAndAfterTestProject
{
    public class BeforeTests
    {
        private volatile int count = 0;

        [Before]
        public void BeforeMethod() => count = 10;

        [Test]
        public void BeforeTest()
        {
            if (count != 10)
            {
                throw new Exception();
            }
        }
    }
}
