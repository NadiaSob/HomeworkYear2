using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit.Attributes
{
    public class TestAttribute : Attribute
    {
        public Type Expected { get; set; }

        public string Ignore { get; set; }

        public TestAttribute(Type expected) => Expected = expected;

        public TestAttribute(string ignore) => Ignore = ignore;

        public TestAttribute()
        {
        }
    }
}
