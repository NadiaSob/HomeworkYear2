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
