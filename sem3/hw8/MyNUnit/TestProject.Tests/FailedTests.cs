using System;
using MyNUnit.Attributes;

namespace TestProject.Tests
{
    public class FailedTests
    {
        private readonly int count = 100;

        [Test]
        public void FailedTest1() => throw new Exception();

        [Test]
        public void FailedTest2()
        {
            if (count == 100)
            {
                throw new Exception();
            }
        }

        [Test]
        public void FailedTest3()
        {
            var mustBeFalse = true;
            if (mustBeFalse)
            {
                throw new Exception();
            }
        }
    }
}
