using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Class containing information about the test.
    /// </summary>
    public class TestInfo
    {
        /// <summary>
        /// Name of class containing the test.
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// Name of the test.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates whether the test is passed.
        /// </summary>
        public bool IsPassed { get; private set; }

        /// <summary>
        /// Indicates whether the test is ignored.
        /// </summary>
        public bool IsIgnored { get; private set; }

        /// <summary>
        /// The reason of the test being ignored if it is.
        /// </summary>
        public string IgnoringReason { get; private set; }

        /// <summary>
        /// Test execution time.
        /// </summary>
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
