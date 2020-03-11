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
            myNUnit.RunTests("..\\..\\..\\TestProject.Tests\\bin");
            myNUnit.PrintReport();
        }
    }
}
