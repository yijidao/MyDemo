using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 适配器模式
    /// Convert a interface of a class into another interface clients expect.
    /// Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
    /// 将一个类的接口变换成客户端期待的另一种接口。
    /// 通过适配器，可以让接口不匹配的两个类一起工作。
    ///
    /// 主要有三个角色
    /// - Target
    ///   目标角色。该角色定义应该将其他类转换为何种接口。
    /// - Source
    ///   源角色。想把谁转换成目标角色，这个谁就是源角色。一般是一个已经存在且运行良好的类。
    /// - Adapter
    ///   适配器角色。通过继承或者类关联的方式，将源角色转换成目标角色。
    /// </summary>
    class AdapterPattern
    {
        public void AdapterDemo()
        {
            ITarget d = new Adapter();
            ITarget d2 = new Adapter2(new Source());
            d.TargerDoSomething();
            d2.TargerDoSomething();
        }
    }

    #region 适配器模式模板代码
    /// <summary>
    /// 抽象目标角色
    /// </summary>
    interface ITarget
    {
        /// <summary>
        /// 目标角色原有方法。
        /// </summary>
        public void TargerDoSomething();
    }

    /// <summary>
    /// 抽象源角色。
    /// </summary>
    interface ISource
    {
        public void SourceDoSomething();
    }

    /// <summary>
    /// 具体源角色
    /// </summary>
    class Source : ISource
    {
        public void SourceDoSomething()
        {
            Console.WriteLine("Source do something...");
        }
    }

    /// <summary>
    /// 适配器角色
    /// 这里通过继承实现适配
    /// </summary>
    class Adapter : Source, ITarget
    {
        public void TargerDoSomething()
        {
            SourceDoSomething();
        }
    }

    /// <summary>
    /// 适配器角色
    /// 这里通过关联实现适配
    /// </summary>
    class Adapter2 : ITarget
    {
        public ISource Source { get; }

        public Adapter2(ISource source)
        {
            Source = source;
        }
        public void TargerDoSomething()
        {
            Source.SourceDoSomething();
        }
    }

    #endregion
}
