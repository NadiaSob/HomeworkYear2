using System;

namespace Attributes
{
    /// <summary>
    /// Attribute for test method.
    /// </summary>
    public class TestAttribute : Attribute
    {
        /// <summary>
        /// Type of expected exception.
        /// </summary>
        public Type Expected { get; set; }

        /// <summary>
        /// Reason of test being ignored.
        /// </summary>
        public string Ignore { get; set; }

        public TestAttribute(Type expected) => Expected = expected;

        public TestAttribute(string ignore) => Ignore = ignore;

        public TestAttribute()
        {
        }
    }
}
