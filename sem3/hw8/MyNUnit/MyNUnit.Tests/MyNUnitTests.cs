using System;
using System.Collections.Concurrent;
using System.Linq;
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
        public void AfterMethodsTest()
        {
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterTestProject\\bin");

            var afterTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "AfterTests")
                {
                    afterTestInfo.Add(info);
                }
            }
            Assert.AreEqual(2, afterTestInfo.Count());
            var foundTests = new bool[2];
            foreach (var info in afterTestInfo)
            {
                for (var i = 0; i < 2; ++i)
                {
                    if (info.Name == $"AfterTest{i + 1}")
                    {
                        foundTests[i] = true;
                    }
                }
                Assert.IsFalse(info.IsIgnored);
                Assert.IsNull(info.IgnoringReason);
                Assert.AreNotEqual(TimeSpan.Zero, info.Time);
            }

            afterTestInfo.TryTake(out var test1);
            afterTestInfo.TryTake(out var test2);
            Assert.IsTrue(test1.IsPassed || test2.IsPassed);
            Assert.IsTrue(!test1.IsPassed || !test2.IsPassed);

            foreach (var foundTest in foundTests)
            {
                Assert.IsTrue(foundTest);
            }
        }

        [TestMethod]
        public void BeforeMethodsTest()
        {
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterTestProject\\bin");

            var beforeTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "BeforeTests")
                {
                    beforeTestInfo.Add(info);
                }
            }
            Assert.AreEqual(1, beforeTestInfo.Count());
            beforeTestInfo.TryTake(out var beforeTest);

            Assert.AreEqual("BeforeTest", beforeTest.Name);
            Assert.IsTrue(beforeTest.IsPassed);
            Assert.IsFalse(beforeTest.IsIgnored);
            Assert.IsNull(beforeTest.IgnoringReason);
            Assert.AreNotEqual(TimeSpan.Zero, beforeTest.Time);
        }


        [TestMethod]
        public void BeforeClassMethodsTest()
        {
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterClassTestProject\\bin");

            var beforeClassTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "BeforeClassTests")
                {
                    beforeClassTestInfo.Add(info);
                }
            }
            Assert.AreEqual(3, beforeClassTestInfo.Count());
            var foundTests = new bool[3];
            foreach (var info in beforeClassTestInfo)
            {
                for (var i = 0; i < 3; ++i)
                {
                    if (info.Name == $"BeforeClassTest{i + 1}")
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
        public void AfterClassMethodsTest()
        {
            myNUnit.RunTests("..\\..\\..\\BeforeAndAfterClassTestProject\\bin");

            var afterClassTestInfo = new ConcurrentBag<TestInfo>();
            var additionalAfterClassTestInfo = new ConcurrentBag<TestInfo>();

            foreach (var info in testInfo)
            {
                if (info.ClassName == "AfterClassTests")
                {
                    afterClassTestInfo.Add(info);
                }
                if (info.ClassName == "AdditionalAfterClassTest")
                {
                    additionalAfterClassTestInfo.Add(info);
                }
            }
            Assert.AreEqual(2, afterClassTestInfo.Count());
            Assert.AreEqual(1, additionalAfterClassTestInfo.Count());
            var foundTests = new bool[2];
            foreach (var info in afterClassTestInfo)
            {
                for (var i = 0; i < 2; ++i)
                {
                    if (info.Name == $"AfterClassTest{i + 1}")
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

            additionalAfterClassTestInfo.TryTake(out var test3);
            Assert.AreEqual("AfterClassTest3", test3.Name);
            Assert.IsTrue(test3.IsPassed);
            Assert.IsFalse(test3.IsIgnored);
            Assert.IsNull(test3.IgnoringReason);
            Assert.AreNotEqual(TimeSpan.Zero, test3.Time);
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IncorrectMethodsTest()
        {
            myNUnit.RunTests("..\\..\\..\\IncorrectTestProject\\bin");
        }

        private MyNUnit myNUnit;
        private ConcurrentBag<TestInfo> testInfo;
    }
}
