using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter an argument.");
                return;
            }

            var path = args[0];

            Console.WriteLine("File system directory:");
            Console.WriteLine(path);

            var checksumCalculator = new ChecksumCalculator(path);
            Console.WriteLine($"Checksum is {checksumCalculator.Calculate()}");

            var multithreadedChecksumCalculator = new MultithreadedChecksumCalculator(path);

            Stopwatch stopWatch = new Stopwatch();
            checksumCalculator.Calculate();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime of single-threaded calculation" + elapsedTime);

            stopWatch = new Stopwatch();
            multithreadedChecksumCalculator.Calculate();
            ts = stopWatch.Elapsed;

            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime of multi-threaded calculation" + elapsedTime);
        }
    }
}
