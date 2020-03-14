using System;
using System.Collections.Concurrent;
using System.Linq;
using BeforeAndAfterClassTestProject;
using BeforeAndAfterTestProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyNUnit.Tests
{
    [TestClass]
    public class MyNUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            myNUnit = new MyNUnit();
            testInfo = myNUnit.TestsInfo;
        }

        [TestMethod]
        public void PassedTestsTest()
        {
            myNUnit.RunTests("..\\..\\..\\PassedTestProject\\bin");
            var passedTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "PassedTests")
                {
                    passedTestInfo.Add(info);
                }
            }

            Assert.AreEqual(4, passedTestInfo.Count());

            var foundTests = new bool[4];
            foreach (var info in passedTestInfo)
            {
                for (var i = 0; i < 4; ++i)
                {
                    if (info.Name == $"PassedTest{i + 1}")
                    {
                        foundTests[i] = true;
                    }
                }

                Assert.IsTrue(info.IsPassed);
                Assert.IsFalse(info.IsIgnored);
                Assert.IsNull(info.IgnoringReason);
                Assert.AreNotEqual(TimeSpan.Zero, info.Time);
            }

            foreach (var foundTest in foundTests)
            {
                Assert.IsTrue(foundTest);
            }
        }

        [TestMethod]
        public void TimeTest()
        {
            myNUnit.RunTests("..\\..\\..\\PassedTestProject\\bin");
            TestInfo timeTest = null;

            foreach (var info in testInfo)
            {
                if (info.Name == "PassedTest4")
                {
                    timeTest = info;
                    break;
                }
            }

            Assert.IsNotNull(timeTest);
            Assert.IsTrue(TimeSpan.FromSeconds(1) <= timeTest.Time);
        }

        [TestMethod]
        public void FailedTestsTest()
        {
            myNUnit.RunTests("..\\..\\..\\FailedTestProject\\bin");
            var failedTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "FailedTests")
                {
                    failedTestInfo.Add(info);
                }
            }

            Assert.AreEqual(3, failedTestInfo.Count());

            var foundTests = new bool[3];
            foreach (var info in failedTestInfo)
            {
                for (var i = 0; i < 3; ++i)
                {
                    if (info.Name == $"FailedTest{i + 1}")
                    {
                        foundTests[i] = true;
                    }
                }

                Assert.IsFalse(info.IsPassed);
                Assert.IsFalse(info.IsIgnored);
                Assert.IsNull(info.IgnoringReason);
                Assert.AreNotEqual(TimeSpan.Zero, info.Time);
            }

            foreach (var foundTest in foundTests)
            {
                Assert.IsTrue(foundTest);
            }
        }

        [TestMethod]
        public void ExceptionTestsTest()
        {
            myNUnit.RunTests("..\\..\\..\\ExceptionTestProject\\bin");
            var exceptionTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "ExceptionTests")
                {
                    exceptionTestInfo.Add(info);
                }
            }

            Assert.AreEqual(3, exceptionTestInfo.Count());
            var foundTests = new bool[3];
            foreach (var info in exceptionTestInfo)
            {
                for (var i = 0; i < 3; ++i)
                {
                    if (info.Name == $"ExceptionTest{i + 1}")
                    {
                        foundTests[i] = true;
                    }
                }

                if (info.Name == "ExceptionTest3")
                {
                    Assert.IsFalse(info.IsPassed);
                }
                else
                {
                    Assert.IsTrue(info.IsPassed);
                }
                Assert.IsFalse(info.IsIgnored);
                Assert.IsNull(info.IgnoringReason);
                Assert.AreNotEqual(TimeSpan.Zero, info.Time);
            }

            foreach (var foundTest in foundTests)
            {
                Assert.IsTrue(foundTest);
            }
        }

        [TestMethod]
        public void BeforeAndAfterClassMethodsTest()
        {
            BeforeAndAfterClassTests.TestArray = new int[] { 0, 0, 0 };
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterClassTestProject\\bin");

            for (var i = 0; i < 3; ++i)
            {
                Assert.AreEqual(i + 1, BeforeAndAfterClassTests.TestArray[i]);
            }
            Assert.AreEqual(4, BeforeAndAfterClassTests.Count);
        }

        [TestMethod]
        public void BeforeAndAfterMethodsTest()
        {
            BeforeAndAfterTests.TestArray = new int[] { 0, 0, 0 };
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterTestProject\\bin");

            for (var i = 0; i < 3; ++i)
            {
                Assert.AreEqual(i + 1, BeforeAndAfterTests.TestArray[i]);
            }
            Assert.AreEqual(6, BeforeAndAfterTests.Count);
        }

        [TestMethod]
        public void IgnoreTest()
        {
            myNUnit.RunTests("..\\..\\..\\IgnoreTestProject\\bin");
            var ignoreTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "IgnoreTests")
                {
                    ignoreTestInfo.Add(info);
                }
            }

            Assert.AreEqual(2, ignoreTestInfo.Count());

            var foundTests = new bool[2];
            foreach (var info in ignoreTestInfo)
            {
                for (var i = 0; i < 2; ++i)
                {
                    if (info.Name == $"IgnoreTest{i + 1}")
                    {
                        foundTests[i] = true;
                    }
                }

                Assert.IsTrue(info.IsPassed);
                Assert.IsTrue(info.IsIgnored);
                Assert.IsNotNull(info.IgnoringReason);
                Assert.AreEqual(TimeSpan.Zero, info.Time);
            }

            foreach (var foundTest in foundTests)
            {
                Assert.IsTrue(foundTest);
            }
        }

        private MyNUnit myNUnit;
        private ConcurrentBag<TestInfo> testInfo;
    }
}
