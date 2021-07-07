using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 观察者模式，也叫发布订阅模式（Publish-Subscribe）
    /// Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
    /// 定义一种对象间一对多的依赖，从而使得一个对象的状态改变时，其他依赖它的对象都会得到通知并自动更新。
    ///
    /// 由四个角色组成：
    /// - Subject
    ///   - 被观察者角色。抽象类。仅仅完成作为被观察者必须实现的职责：管理观察者并通知观察者。
    /// - ConcreteSubject
    ///   - 具体被观察者角色。实现自己的业务逻辑，并定义对哪些事件进行通知。
    /// - IObserver
    ///   - 抽象观察者角色。定义 Update 方法。
    /// - ConcreteObserver
    ///   - 具体观察者角色。实现 Update 方法。
    ///
    /// 优点：
    /// - 观察者和被观察者解耦。
    /// - 建立了一套触发机制。
    /// 缺点：
    /// - 顺序触发的话，一个观察者卡壳，其他都等待，影响整体效率。可以考虑异步解决。
    /// - 多级触发的时候，效率更糟糕。
    /// 
    /// </summary>
    class ObserverPattern
    {
        public void ObserverDemo()
        {
            var subject = new ConcreteSubject();
            var o = new ConcreteObserver();
            var o2 = new ConcreteObserver2();
            subject.Attach(o);
            subject.Attach(o2);
            subject.DoSomething();

        }
    }

    #region 观察者模式通用代码
    /// <summary>
    /// 抽象被观察者角色
    /// 必须能够动态地增加、取消观察者。
    /// 仅仅完成作为被观察者必须实现的职责：管理观察者并通知观察者。
    /// </summary>
    abstract class Subject
    {
        private readonly List<IObserver> _list = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _list.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _list.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _list)
            {
                observer.Update();
            }
        }
    }

    /// <summary>
    /// 具体被观察者角色
    /// 实现自己的业务逻辑，并定义对哪些事件进行通知。
    /// </summary>
    class ConcreteSubject : Subject
    {
        public void DoSomething()
        {
            Console.WriteLine("被观察对象状态改变！");
            Notify();
        }
    }

    /// <summary>
    /// 抽象观察者角色
    /// 定义 Update 方法。
    /// </summary>
    interface IObserver
    {
        /// <summary>
        /// Update 方法
        /// </summary>
        public void Update();

        /// <summary>
        /// 参数为被观察对象和 dto 的 Update 方法。
        /// 其实在日常应用上，这个更常用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Update(Subject sender, string args);
    }

    /// <summary>
    /// 具体观察者角色
    /// 具体实现 Update 逻辑。
    /// </summary>
    class ConcreteObserver : IObserver
    {
        public void Update()
        {
            Console.WriteLine("Observer 收到通知，并进行处理！");
        }

        public void Update(Subject sender, string args)
        {
            throw new NotImplementedException();
        }
    }
    class ConcreteObserver2 : IObserver
    {
        public void Update()
        {
            Console.WriteLine("Observer2 收到通知，并进行处理！");
        }

        public void Update(Subject sender, string args)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
