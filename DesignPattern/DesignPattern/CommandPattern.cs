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
    /// - Receive 接收者角色
    /// - Command 命令者角色
    /// - Invoker 调用者角色
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
