using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

namespace DesignPattern.DesignPattern
{
    /// <summary>
    /// 责任链模式
    /// Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request.
    /// Chain the receiving objects and pass the request along the chain until an object handles it.
    /// 使多个对象有机会去处理请求，从而避免耦合请求的发送者和接收者。
    /// 将多个接收者对象连成链条，并沿着这条链条去处理请求，直到有对象处理了该请求。
    ///
    /// 优点：
    /// 1. 将请求和响应分离开。
    /// 2. 符合最小知识原则。
    /// 缺点：
    /// 1. 遍历链条，如果链条长，性能和调试都很麻烦。所以可以加一个最大结点来限制链条长度。
    /// 
    /// </summary>
    class ChainOfResponsibilityPattern
    {
        public void Demo()
        {
            var request = new Request { Level = Level.Second };

            var handler = new ConcreteHandler();
            var handler2 = new ConcreteHandler2();

            handler.NextHanlder = handler2;
            handler.HandleMessage(request);

            request = new Request { Level = Level.First };

            handler2.NextHanlder = handler;
            handler2.HandleMessage(request);
        }
    }

    #region 责任链模式模版代码




    enum Level
    {
        First,
        Second,
        Third
    }

    class Request
    {
        public Level Level { get; set; }
    }

    class Response
    {

    }

    abstract class Handler
    {
        public Handler NextHanlder { get; set; }

        public Response HandleMessage(Request request)
        {
            // 请求属于当前 level 则进行处理，不属于则到下一个处理器进行处理。
            Response response = GetLevel().Equals(request.Level) ? Echo(request) : NextHanlder.Echo(request);
            return response;
        }

        /// <summary>
        /// 具体处理逻辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Response Echo(Request request);

        public abstract Level GetLevel();

    }

    class ConcreteHandler : Handler
    {

        protected override Response Echo(Request request)
        {
            Console.WriteLine("Handler 处理响应");
            return null;
        }

        public override Level GetLevel() => Level.First;
    }

    class ConcreteHandler2 : Handler
    {

        protected override Response Echo(Request request)
        {
            Console.WriteLine("Handler2 处理响应");
            return null;
        }

        public override Level GetLevel() => Level.Second;
    }
    #endregion

}
