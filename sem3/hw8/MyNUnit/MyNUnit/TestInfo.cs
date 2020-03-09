using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class TestInfo
    {
        public string ClassName { get; private set; }

        public string Name { get; private set; }

        public bool IsPassed { get; private set; }

        public bool IsIgnored { get; private set; }

        public string IgnoringReason { get; private set; }

        public TimeSpan Time { get; private set; }

        public TestInfo(string className, string name, bool isPassed, 
            bool isIgnored, string ignoringReason, TimeSpan time)
        {
            ClassName = className;
            Name = name;
            IsPassed = isPassed;
            IsIgnored = isIgnored;
            IgnoringReason = ignoringReason;
            Time = time;
        }
    }
}
