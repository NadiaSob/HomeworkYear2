using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using MyNUnit.Attributes;
using System.Diagnostics;

namespace MyNUnit
{
    /// <summary>
    /// Class that runs tests in all assemblies located in the given path.
    /// </summary>
    public class MyNUnit
    {
        /// <summary>
        /// Collection of the TestsInfo elements.
        /// </summary>
        public ConcurrentBag<TestInfo> TestsInfo { get; private set; } = new ConcurrentBag<TestInfo>();

        /// <summary>
        /// Runs tests in all assemblies located in the given path.
        /// </summary>
        /// <param name="path">Path in which the assemblies are located.</param>
        public void RunTests(string path)
        {
            var types = GetAssemblies(path).SelectMany(a => a.GetTypes());
            Parallel.ForEach(types, RunMethodsWithAttributes);
        }

        private List<Assembly> GetAssemblies(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            var loadedAssemblies = new List<Assembly>();
            var assemblies = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);

            if (assemblies.Count() == 0)
            {
                throw new FileNotFoundException("No assembly is found");
            }

            foreach (var assembly in assemblies)
            {
                loadedAssemblies.Add(Assembly.LoadFrom(assembly));
            }
            return loadedAssemblies;
        }

        private void RunMethodsWithAttributes(Type type)
        {
            var beforeClassMethods = new List<MethodInfo>();
            var afterClassMethods = new List<MethodInfo>();

            var beforeMethods = new List<MethodInfo>();
            var afterMethods = new List<MethodInfo>();

            var testMethods = new List<MethodInfo>();

            foreach (var methodInfo in type.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(methodInfo))
                {
                    switch (attribute.GetType())
                    {
                        case Type beforeClassAtr when beforeClassAtr == typeof(BeforeClassAttribute):
                            beforeClassMethods.Add(methodInfo);
                            break;

                        case Type afterClassAtr when afterClassAtr == typeof(AfterClassAttribute):
                            afterClassMethods.Add(methodInfo);
                            break;

                        case Type testAtr when testAtr == typeof(TestAttribute):
                            testMethods.Add(methodInfo);
                            break;

                        case Type beforeAtr when beforeAtr == typeof(BeforeAttribute):
                            beforeMethods.Add(methodInfo);
                            break;

                        case Type afterAtr when afterAtr == typeof(AfterAttribute):
                            afterMethods.Add(methodInfo);
                            break;
                    }
                }
            }

            var instance = Activator.CreateInstance(type);

            if (beforeClassMethods.Count() != 0)
            {
                Parallel.ForEach(beforeClassMethods, method =>
                {
                    CheckMethodToBeCorrect(method);
                    if (!method.IsStatic)
                    {
                        throw new InvalidOperationException("BeforeClass method must be static");
                    }
                    RunMethod(method, null);
                });
            }

            if (testMethods.Count() != 0)
            {
                Parallel.ForEach(testMethods, test =>
                {
                    CheckMethodToBeCorrect(test);
                    RunTest(test, instance, beforeMethods, afterMethods);
                });
            }

            if (afterClassMethods.Count() != 0)
            {
                Parallel.ForEach(afterClassMethods, method =>
                {
                    CheckMethodToBeCorrect(method);
                    if (!method.IsStatic)
                    {
                        throw new InvalidOperationException("AfterClass method must be static");
                    }
                    RunMethod(method, null);
                });
            }
        }

        private void RunTest(MethodInfo test, object instance, List<MethodInfo> beforeMethods, List<MethodInfo> afterMethods)
        {
            if (beforeMethods.Count() != 0)
            {
                Parallel.ForEach(beforeMethods, method =>
                {
                    CheckMethodToBeCorrect(method);
                    RunMethod(method, instance);
                });
            }

            var attribute = test.GetCustomAttribute<TestAttribute>();

            var className = test.DeclaringType.Name;
            var name = test.Name;
            var isPassed = false;
            var isIgnored = false;
            var time = TimeSpan.Zero;
            string ignoringReason = null;

            TestInfo testInfo = null;

            if (attribute.Ignore != null)
            {
                isIgnored = true;
                ignoringReason = attribute.Ignore;
                isPassed = true;
                testInfo = new TestInfo(className, name, isPassed, isIgnored, ignoringReason, time);
                TestsInfo.Add(testInfo);
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                RunMethod(test, instance);

                if (attribute.Expected == null)
                {
                    isPassed = true;
                    stopwatch.Stop();
                }
            }
            catch (Exception exception)
            {
                if (attribute.Expected == exception.InnerException.GetType())
                {
                    stopwatch.Stop();
                    isPassed = true;
                }
            }
            finally
            {
                stopwatch.Stop();
                time = stopwatch.Elapsed;
                testInfo = new TestInfo(className, name, isPassed, isIgnored, ignoringReason, time);
                TestsInfo.Add(testInfo);

                if (afterMethods.Count() != 0)
                {
                    Parallel.ForEach(afterMethods, method =>
                    {
                        CheckMethodToBeCorrect(method);
                        RunMethod(method, instance);
                    });
                }
            }
        }

        private void RunMethod(MethodInfo method, object instance) => method.Invoke(instance, null);

        /// <summary>
        /// Prints a report about the result of tests execution. 
        /// </summary>
        public void PrintReport()
        {
            Console.WriteLine($"Tests found: {TestsInfo.Count()}");
            Console.WriteLine();

            var allTestsPassed = true;

            foreach (var test in TestsInfo)
            {
                Console.WriteLine($"Test: {test.Name}");
                Console.WriteLine($"Class: {test.ClassName}");

                if (test.IsIgnored)
                {
                    Console.WriteLine("Test is ignored");
                    Console.WriteLine($"Reason of being ignored: {test.IgnoringReason}");
                }
                else
                {
                    Console.WriteLine($"Time: {test.Time.TotalSeconds} seconds");
                    if (test.IsPassed)
                    {
                        Console.WriteLine("Passed!");
                    }
                    else
                    {
                        Console.WriteLine("Failed!");
                        allTestsPassed = false;
                    }
                }
                Console.WriteLine();
            }

            if (TestsInfo.Count() != 0)
            {
                if (allTestsPassed)
                {
                    Console.WriteLine("All tests have passed!");
                }
                else
                {
                    Console.WriteLine("Some tests have failed!");
                }
            }
        }
        
        private void CheckMethodToBeCorrect(MethodInfo method)
        {
            if (method.ReturnType != typeof(void))
            {
                throw new InvalidOperationException($"Method {method.Name} must not return value");
            }

            if (method.GetParameters().Length != 0)
            {
                throw new InvalidOperationException($"Method {method.Name} must not have any input parameters");
            }
        }
    }
}
