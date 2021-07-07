using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 装饰者模式
    /// Attach additional responsibilities to an object dynamically keeping the same interface.
    /// Decorator provide a flexible alternative to subclass for extending functionality.
    /// 在接口不变的情况下，动态地给类添加功能。
    /// 装饰者模式提供了更加灵活的实现来替代通过子类继承来拓展功能，
    ///
    /// 装饰者模式的角色：
    /// - 抽象组件角色
    ///   - 在装饰者模式中，必然有一个最基本、最核心、最原始的接口或者抽象类充当抽象组件。
    /// - 具体组件角色
    ///   - 也是被装饰的对象。
    /// - 抽象装饰角色
    ///   - 其中必然有一个指针指向被装饰的对象。
    /// - 具体装饰角色
    ///   - 包含具体的装饰逻辑。
    ///
    /// 优点：
    /// - 装饰类和被装饰类可以独立发展，不会耦合。
    /// - 装饰模式是继承的一个替代方案。不管装饰多少层，都是 is-a 的关系。
    /// - 装饰器可以在调用时动态添加，动态调整顺序。
    /// 缺点：
    /// - 太多装饰器的话包装太多层很复杂。
    ///
    /// 使用场景：
    /// - 扩展一个类的功能。
    /// - 需要动态地给一个类增加或者移除功能。
    /// - 为一批兄弟类增加功能。
    /// </summary>
    class DecoratorPattern
    {

        public void DecoratorDemo()
        {
            Component component = new ConcreteComponent();
            component = new ConcreteDecorator(component); // 第一次装饰
            component = new ConcreteDecorator2(component); // 第二次装饰
            component.DoSomething();
        }

    }

    #region 装饰者模式模板代码
    /// <summary>
    /// 在装饰者模式中，必然有一个最基本、最核心、最原始的接口或者抽象类充当抽象组件。
    /// </summary>
    abstract class Component
    {
        public abstract void DoSomething();
    }

    /// <summary>
    /// 具体组件。也是被装饰的对象。
    /// </summary>
    class ConcreteComponent : Component
    {
        public override void DoSomething()
        {
            Console.WriteLine("Do something");
        }
    }
    
    /// <summary>
    /// 抽象装饰器。其中必然有一个指针指向被装饰的对象。
    /// </summary>
    abstract class Decorator : Component
    {
        public Component Component { get; }

        protected Decorator(Component component)
        {
            Component = component;
        }

        public override void DoSomething()
        {
            Component.DoSomething();
        }
    }

    /// <summary>
    /// 具体装饰器。
    /// </summary>
    class ConcreteDecorator : Decorator
    {
        public ConcreteDecorator(Component component) : base(component)
        {
        }

        public override void DoSomething()
        {
            Method1();
            base.DoSomething();
        }

        public void Method1()
        {
            Console.WriteLine("Method1 装饰...");
        }
    }

    /// <summary>
    /// 具体装饰器
    /// </summary>
    class ConcreteDecorator2 : Decorator
    {
        public ConcreteDecorator2(Component component) : base(component)
        {
        }

        public override void DoSomething()
        {
            Method2();
            base.DoSomething();
        }

        public void Method2()
        {
            Console.WriteLine("Method2 装饰...");
        }
    }
    #endregion
}
