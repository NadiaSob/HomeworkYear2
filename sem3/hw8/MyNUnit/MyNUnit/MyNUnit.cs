﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using MyNUnit.Attributes;
using System.Diagnostics;

namespace MyNUnit
{
    public class MyNUnit
    {
        private readonly ConcurrentBag<TestInfo> testsInfo = new ConcurrentBag<TestInfo>();

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

            //Проверить на непустоту списков
            Parallel.ForEach(beforeClassMethods, method => RunMethod(method, null));
            Parallel.ForEach(testMethods, test => RunTest(test, instance, beforeMethods, afterMethods));
            Parallel.ForEach(afterClassMethods, method => RunMethod(method, null));
        }

        private void RunTest(MethodInfo test, object instance, List<MethodInfo> beforeMethods, List<MethodInfo> afterMethods)
        {
            Parallel.ForEach(beforeMethods, method => RunMethod(method, instance));

            var attribute = test.GetCustomAttribute<TestAttribute>();

            string className = test.DeclaringType.Name;
            string name = test.Name;
            bool isPassed = false;
            bool isIgnored = false;
            string ignoringReason = null;
            TimeSpan time = TimeSpan.Zero;

            TestInfo testInfo = null;

            if (attribute.Ignore != null)
            {
                isIgnored = true;
                ignoringReason = attribute.Ignore;
                isPassed = true;
                testInfo = new TestInfo(className, name, isPassed, isIgnored, ignoringReason, time);
                testsInfo.Add(testInfo);
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
                testsInfo.Add(testInfo);

                Parallel.ForEach(afterMethods, method => RunMethod(method, instance));
            }
        }

        private void RunMethod(MethodInfo method, object instance) => method.Invoke(instance, null);

        public void PrintReport()
        {
            Console.WriteLine($"Tests found: {testsInfo.Count()}");

            var allTestsPassed = true;

            foreach (var test in testsInfo)
            {
                Console.WriteLine($"Test: {test.Name}");
                Console.WriteLine($"Class: {test.ClassName}");
                Console.WriteLine($"Time: {test.Time.TotalSeconds} seconds");

                if (test.IsIgnored)
                {
                    Console.WriteLine($"Test is ignored because: {test.IgnoringReason}");
                }
                else
                {
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

            if (testsInfo.Count() != 0)
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
    }
}
