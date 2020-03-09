using MyNUnit.Attributes;
using System;

namespace TestProject.Tests
{
    public class TestProjectTests
    {
        [Test]
        public void PassedTest()
        {
        }

        [Test]
        public void FailedTest()
        {
            throw new InvalidOperationException();
        }
    }
}
