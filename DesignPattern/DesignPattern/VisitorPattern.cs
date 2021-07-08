using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 访问者模式
    /// Represent an operation to be performed on the elements of an object structure.
    /// Visitor lets you define a new operation without changing the classed of the elements on which it operates.
    /// 封装一些作用于某种数据结构中各元素的操作。
    /// 访问者模式可以在不改变数据结构的前提下，定义作用于这些元素的新的操作。
    ///
    /// 有五个角色组成：
    /// - Element
    /// 抽象元素角色。声明接受哪一类访问者访问。其实就是 Accept 方法的参数。
    /// - ConcreteElement
    /// 具体元素角色。实现 Accept 方法，通常就是 visitor.Visit(this)。
    /// - IVisitor
    /// 抽象访问者角色。声明访问者可以访问哪些元素。其实就是 Visit 的参数。
    /// - Visitor
    /// 具体访问者角色。
    /// - ObjectStruture
    /// 结构对象角色。用于产生元素，在项目中一般就是一个普通的集合，很少抽象出这个角色。
    ///
    /// 优点
    /// - 符合单一职责原则。Element 负责数据的加载，Visitor 负责数据的展示。
    /// - 易扩展。如果需要对 Element 进行不同的展示，只需要往 Visitor 中加方法就好了。
    /// - 灵活性高。如果需要对 Element 中的数据进行不同倍数的计算，不需要改变 Element，只需要在 Visitor 中进行计算就好了。
    /// 缺点
    /// - 具体元素角色对具体访问者角色暴露了细节。不符合最小知识原则。关注了其他类内部的实现细节。
    /// - 如果具体角色增加、删除、修改了字段或者实现，Visitor就必须变化，如果 Visitor 非常多的话，会很难搞。
    /// - 违反了依赖倒置原则。直接依赖实现类，扩展困难。
    ///
    /// 使用场景：
    /// - 需要遍历不同具体元素角色并且做不同的操作，那么可以用访问者模式。因为迭代器模式不满足，迭代器模式一般是用于遍历抽象元素角色同时做相同的操作。
    /// - 需要对具体元素角色做很多且不相关的操作，使用访问者模式可以防止污染元素角色的代码。
    /// - 充当拦截器角色（Interceptor）。
    /// 
    /// </summary>
    class VisitorPattern
    {
        public void VisitorDemo()
        {
            for (int i = 0; i < 10; i++)
            {
                var e = ObjectStruture.CreateElement();
                Thread.Sleep(50);
                e.Accept(new Visitor());
            }
        }

        /// <summary>
        /// 这个 Demo 展示了在需要对不同元素的Count 进行倍数放大并汇总的业务下，不需要修改元素类，直接在 visitor 中添加方法就完成了这个操作。
        /// 这个 Demo 良好的展示了访问者模式对迭代器的补充，以及面对业务变化的灵活性。
        /// </summary>
        public void VisitorDemo2()
        {
            var visitor = new Visitor();
            for (int i = 0; i < 5; i++)
            {
                var e = ObjectStruture.CreateElement();
                Thread.Sleep(50);
                e.Accept(visitor);
            }

            Console.WriteLine($"Total count: {visitor.GetTotalCount()}");
        }
    }

    #region 访问者模式模板代码
    /// <summary>
    /// 抽象元素角色
    /// 声明接受哪一类访问者访问。其实就是 Accept 方法的参数。
    /// </summary>
    abstract class Element
    {
        public int Count { get; set; }

        public abstract void DoSomething();

        public abstract void Accept(IVisitor visitor);
    }
    /// <summary>
    /// 具体元素角色
    /// 实现 Accept 方法，通常就是 visitor.Visit(this)
    /// </summary>
    class ConcreteElement : Element
    {
        public ConcreteElement()
        {
            Count = 10;
        }

        public override void DoSomething()
        {
            Console.WriteLine($"Element: {Count}");
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class ConcreteElement2 : Element
    {
        public ConcreteElement2()
        {
            Count = 20;
        }

        public override void DoSomething()
        {
            Console.WriteLine($"Element2: {Count}");
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// 抽象访问者角色
    /// 声明访问者可以访问哪些元素。其实就是 Visit 的参数。
    /// </summary>
    interface IVisitor
    {
        public void Visit(ConcreteElement element);
        public void Visit(ConcreteElement2 element);

        public int GetTotalCount();
    }

    /// <summary>
    /// 具体访问者角色
    /// </summary>
    class Visitor : IVisitor
    {
        private int _totalCount;

        public void Visit(ConcreteElement element)
        {
            element.DoSomething();
            _totalCount += element.Count;
        }

        public void Visit(ConcreteElement2 element)
        {
            element.DoSomething();
            _totalCount += element.Count * 2;
        }

        public int GetTotalCount()
        {
            return _totalCount;
        }
    }
    /// <summary>
    /// 结构对象角色
    /// 用于产生元素，在项目中一般就是一个普通的集合，很少抽象出这个角色
    /// </summary>
    class ObjectStruture
    {
        public static Element CreateElement()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            if (rand.Next(100) > 50)
            {
                return new ConcreteElement();
            }
            else
            {
                return new ConcreteElement2();
            }
        }
    }

    #endregion
}
