using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 单例饿汉模式
    /// </summary>
    class SingletonPattern
    {
        private static readonly SingletonPattern Singleton = new SingletonPattern();

        private SingletonPattern()
        {

        }

        public static SingletonPattern GetSingleton() => Singleton;

        /// <summary>
        /// 单例模式的其他方法尽量都是静态的
        /// </summary>
        public static void DoSomething() { }
    }

    /// <summary>
    /// 单例饱汉模式
    /// 其实我觉得直接饿汉就好了，只有这个实例不常用，或者初始化非常耗资源耗时（如从数据库中取数），才考虑这种方式。
    /// </summary>
    class SingletonPattern2
    {
        private static SingletonPattern2 Singleton = null;

        private static readonly object Lock = new object();

        private SingletonPattern2()
        {
        }

        public static SingletonPattern2 GetSingleton()
        {
            if (Singleton == null)
            {
                lock (Lock)
                {
                    Singleton ??= new SingletonPattern2();
                }
            }

            return Singleton;
        }

        /// <summary>
        /// 单例模式的其他方法尽量都是静态的
        /// </summary>
        public static void DoSomething() { }
    }

    /// <summary>
    /// Lazy 关键字的单例
    /// Lazy 和 ThreadLocal 可以配合了解。
    /// Lazy 经常用来实现懒加载属性， ThreadLocal 用来实现线程私有变量
    /// </summary>
    class SingletonPattern3
    {
        private static readonly Lazy<SingletonPattern3> Singleton = new Lazy<SingletonPattern3>(LazyThreadSafetyMode.ExecutionAndPublication);

        private SingletonPattern3()
        {
        }

        public static SingletonPattern3 GetSingleton()
        {
            return Singleton.Value;
        }
    }

}
