using System;
using DesignPattern.DesignPrinciples;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            PrincipleDemo();
        }

        public static void PrincipleDemo()
        {
            var lsp = new LiskovSubstitutionPrinciple();
            lsp.OverloadDemo();

            var ocp = new OpenClosedPrinciple();
            ocp.MockSell();
            ocp.MockOffSell();
        }
    }
}
