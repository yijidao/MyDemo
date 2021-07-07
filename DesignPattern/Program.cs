using System;
using DesignPattern.DesignPattern;
using DesignPattern.DesignPrinciples;

namespace DesignPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //PrincipleDemo();
            //ProxyPatternDemo();
            //MediatorPatternDemo();
            //ResponsibilityChainDemo();
            //DecoratorDemo();
            //StrategyDemo();
            //AdapterDemo();
            //InteratorDemo();
            CompositeDemo();
            Console.ReadLine();
        }

        public static void PrincipleDemo()
        {
            var lsp = new LiskovSubstitutionPrinciple();
            lsp.OverloadDemo();

            var ocp = new OpenClosedPrinciple();
            ocp.MockSell();
            ocp.MockOffSell();
        }

        public static void ProxyPatternDemo()
        {
            var pp = new ProxyPattern();
            pp.DynimicProxy();
        }

        public static void MediatorPatternDemo()
        {
            var mp = new MediatorPattern();
            //mp.CouplePssDemo();

            mp.MpPssDemo();
        }

        public static void ResponsibilityChainDemo()
        {
            var rc = new ChainOfResponsibilityPattern();
            rc.Demo();
        }

        public static void DecoratorDemo()
        {
            var dp = new DecoratorPattern();
            dp.DecoratorDemo();
        }

        public static void StrategyDemo()
        {
            var sp = new StrategyPattern();
            sp.StrategyDemo();
        }

        public static void AdapterDemo()
        {
            var ap = new AdapterPattern();
            ap.AdapterDemo();
        }

        public static void InteratorDemo()
        {
            var ip = new InteratorPattern();
            ip.InteratorDemo();
        }

        public static void CompositeDemo()
        {
            var cp = new CompositePattern();
            cp.CompositeDemo();
        }
    }
}
