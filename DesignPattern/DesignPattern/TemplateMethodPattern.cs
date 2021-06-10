using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 模板方法模式
    /// Define the skeleton of an algorithm in an operation,deferring some steps to subclass. Template Method lets subclass redefine certain steps of
    /// an algorithm without changing the algorithm's structure.
    /// 定义一个操作中的算法的框架，然后想一些步骤的实现延迟到子类中。
    /// 使得子类不需要改变算法的结构，就能重新定义该算法的特定步骤。
    ///
    /// 模板方法由模板类和具体类组成，
    /// 模板类定义算法步骤和模板方法。
    /// 具体类实现具体算法
    /// </summary>
    class TemplateMethodPattern
    {
        public void TemplateMethodDemo()
        {
            var concreteClass1 = new ConcreteClass1();
            concreteClass1.TemplateMethod();

            var concreteClass2 = new ConcreteClass2();
            concreteClass2.TemplateMethod();
        }
    }

    /// <summary>
    /// 模板类
    /// </summary>
    abstract class AbstractClass
    {
        /// <summary>
        /// 算法步骤
        /// </summary>
        protected abstract void Step1();

        protected abstract void Step2();

        protected abstract void Step3();

        /// <summary>
        /// 模板方法
        /// </summary>
        public void TemplateMethod()
        {
            Step1();
            Step2();
            Step3();
        }
    }

    /// <summary>
    /// 具体类
    /// </summary>
    class ConcreteClass1 : AbstractClass
    {
        /// <summary>
        /// 算法实现
        /// </summary>
        protected override void Step1()
        {
        }

        protected override void Step2()
        {
        }

        protected override void Step3()
        {
        }
    }

    /// <summary>
    /// 具体类
    /// </summary>
    class ConcreteClass2 : AbstractClass
    {
        /// <summary>
        /// 算法实现
        /// </summary>
        protected override void Step1()
        {
        }

        protected override void Step2()
        {
        }

        protected override void Step3()
        {
        }
    }
}
