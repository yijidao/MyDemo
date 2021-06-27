using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 建造者模式，也叫做生成器模式。
    /// Separate the construction of a complex object from its representation so that the same construction precess can create different representations.
    /// 讲一个复杂对象的构建和表示分离，从而使同样的构建过程有不同的表示。
    ///
    /// 建造者模式包含建造者类，产品类（实现模板模式），导演类。
    /// 建造者模式跟工厂模式同属创建类模式，但是注重点不同，建造者模式注重通过不同步骤生成不同的产品。工厂模式注重生成不同的产品。
    /// </summary>
    class BuilderPattern
    {
        public void BuilderPatterDemo()
        {
            var director = new Director();
            director.GetClass1();
        }
    }

    /// <summary>
    /// 抽象建造者
    /// </summary>
    abstract class Builder
    {
        public abstract void SetPart();

        public abstract AbstractClass BuildProduct();
    }

    /// <summary>
    /// 具体建造者。建造者一般会包含一个实现了模板模式的产品类。
    /// 多个产品类的话，一般需要多个建造类。
    /// </summary>
    class ConcreteBuilder : Builder
    {
        private AbstractClass Product { get; } = new ConcreteClass1();
        public override void SetPart()
        {
            Product.Step3();
            Product.Step2();
            Product.Step1();

        }

        public override AbstractClass BuildProduct() => Product;
    }

    class ConcreteBuilder2 : Builder
    {
        private AbstractClass Product { get; } = new ConcreteClass1();
        /// <summary>
        /// 跟ConcreteBuilder的SetPart 不同的步骤。
        /// </summary>
        public override void SetPart()
        {
            Product.Step1();
            Product.Step2();
            Product.Step3();
        }

        public override AbstractClass BuildProduct() => Product;
    }

    /// <summary>
    /// 导演类。导演类主要起到封装的作用，避免高层模块深入到建造者内部的实现类。
    /// </summary>
    class Director
    {
        private Builder Builder { get; } = new ConcreteBuilder();
        private Builder Builder2 { get; } = new ConcreteBuilder2();

        public AbstractClass GetClass1() => Builder.BuildProduct();

        public AbstractClass GetClass2() => Builder2.BuildProduct();

    }
}
