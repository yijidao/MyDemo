using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RxDotNetDemo.Asynchronous
{
    class AsynchronousCodeInDotNet
    {
        /// <summary>
        /// 直接 new thread 是一个不好的方法，因为新建一个线程一般需要1M 的内存，很浪费资源，而销毁线程也会耗费资源
        /// </summary>
        public static void ManualCreateThread()
        {
            var t = new Thread(() =>
            {
                // 一个耗时的工作
                Console.WriteLine("Long work is done,the result is ...");
            });
            t.Start();
        }

        /// <summary>
        /// 使用线程池新建线程。线程池会负责线程的创建和复用，同时内部会有一个工作线程负责调度。
        /// 直接使用线程有两个问题，
        /// 1. 无法简单地知道线程是否已经结束。
        /// 2. 无法简单地得到线程计算地结果。
        /// 而使用异步的场景常常就是计算并返回值，所以直接使用线程其实不是常用。
        /// 为了满足这种场景，.NET 提供了一些模式去完成这个需求。The Asynchronous Programming(APM)、The Event-Based Asynchronous Pattern(EAP)、Task-Base Asynchronous Pattern(TAP)
        ///
        /// APM：
        /// 有两个方法 Begin[OperationName] 和 End[OperationName]。在调用完 Begin 之后会立刻返回实现了 IAsyncResult 的对象。End 接受一个 IAsyncResult 参数，并且会返回异步操作的值。
        ///
        /// EAP：
        /// 当调用一个耗时的 [MethodName]Async，容器类就会对应生成一个 [MethodName]Completed，并且在操作完成时调用。
        ///
        /// 在.NET 4之后，推荐的异步模式时 Task-Base Asynchronous Pattern(TAP),TAP 是基于 Task Parallel Library(TPL) 实现的。其他模式也可以转成 TPL。
        /// 
        /// </summary>
        public static void CreateThreadByThreadPool()
        {

            ThreadPool.QueueUserWorkItem((_) =>
            {
                Console.WriteLine("Long work is done,the result is ...");
            });
        }

    }
}
