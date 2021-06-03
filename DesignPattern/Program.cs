using System;
using DesignPattern.DesignPrinciples;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            LSPDemo();
        }

        public static void LSPDemo()
        {
            var lsp = new LiskovSubstitutionPrinciple();
            lsp.OverloadDemo();

        }
    }
}
