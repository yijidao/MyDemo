using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 门面模式，也叫做外观模式。
    /// Provide a unified interface to a set of interfaces in a subsystem.
    /// Facade defines a higher-level interface thar makes the subsystem easier to use.
    /// 提供一个统一的对象，用于访问子系统中的接口。
    /// 门面模式定义了一个高层次的接口，从而使子系统更易于使用。
    ///
    /// 目的是提供一个粗粒度的接口。
    /// 
    /// 门面系统注重统一的对象，也就是提供一个访问子系统接口的高层次接口，除了这个接口，不允许其他任何访问子系统的行为发生。
    /// 门面系统是外界访问子系统内部的唯一通道。
    ///
    /// 由两个角色组成：
    /// - Facade
    ///   - 门面角色。客户端调用门面角色的方法。此角色知道子系统的所有功能，会将从客户端发来的请求委派到相应的子系统里去。
    ///   - 这只是一个委托类，只是委派请求，不会有实际业务。
    /// - Subsystem
    ///   - 子系统角色。可以有一个或者多个子系统，每一个子系统角色都是不是单独的类，而是类的集合。子系统角色并不知道门面角色的存在，对子系统角色来说，门面角色只是一个客户端而已。
    ///
    /// 优点：
    /// - 减少外部系统和子系统的依赖。
    /// - 提高安全性。外部系统能访问子系统那些逻辑都是门面系统说算了。
    ///
    /// 缺点：
    /// - 不符合开闭原则。如果新增了子系统，就要修改门面类。
    ///
    /// 使用场景：
    /// - 为业务复杂的模块或者子系统提供统一的供外界使用的接口。
    /// 
    /// </summary>
    class FacadePattern
    {
    }

    #region 门面模式模板代码
    /// <summary>
    /// 门面角色
    /// 只做转发，没有业务逻辑
    /// </summary>
    class Facade
    {
        private ClassA _a = new ClassA();
        private ClassB _b = new ClassB();
        private ClassC _c = new ClassC();
        private ClassD _d = new ClassD();

        /// <summary>
        /// 只做转发
        /// </summary>
        public void MethodA()
        {
            _a.DoSomethingA();
        }

        /// <summary>
        /// 只做转发
        /// </summary>
        public void MethodB()
        {
            _b.DoSomethingB();
        }

        /// <summary>
        /// 只做转发
        /// </summary>
        public void MethodC()
        {
            _c.DoSomethingC();
        }

        /// <summary>
        /// 在门面角色中，写这种方法是错误的，因为完成这个逻辑需要顺序调用方法，等于门面类也实现了业务逻辑，从而将子系统倒依赖回门面角色了，并且违反了单一职责原则。
        /// </summary>
        public void IncorrectMethod()
        {
            _a.DoSomethingA();
            _c.DoSomethingC();
        }

        /// <summary>
        /// 通过增加封装类来再次封装子系统逻辑，这样门面类就不会有业务逻辑了。
        /// </summary>
        public void CorrectMethod()
        {
            _d.DoSomethingD();
        }
    }

    /// <summary>
    /// 某个子系统中的一部分逻辑。
    /// </summary>
    class ClassA
    {
        public void DoSomethingA()
        {

        }
    }

    /// <summary>
    /// 某个子系统中的一部分逻辑。
    /// </summary>
    class ClassB
    {
        public void DoSomethingB()
        {

        }
    }

    /// <summary>
    /// 某个子系统中的一部分逻辑。
    /// </summary>
    class ClassC
    {
        public void DoSomethingC()
        {

        }
    }

    /// <summary>
    /// 用封装类来再次封装子系统中的逻辑
    /// </summary>
    class ClassD
    {
        private ClassA _a = new ClassA();
        private ClassC _c = new ClassC();

        public void DoSomethingD()
        {
            _a.DoSomethingA();
            _c.DoSomethingC();
        }
    }

    #endregion
}
