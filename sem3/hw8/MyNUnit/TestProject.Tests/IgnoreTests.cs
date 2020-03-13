using System;
using MyNUnit.Attributes;

namespace TestProject.Tests
{
    public class IgnoreTests
    {
        [Test("Because I want to")]
        public void IgnoreTest1() => throw new Exception();

        [Test("")]
        public void IgnoreTest2()
        {
        }
    }
}
