using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 命令模式
    /// Encapsulate a request as an object, there by letting you parameterize clients with different request,queues or log requests, and support undo able operations.
    /// 将请求封装成对象，因此可以使用不用的请求参数化客户端，排队执行请求或者记录请求日志，支持撤销操作。
    ///
    /// 命令模式由三个角色组成：
    /// - Receiver 
    ///   接收者角色。真正执行命令的对象。
    /// - Command
    ///   命令者角色。声明命令，命令可以依赖一个或多个 Receiver。
    /// - Invoker
    ///   调用者角色。接收命令，并执行命令。
    ///
    /// 优点：
    /// - 解耦。
    /// - 可扩展。
    /// - 结合其他模式效果相当不错。结合责任链模式，实现命令族解析任务。结合模板方法，可以减少 Command 子类的膨胀问题。
    /// 缺点：
    /// - 有多个命令就有多个 Command 的子类，会类膨胀。
    /// 
    /// </summary>
    class CommandPattern
    {

        public void CommandPatternDemo()
        {
            var invoker = new Invoker();
            var receiver = new ConcreteReceiver();
            var command = new ConcreteCommand(receiver);
            invoker.Command = command;
            invoker.Action();
        }

    }

    #region 命令模式通用代码
    abstract class Command
    {
        public abstract void Execute();
    }

    class ConcreteCommand : Command
    {
        public Receiver Receiver { get; }

        public ConcreteCommand(Receiver receiver)
        {
            Receiver = receiver;
        }

        public override void Execute()
        {
            Receiver.DoSomething();
        }
    }

    class ConcreteCommand2 : Command
    {
        public Receiver Receiver { get; }

        public ConcreteCommand2(Receiver receiver)
        {
            Receiver = receiver;
        }

        public override void Execute()
        {
            Receiver.DoSomething();
        }
    }

    class Invoker
    {
        public Command Command { get; set; }

        public void Action()
        {
            Command.Execute();
        }
    }

    abstract class Receiver
    {
        /// <summary>
        /// 每个接收者都应该完成的业务
        /// </summary>
        public abstract void DoSomething();
    }

    class ConcreteReceiver : Receiver
    {
        public override void DoSomething()
        {
        }
    }

    class ConcreteReceiver2 : Receiver
    {
        public override void DoSomething()
        {
        }
    }

    #endregion

}
