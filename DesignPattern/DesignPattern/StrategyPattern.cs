using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 策略模式
    /// Define a family of algorithms,encapsulate each one,and make them interchangeable.
    /// 定义一组算法，并且将其封装起来，使它们之间可以互换。
    ///
    /// 主要有三个角色：
    /// - IStrategy
    ///   - 抽象策略角色。定义每个策略或算法必须具有的方法和属性。
    /// - ConcreteStrategy
    ///   - 具体策略角色。策略的具体实现算法。
    /// - Context
    ///   - 上下文角色。屏蔽高层模块对策略、算法的直接访问，封装可能的变化。
    /// </summary>
    class StrategyPattern
    {
        public void StrategyDemo()
        {
            var s = new ConcreteStrategy();
            var s2 = new ConcreteStrategy2();
            var context = new Context(s);
            context.DoSomething();
            context.Strategy = s2;
            context.DoSomething();
        }
    }

    #region 策略模式模板代码
    /// <summary>
    /// 抽象策略角色
    /// 定义每个策略或算法必须具有的方法和属性。
    /// </summary>
    interface IStrategy
    {
        public void DoSomething();
    }

    /// <summary>
    /// 具体策略角色
    /// </summary>
    class ConcreteStrategy : IStrategy
    {

        public void DoSomething()
        {
            Console.WriteLine("策略1的逻辑");
        }
    }

    /// <summary>
    /// 具体策略角色
    /// </summary>
    class ConcreteStrategy2 : IStrategy
    {

        public void DoSomething()
        {
            Console.WriteLine("策略2的逻辑");
        }
    }

    /// <summary>
    /// 上下文角色
    /// 封装作用，屏蔽高层模块对策略、算法的直接访问，封装可能的变化。
    /// </summary>
    class Context
    {
        public IStrategy Strategy { get; set; }

        public Context(IStrategy strategy)
        {
            Strategy = strategy;
        }

        public void DoSomething() => Strategy.DoSomething();
    }

    #endregion
}
