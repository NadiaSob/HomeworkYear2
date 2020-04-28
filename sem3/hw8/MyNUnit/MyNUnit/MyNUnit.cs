using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Attributes;

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
        public IEnumerable<TestInfo> TestsInfo = new ConcurrentBag<TestInfo>();

        /// <summary>
        /// Runs tests in all assemblies located in the given path.
        /// </summary>
        /// <param name="path">Path in which the assemblies are located.</param>
        public void RunTests(string path)
        {
            var types = GetAssemblies(path).SelectMany(a => a.GetTypes());
            try
            {
                Parallel.ForEach(types, RunMethodsWithAttributes);
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
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

            Parallel.ForEach(assemblies, assembly => loadedAssemblies.Add(Assembly.LoadFrom(assembly)));
            return loadedAssemblies;
        }

        private void RunMethodsWithAttributes(Type type)
        {
            var beforeClassMethods = new List<MethodInfo>();
            var afterClassMethods = new List<MethodInfo>();

            var beforeMethods = new List<MethodInfo>();
            var afterMethods = new List<MethodInfo>();

            var testMethods = new List<MethodInfo>();

            DistributeMethods(type, beforeClassMethods, afterClassMethods, beforeMethods, afterMethods, testMethods);

            if (beforeMethods.Count > 1 || afterMethods.Count > 1)
            {
                throw new InvalidOperationException("There must not be more than one Before and After methods in one class.");
            }

            RunMethodsWithOneAttribute(beforeClassMethods, null);

            var instance = Activator.CreateInstance(type);

            if (testMethods.Count() != 0)
            {
                try
                {
                    Parallel.ForEach(testMethods, test => RunTest(test, instance, beforeMethods, afterMethods));
                }
                catch (AggregateException exception)
                {
                    throw exception.InnerException;
                }
            }

            RunMethodsWithOneAttribute(afterClassMethods, null);
        }

        private void DistributeMethods(Type type, List<MethodInfo> beforeClassMethods, List<MethodInfo> afterClassMethods,
            List<MethodInfo> beforeMethods, List<MethodInfo> afterMethods, List<MethodInfo> testMethods)
        {
            foreach (var methodInfo in type.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(methodInfo))
                {
                    switch (attribute.GetType())
                    {
                        case Type beforeClassAtr when beforeClassAtr == typeof(BeforeClassAttribute):
                            CheckMethodToBeCorrect<BeforeClassAttribute>(methodInfo);
                            beforeClassMethods.Add(methodInfo);
                            break;

                        case Type afterClassAtr when afterClassAtr == typeof(AfterClassAttribute):
                            CheckMethodToBeCorrect<AfterClassAttribute>(methodInfo);
                            afterClassMethods.Add(methodInfo);
                            break;

                        case Type testAtr when testAtr == typeof(TestAttribute):
                            CheckMethodToBeCorrect<TestAttribute>(methodInfo);
                            testMethods.Add(methodInfo);
                            break;

                        case Type beforeAtr when beforeAtr == typeof(BeforeAttribute):
                            CheckMethodToBeCorrect<BeforeAttribute>(methodInfo);
                            beforeMethods.Add(methodInfo);
                            break;

                        case Type afterAtr when afterAtr == typeof(AfterAttribute):
                            CheckMethodToBeCorrect<AfterAttribute>(methodInfo);
                            afterMethods.Add(methodInfo);
                            break;
                    }
                }
            }
        }

        private void RunTest(MethodInfo test, object instance, List<MethodInfo> beforeMethods, List<MethodInfo> afterMethods)
        {
            RunMethodsWithOneAttribute(beforeMethods, instance);

            var attribute = test.GetCustomAttribute<TestAttribute>();

            var className = test.DeclaringType.Name;
            var name = test.Name;
            var isPassed = false;
            var isIgnored = false;
            var time = TimeSpan.Zero;
            string ignoringReason = null;
            Exception testException = null;

            TestInfo testInfo = null;

            if (attribute.Ignore != null)
            {
                isIgnored = true;
                ignoringReason = attribute.Ignore;
                isPassed = true;
                testInfo = new TestInfo(className, name, isPassed, isIgnored, ignoringReason, time, testException);
                ((ConcurrentBag<TestInfo>)TestsInfo).Add(testInfo);
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
                else
                {
                    testException = exception.InnerException;
                }
            }
            finally
            {
                stopwatch.Stop();
                time = stopwatch.Elapsed;
                testInfo = new TestInfo(className, name, isPassed, isIgnored, ignoringReason, time, testException);
                ((ConcurrentBag<TestInfo>)TestsInfo).Add(testInfo);

                RunMethodsWithOneAttribute(afterMethods, instance);
            }
        }

        private void RunMethodsWithOneAttribute(List<MethodInfo> methods, object instance)
        {
            if (methods.Count() != 0)
            {
                try
                {
                    Parallel.ForEach(methods, method => RunMethod(method, instance));
                }
                catch (AggregateException exception)
                {
                    throw exception.InnerException;
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
                        Console.WriteLine($"Test has thrown an exception: {test.Exception}");
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
        
        private void CheckMethodToBeCorrect<T>(MethodInfo method) where T : Attribute
        {
            if (method.ReturnType != typeof(void))
            {
                throw new InvalidOperationException($"Method {method.Name} must not return value");
            }

            if (method.GetParameters().Length != 0)
            {
                throw new InvalidOperationException($"Method {method.Name} must not have any input parameters");
            }

            if (typeof(T) == typeof(BeforeClassAttribute) || typeof(T) == typeof(AfterClassAttribute))
            {
                if (!method.IsStatic)
                {
                    throw new InvalidOperationException("BeforeClass and AfterClass methods must be static");
                }
            }
        }
    }
}
