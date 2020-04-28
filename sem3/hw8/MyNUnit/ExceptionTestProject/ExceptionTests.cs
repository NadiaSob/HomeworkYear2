using Attributes;
using System;

namespace ExceptionTestProject
{
    public class ExceptionTests
    {
        [Test(typeof(InvalidOperationException))]
        public void ExceptionTest1() => throw new InvalidOperationException();

        [Test(typeof(FormatException))]
        public void ExceptionTest2() => throw new FormatException();

        [Test(typeof(IndexOutOfRangeException))]
        public void ExceptionTest3() => throw new FormatException();
    }
}
