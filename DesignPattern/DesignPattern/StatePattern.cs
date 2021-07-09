using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 状态模式
    /// Allow an object to alter its behavior when its internal state changes.
    /// The object will appear to change its class.
    /// 当一个对象内部状态改变时，允许去改变其行为。
    /// 这个对象将会看起来像是改变了类型一样。（其实就是在状态改变时，调用不同的子类方法，所以看起来像是类被改变了一样）
    ///
    /// 状态模式的核心就是封装，通过状态的变更引起行为的变更，所以外部看就好像这个对象对应的类发生了改变一样。
    ///
    /// 状态模式有两个约定：
    /// - 把状态对象声明为静态常量，有几个状态对象就声明几个静态常量。
    /// - 上下文角色具有状态抽象角色定义的所有行为，具体执行使用委托方式。
    ///
    /// 优点：
    /// - 结构清晰。避免了过多的 switch...case 或者 if...else 等分支语句的而是用，避免了程序的复杂性，让代码可读，所以好维护。
    /// - 遵循设计原则。遵循了开闭原则和单一职责。每个状态都是一个子类，增加状态就增加子类，修改状态就修改子类。
    /// - 封装很好。
    /// 缺点：
    /// - 状态太多，类膨胀。可以在数据库中建个状态表，根据相应状态执行相应操作。
    ///
    /// 使用场景：
    /// - 行为随状态改变而改变的场景。例如权限设计，人员的状态不同，执行相同的行为，结果也会不同，就适合状态模式。
    /// - 条件、分支语句太多的场景，可以用状态模式替换。
    ///
    /// 状态模式+建造者模式会有很不错的效果
    /// </summary>
    class StatePattern
    {

        public void StatePatternDemo()
        {
            // 定义上下文角色
            var context = new StateContext(); 
            // 设置初始状态
            context.CurrentState = StateContext.STATE1;

            // 执行处理方法
            context.Handle2();
            context.Handle1();
        }

    }

    #region 状态模式模板代码
    /// <summary>
    /// 抽象状态角色
    /// 负责对象状态定义，封装上下文角色以实现状态切换。
    /// </summary>
    abstract class State
    {
        public StateContext Context { get; set; }

        /// <summary>
        /// 行为1
        /// </summary>
        public abstract void Handle1();

        /// <summary>
        /// 行为2
        /// </summary>
        public abstract void Handle2();
    }
    /// <summary>
    /// 具体状态角色
    /// 每个具体状态必须完成两个职责：本状态的行为管理以及趋向状态处理。
    /// 也就是本状态要处理的逻辑，以及本状态如何过滤到其他状态。
    /// </summary>
    class ConcreteState : State
    {
        /// <summary>
        /// 本状态下必须执行的处理
        /// </summary>
        public override void Handle1()
        {
            Console.WriteLine("Handle1 执行业务逻辑...");
        }

        /// <summary>
        /// 过渡到其他状态
        /// 切换状态，并且调用该状态的处理逻辑
        /// </summary>
        public override void Handle2()
        {
            Context.CurrentState = StateContext.STATE2;
            Context.CurrentState.Handle2();
        }
    }

    class ConcreteState2 : State
    {
        /// <summary>
        /// 本状态下必须执行的处理
        /// </summary>
        public override void Handle1()
        {
            Context.CurrentState = StateContext.STATE1;
            Context.CurrentState.Handle1();
        }
        /// <summary>
        /// 过渡到其他状态
        /// 切换状态，并且调用该状态的处理逻辑
        /// </summary>
        public override void Handle2()
        {
            Console.WriteLine("Handle2 执行业务逻辑...");
        }
    }
    /// <summary>
    /// 上下文角色
    /// 定义客户端需要的接口，并且负责具体状态的切换。
    /// </summary>
    class StateContext
    {
        public static readonly State STATE1 = new ConcreteState();
        public static readonly State STATE2 = new ConcreteState2();
        private State _currentState;

        public State CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value; 
                _currentState.Context = this; // 切换状态
            }
        }
        /// <summary>
        /// 行为委托
        /// </summary>
        public void Handle1()
        {
            CurrentState.Handle1();
        }
        /// <summary>
        /// 行为委托
        /// </summary>
        public void Handle2()
        {
            CurrentState.Handle2();
        }
    }

    #endregion

}
