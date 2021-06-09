using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 抽象工厂
    /// Provide an interface for creating families of related or dependent objects without specifying their concrete classed.
    /// 为一组相关或依赖的对象提供一个接口，而不需要执行他们的具体类。
    ///
    /// 抽象工厂是工厂模式的扩展。
    /// 优点就是抽象，所以封装性很高，用户只需要调用工厂类，就能生成一系列的对象。
    /// 缺点就是如果需要增加产品的话（也就是增加契约），底层模块需要直接修改，并且影响到顶层模块，所以纵向扩展比较困。
    ///
    /// 使用场景：
    /// 如果有一个对象族有相同的约束，那么用抽象工厂就很方便。
    /// </summary>
    class AbstractFactoryPattern
    {
        public void AbstractFactoryDemo()
        {
            var creator1 = new Creator1();
            var creator2 = new Creator2();

            creator1.CreateProductA(); // 生成 A1
            creator1.CreateProductB(); // 生成 B1

            creator2.CreateProductA(); // 生成 A2
            creator2.CreateProductB(); // 生成 B2
        }
    }

    abstract class AbstractProductA
    {
        public void ShareMethod() { }
        public abstract void DoSome();
    }

    class ProductA1 : AbstractProductA
    {
        public override void DoSome()
        {
            Console.WriteLine("A1 实现方法");
        }
    }

    class ProductA2 : AbstractProductA
    {
        public override void DoSome()
        {
            Console.WriteLine("A2 实现方法");
        }
    }

    abstract class AbstractProductB
    {
        public void ShareMethod() { }
        public abstract void DoSome();
    }

    class ProductB1 : AbstractProductB
    {
        public override void DoSome()
        {
            Console.WriteLine("B1 实现方法");
        }
    }

    class ProductB2 : AbstractProductB
    {
        public override void DoSome()
        {
            Console.WriteLine("B2 实现方法");
        }
    }

    abstract class AbstractCreator
    {
        public abstract AbstractProductA CreateProductA();
        public abstract AbstractProductB CreateProductB();
    }

    class Creator1 : AbstractCreator
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA1();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB1();
        }
    }

    class Creator2 : AbstractCreator
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA2();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB2();
        }
    }
}
