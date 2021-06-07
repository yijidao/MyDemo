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
    /// 其实我觉得直接饿汉就好了
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



}
