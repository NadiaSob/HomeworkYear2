using System;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Before starting set the assemblies path into the command line arguments.");
                return;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("You must specify only one path to assemblies in the command line arguments.");
                return;
            }

            var myNUnit = new MyNUnit();
            var path = args[0];

            myNUnit.RunTests(path);
            myNUnit.PrintReport();
        }
    }
}
