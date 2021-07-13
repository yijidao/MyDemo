using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 桥梁模式
    /// Decouple an abstraction from its implementation so that the two can vary independently.
    /// 将抽象和实现解耦，从而使得抽象和实现可以独立变化。
    ///
    /// 桥梁模式的重点是解耦，将抽象和实现接口，在普通实现中，抽象和实现是继承关系，是耦合的。
    ///
    /// 四个角色组成：
    /// - 实现化角色 IImplementor
    ///   - 接口或者抽象类，定义角色必须的行为和属性。
    /// - 具体实现化角色 ConcreteImplementor
    /// - 抽象化角色 Abstraction
    ///   - 主要职责是定义出该角色的行为，同时保存一个对实现化角色的引用。
    /// - 重定义角色 RefinedAbstraction
    ///   - 修改抽象化角色中引用到实现化角色的行为。
    /// - IImplementor 和 ConcreteImplementor 就像两个桥墩，链接两个桥墩的，就是桥。IImplementor 代表抽象，ConcreteImplementor 代表不同的实现。
    ///
    /// 优点：
    /// - 抽象和实现解耦。这也是桥梁模式的主要特点，为了解决继承的缺点而提出的设计模式。实现不受抽象的约束。
    /// - 优秀的扩展能力。符合开闭原则、依赖倒置原则、最小知识原则。
    ///
    /// 使用场景
    /// - 不希望或者不适用继承的场景。
    ///   - 如继承层次过多、无法更细化设计颗粒等场景，可以考虑桥梁模式。
    /// - 接口或者抽象类不稳定的场景。
    ///   - 接口或者抽象类不稳定，就不能使用继承来实现业务需求，后期不敢随便修改，得不偿失，可以考虑桥梁模式。
    /// - 重用性要求比较高的场景。
    ///   - 设计的粒度越细，重用的可能性就越大，这个时候如果采用继承，就要受父类的限制，注定不可能出现太细的颗粒度。
    ///
    /// 注意事项：
    /// 桥梁模式很简单，主要考虑的是如何拆分抽象和实现，封装变化，将可能变化的因素封装到最小的逻辑的单元中，防止风险扩散。
    /// 所以如果发现类的继承有N层时，可以考虑桥梁模式。
    /// 
    /// </summary>
    class BridgePattern
    {
        /// <summary>
        /// 桥梁模式场景代码
        /// </summary>
        public void BridgeDemo()
        {
            IImplementor imp = new ConcreteImplementor();
            Abstraction abs = new RefinedAbstraction(imp);
            abs.Request();
        }

    }

    #region 桥梁模式模板代码
    /// <summary>
    /// 实现化角色
    /// 接口或者抽象类，定义角色必须的行为和属性。
    /// </summary>
    interface IImplementor
    {
        public void DoSomething();

        public void DoAnything();
    }
    /// <summary>
    /// 具体实现化角色
    /// </summary>
    class ConcreteImplementor : IImplementor
    {
        public void DoSomething()
        {
        }

        public void DoAnything()
        {
        }
    }

    /// <summary>
    /// 抽象化角色
    /// 主要职责是定义出该角色的行为，同时保存一个对实现化角色的引用。
    /// </summary>
    abstract class Abstraction
    {
        public IImplementor Implementor { get; }

        protected Abstraction(IImplementor implementor)
        {
            Implementor = implementor;
        }

        public virtual void Request()
        {
            Implementor.DoSomething();
        }
    }
    /// <summary>
    /// 重定义角色
    /// 修改抽象化角色中引用到实现化角色的行为。
    /// </summary>
    class RefinedAbstraction : Abstraction
    {

        public RefinedAbstraction(IImplementor implementor) : base(implementor)
        {
        }

        public override void Request()
        {
            base.Request();
            Implementor.DoAnything();
        }
    }

    #endregion
}
