using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.FunctionalThinking
{
    class MethodChainingWithExtensionMethods
    {
        /// <summary>
        /// 声明一个扩展方法
        /// 1. 创建一个 static class
        /// 2. 创建一个 public 或者 internal 静态方法
        /// 3. 添加 this 关键字在第一个参数前面
        /// </summary>
        public static void ExtensionMethodTest()
        {
            var meaningOfLift = 42;
            Console.WriteLine($"is the meaning of lift even:{meaningOfLift.IsEven()}");
        }

        /// <summary>
        /// 扩展方法可以在 Null 实例之后使用，跟普通的实例方法不一样
        /// </summary>
        public static void WorkingWithNull()
        {
            string str = "";
            Console.WriteLine($"is str empty:{str.IsNullOrEmpty()}");
            str = null;
            Console.WriteLine($"is str empty:{str.IsNullOrEmpty()}");
        }

        /// <summary>
        /// 使用扩展方法实现链式编程
        /// 扩展方法应该尽量面向接口编程，这样可以达到更好的应用
        /// </summary>
        public static void FluentInterfacesAndMethodChaining()
        {
            var words = new List<string>();

            // 这个写法重复太多
            //words.Add("This");
            //words.Add("Feels");
            //words.Add("Weird");

            // 使用扩展方法实现链式编程
            words.AddItem("This")
                .AddItem("Feels")
                .AddItem("Weird");
            Console.WriteLine(string.Join(" ", words));
        }

    }

    /// <summary>
    /// 
    /// </summary>
    static class IntExtensions
    {
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }
    }

    static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }

    static class CollectionExtensions
    {
        /// <summary>
        /// 扩展方法应该进来面对接口实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ICollection<T> AddItem<T>(this ICollection<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }

}
