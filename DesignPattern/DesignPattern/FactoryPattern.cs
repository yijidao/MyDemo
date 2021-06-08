using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 工厂模式就具有良好的解耦性，当业务发生变化时，只需要修改具体的工厂类或者拓展工厂类就可以，
    /// 而且工厂模式不关心具体产品实现，所以只要产品抽象类定义得好，只需要新增具体产品类，其他代码不用修改，就能完成业务了。
    ///
    /// 工厂有许多实现，如通用工厂，结合静态方法的简单工厂，使用字典做缓存的缓存工厂，使用反射的单例工厂，使用 Lazy 的懒加载工厂等等。
    /// </summary>
    class FactoryPattern
    {
        public void HumanFactoryDemo()
        {
            AbstractHumanFactory factory = new HumanFactory();
            var human = factory.CreateHuman<WhiteHuman>();
            human.Talk();
            human = factory.CreateHuman<BlackHuman>();
            human.Talk();
            human = factory.CreateHuman<YellowHuman>();
            human.Talk();
        }

        /// <summary>
        /// 工厂模式的通用实现
        /// </summary>
        public void GeneralFactory()
        {
            Creator creator = new ConcreteCreator();
            Product product = creator.CreateProduct<ConcreteProduct>();
            product.Method();
            product.Method2();
            product = creator.CreateProduct<ConcreteProduct2>();
            product.Method();
            product.Method2();
        }

        /// <summary>
        /// 简单工厂模式
        /// 不符合开闭原则，不好扩展，但是在很多情况下很好用。
        /// </summary>
        public void SimpleFactory()
        {
            SimpleCreator.CreateProduct<ConcreteProduct>();
            SimpleCreator.CreateProduct<ConcreteProduct2>();
        }
    }

    interface IHuman
    {
        public string Color { get; set; }

        public void Talk();
    }

    class BlackHuman : IHuman
    {
        public string Color { get; set; } = "Black";
        public void Talk() => Console.WriteLine("我是黑种人");
    }

    class YellowHuman : IHuman
    {
        public string Color { get; set; } = "Yellow";
        public void Talk() => Console.WriteLine("我是黄种人");
    }

    class WhiteHuman : IHuman
    {
        public string Color { get; set; } = "White";
        public void Talk() => Console.WriteLine("我是白种人");
    }

    abstract class AbstractHumanFactory
    {
        /// <summary>
        /// 带属性初始化的工厂方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="init"></param>
        /// <returns></returns>
        public abstract IHuman CreateHuman<T>(Action<T> init = null) where T : class, IHuman;
    }

    class HumanFactory : AbstractHumanFactory
    {
        public override IHuman CreateHuman<T>(Action<T> init = null)
        {
            var instance = Activator.CreateInstance<T>();
            init?.Invoke(instance);
            return instance;
        }
    }

    abstract class Creator
    {
        public abstract T CreateProduct<T>() where T : Product;
    }

    /// <summary>
    /// 通用工厂实现
    /// 有抽象类，有实现类
    /// </summary>
    class ConcreteCreator : Creator
    {
        public override T CreateProduct<T>()
        {
            // 这里还需要异常处理
            return Activator.CreateInstance<T>();
        }
    }

    /// <summary>
    /// 简单工厂实现
    /// 只有静态实现类
    /// </summary>
    class SimpleCreator
    {

        public static T CreateProduct<T>() where T : Product
        {
            return Activator.CreateInstance<T>();
        }
    }

    abstract class Product
    {
        /// <summary>
        /// 公用业务方法
        /// </summary>
        public void Method()
        {
        }

        /// <summary>
        /// 抽象自定义方法
        /// </summary>
        public abstract void Method2();
    }

    class ConcreteProduct : Product
    {
        public override void Method2()
        {
        }
    }

    class ConcreteProduct2 : Product
    {
        public override void Method2()
        {
        }
    }



}
