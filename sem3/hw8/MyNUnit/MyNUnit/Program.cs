using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            var myNUnit = new MyNUnit();
            var path = args[0];

            myNUnit.RunTests(path);
            myNUnit.PrintReport();
        }
    }
}
