using System;
using System.Collections.Generic;
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

            Console.WriteLine("File system directory:");
            Console.WriteLine(args[0]);

            var checksumCalculator = new ChecksumCalculator(args[0]);
            Console.WriteLine($"Checksum is {checksumCalculator.Calculate()}");
        }
    }
}
