using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.Tests;

namespace MyNUnit.Tests
{
    [TestClass]
    public class MyNUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            myNUnit = new MyNUnit();
            myNUnit.RunTests("..\\..\\..\\TestProject.Tests\\bin");
            testInfo = myNUnit.TestsInfo;
        }

        [TestMethod]
        public void TestsCountTest() => Assert.AreEqual(16, testInfo.Count());

        [TestMethod]
        public void PassedTestsTest()
        {
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
            var testArray = new int[4] { 1, 2, 2, 3 };

            CollectionAssert.AreEqual(testArray, BeforeAndAfterClassTests.TestQueue.ToArray());
        }

        [TestMethod]
        public void BeforeAndAfterMethodsTest()
        {
            var testArray = new int[6] { 1, 2, 3, 1, 2, 3 };

            CollectionAssert.AreEqual(testArray, BeforeAndAfterTests.TestQueue.ToArray());
        }

        [TestMethod]
        public void IgnoreTest()
        {
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
