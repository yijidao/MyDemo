using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        /// <summary>
        /// 委托一开始主要用于事件，但是自从引入了 Func 和 Action，委托就变得强大起来了，可以优化许多设计模式，让其变得更短，更可读
        /// 策略模式常用于工作流中，保留某个环节给使用者进行自定义，ICompare 和 Sort 就是标准的策略模式使用。
        /// 这个 Demo 使用 Func 优化策略者模式
        /// </summary>
        public static void UsingFuncAsStrategy()
        {
            var words = new List<string> { "ab", "a", "aabb", "abc" };
            words.Sort(new LengthComparer()); // LengthComparer 可以满足工作，但是痛苦的是，每次都要新建一个类去实现 IComparer
            Console.WriteLine(string.Join(",", words));

            // 使用 Func 实现泛型比较器
            words.Sort(new GenericComparer<string>((s, s2) => s.Length == s2.Length ? 0 : s.Length > s2.Length ? -1 : 1));
            Console.WriteLine(string.Join(",", words));
        }

        /// <summary>
        /// 懒加载模式，或者说工厂模式、单例模式等等，是非常常用的设计模式，但是经常需要写大量的 if 代码，或者考虑并发问题，或者根据不同类重复大量代码
        /// 这个demo 使用 Lazy 和 Func 来优化这些设计模式
        /// </summary>
        public static void UsingFuncAsFactory()
        {
            var thinClass = new ThinClass();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            thinClass.SomeMethod();

            var thinClassUsingLazy = new ThinClassUsingLazy();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            thinClassUsingLazy.SomeMethod();

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

    /// <summary>
    /// 常规实现
    /// LengthComparer 可以满足工作，但是痛苦的是，每次都要新建一个类去实现 IComparer
    /// </summary>
    class LengthComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length == y.Length) return 0;

            return x.Length > y.Length ? 1 : -1;
        }
    }

    /// <summary>
    /// 使用 Func 实现泛型比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class GenericComparer<T> : IComparer<T>
    {
        public Func<T, T, int> CompareFunc { get; }

        public GenericComparer(Func<T, T, int> compareFunc)
        {
            CompareFunc = compareFunc;
        }

        public int Compare(T x, T y)
        {
            return CompareFunc(x, y);
        }
    }

    class HeavyClass
    {
        public HeavyClass(string from)
        {
            Console.WriteLine($"{from} create HeavyClass");
        }

        public void Hello() => Console.WriteLine("Hello!");
    }

    /// <summary>
    /// 一个使用 if 来创建懒加载模式的例子
    /// 这种实现有几个隐藏的问题，
    /// 一是如果要创建十个懒加载，那么要十个 if 块代码，很痛苦
    /// 二是在并发情况下，需要手动加锁，更痛苦
    /// </summary>
    class ThinClass
    {
        private HeavyClass _heavy;

        public HeavyClass Heavy
        {
            get
            {
                if (_heavy == null)
                {
                    _heavy = new HeavyClass("ThinClass");
                }
                return _heavy;
            }
        }

        public ThinClass()
        {
            Console.WriteLine("ThinClass created");
        }

        public void SomeMethod()
        {
            Heavy.Hello();
        }
    }

    /// <summary>
    /// 使用 Lazy 和 Func 来实现懒加载模式，Lazy 支持线程安全
    /// </summary>
    class ThinClassUsingLazy
    {
        readonly Lazy<HeavyClass> _lazyHeavyClass = new Lazy<HeavyClass>(() =>
        {
            return  new HeavyClass("ThinClassUsingLazy");
        });

        public ThinClassUsingLazy()
        {
            Console.WriteLine("ThinClassUsingLazy created");
        }

        public void SomeMethod()
        {
            _lazyHeavyClass.Value.Hello();
        }
    }
}
