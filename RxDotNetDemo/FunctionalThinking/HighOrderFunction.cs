using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.FunctionalThinking
{
    /// <summary>
    /// 实现高阶函数，参数或返回值是函数
    /// </summary>
    public class HighOrderFunction
    {
        /// <summary>
        /// 使用 delegate 关键字来实现委托
        /// 委托用来表示一个方法的引用，所以可以用来实现高阶函数
        /// 委托编译后其实会生成一个类
        /// </summary>
        public static void DelegateAsParameterType()
        {
            var s1 = "Hello";
            var s2 = "World";
            var comparators = new StringComparators();
            var test = new ComparisonTest(comparators.CompareContent); // 委托传实例方法参数
            Console.WriteLine($"CompareContent returned: {test(s1, s2)}");

            test = new ComparisonTest(StringComparators.CompareLength); // 委托传静态方法参数
            Console.WriteLine($"CompareLength returned: {test(s1, s2)}");

            test = comparators.CompareContent; // 简化版写法
            Console.WriteLine($"CompareContent returned: {test(s1, s2)}");

            var cities = new[] { "London", "Madrid" };
            var friends = new[] { "Minnie", "Goofey" };
            test = StringComparators.CompareLength;
            Console.WriteLine($"Are friends and cities similar? {AreSimilar(cities, friends, test)}"); // 使用委托实现高阶函数
        }

        public static bool AreSimilar(string[] leftItems, string[] rightItems, ComparisonTest tester)
        {
            if (leftItems.Length != rightItems.Length) return false;

            for (int i = 0; i < leftItems.Length; i++)
            {
                if (tester(leftItems[i], rightItems[i]) == false) return false;
            }

            return true;
        }

        public delegate bool ComparisonTest(string first, string second); // 使用 delegate 定义委托

        /// <summary>
        /// 直接使用 delegate 关键字来实现委托有一点不好，就是得写一个有命名的方法，有点割裂感
        /// 所以这里使用匿名方法来优化这个步骤
        /// </summary>
        public static void AnonymousMethods()
        {
            ComparisonTest tester = delegate (string first, string second)
            {
                return first.Length == second.Length;
            };
            Console.WriteLine($"anonymous methods returned: {tester("Hello", "World")}");

            var cities = new[] { "London", "Madrid" };
            var friends = new[] { "Minnie", "Goofey" };
            Console.WriteLine($"Are friends and cities similar? {AreSimilar(cities, friends, tester)}"); // 使用匿名方法实现的委托
        }

        /// <summary>
        /// 使用匿名方法需要注意闭包，闭包有个特点，就是闭包捕获的变量的值，是在执行时确定的，而不是声明时确定的
        /// </summary>
        public static void Closures()
        {
            var actions = new List<ActionDelegate>();
            for (int i = 0; i < 5; i++)
            {
                actions.Add(delegate ()
                {
                    Console.WriteLine(i); // 捕获了变量 i
                });
            }


            foreach (var act in actions)
            {
                act(); // 这里指挥打印 5，这就是闭包捕获变量的例子
            }
        }

        public delegate void ActionDelegate();

        /// <summary>
        /// C# 3.0 增加了 lambda，所以可以使用 lambda 表达式优化匿名函数
        /// </summary>
        public static void LambdaExpressions()
        {
            ComparisonTest tester = (first, second) => first.Length == second.Length;
            Console.WriteLine($"anonymous methods returned: {tester("Hello", "World")}");

            var cities = new[] { "London", "Madrid" };
            var friends = new[] { "Minnie", "Goofey" };
            Console.WriteLine($"Are friends and cities similar? {AreSimilar(cities, friends, tester)}"); // 使用匿名方法实现的委托
        }

        /// <summary>
        /// lambda 只是简化的匿名函数，所以当然也有闭包的特性
        /// </summary>
        public static void LambdaExpressionsClosures()
        {
            var actions = new List<ActionDelegate>();
            for (int i = 0; i < 5; i++)
            {
                actions.Add(() => Console.WriteLine(i)); // 捕获了变量 i
            }

            foreach (var act in actions)
            {
                act(); // 这里指挥打印 5，这就是闭包捕获变量的例子
            }
        }

        /// <summary>
        /// 使用委托需要使用关键字 delegate 声明，贼烦
        /// 所以 .net 提供了 Func 和 Action 进一步封装委托，方便使用。芜湖，舒服多了。
        /// </summary>
        public static void FuncAndAction()
        {
            Func<string, string, bool> tester = (s1, s2) => s1.Length == s2.Length; // 使用 Func 来减少 delegate 的声明，进一步减少割裂感
            Console.WriteLine($"anonymous methods returned: {tester("Hello", "World")}");

            var cities = new[] { "London", "Madrid" };
            var friends = new[] { "Minnie", "Goofey" };
            Console.WriteLine($"Are friends and cities similar? {AreSimilarFunc(cities, friends, tester)}"); 
        }

        public static bool AreSimilarFunc(string[] leftItems, string[] rightItems, Func<string, string, bool> tester)
        {
            if (leftItems.Length != rightItems.Length) return false;

            for (int i = 0; i < leftItems.Length; i++)
            {
                if (tester(leftItems[i], rightItems[i]) == false) return false;
            }

            return true;
        }
    }

    class StringComparators
    {
        public static bool CompareLength(string first, string second)
        {
            return first.Length == second.Length;
        }

        public bool CompareContent(string first, string second)
        {
            return first == second;
        }
    }
}
