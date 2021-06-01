using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// TAP 在 .net 中有两个关键的实现类，Task 和 Task<TResult>。Task 和 Task<TResult> 的区别是，Task 返回 Task，Task<TResult> 直接返回计算后的结果。
        /// Task 是 future 模型的 .net 实现。future 初始化的时候不知道计算结果，但是在计算完毕后，就会得到计算结果。
        /// TAP 内部使用了 TaskScheduler 来进行任务的调度。当 Task 创建并开始后，Task 会被推入队列，并由 TaskScheduler 进行管理。
        /// Task.Status 可以显示任务的状态，
        /// Task 新建的时候，状态是 WaitingForActivation，
        /// 被 TaskScheduler 分配线程后，状态是 Running，
        /// 完成计算后，状态时 RanToCompletion。
        /// 在计算完成后，可以使用 Task.Result 来获得计算后的结果。但是这跟 wait 一样，会阻塞线程。
        /// 
        /// 
        /// </summary>
        public static void TaskBaseAsynchronousPattern()
        {
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync("http://ReactiveX.io");
            Console.WriteLine($"the request was sent, status:{request.Status}");
            Console.WriteLine(request.Result.Headers);
        }

        /// <summary>
        /// 直接调用Task.Result 会阻塞线程。所以一般不用，可以使用 continueWith 来取代这个操作。
        /// ContinueWith 中的代码会在 Task 完成（无论成功还是失败）之后执行，ContinueWith 会接受前一个Task 作为参数。
        /// </summary>
        public static void UseContinueWith()
        {
            var httpClient = new HttpClient();
            httpClient.GetAsync("http://ReactiveX.io")
                .ContinueWith(requestTask =>
                {
                    Console.WriteLine($"the request was sent, status: {requestTask.Status}"); // 这里的状态就是 RanToCompletion
                    Console.WriteLine(requestTask.Result.Headers);
                });
        }

        /// <summary>
        /// ContinueWith 有一个问题，就是写起来很麻烦，需要循环嵌套，太长了
        /// 这个demo 展示了ContinueWith 嵌套的问题
        /// </summary>
        public static void UseContinueWith2()
        {
            var httpClient = new HttpClient();
            httpClient.GetAsync("http://ReactiveX.io")
                .ContinueWith(requestTask =>
                {
                    var httpContent = requestTask.Result.Content;
                    httpContent.ReadAsStringAsync()
                        .ContinueWith(contentTask =>
                        {
                            Console.WriteLine(contentTask.Result);
                        });
                });
        }

        /// <summary>
        /// 使用 async - await 来替代 ContinueWith，可以是代码更简洁可读
        /// 当使用了 async，编译器就会生成一个 Task
        /// 其实一般情况下，异步代码就是这样的，一行行的 async - await
        /// </summary>
        public static async void SimplifyingAsynchronousCodeWithAsyncAwait()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://ReactiveX.io");
            var page = await response.Content.ReadAsStringAsync();
            Console.WriteLine(page);
        }


        /// <summary>
        /// 使用 async 可以通知编译器生成一个 Task，但是如果代码内部并没有支持异步操作，那么就不会切换线程，
        /// 而且这种使用 async 去修饰同步方法，编译器依旧会生成一个状态机去管理代码，其实有损代码性能的。
        ///
        /// 新建 Task 可以调用 Task.Run
        /// </summary>
        public static async void AsyncMethodCaller()
        {
            var isSame = await MyAsyncMethod(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine($"1 - {isSame}"); // True

            var isSame2 = await MyAsyncMethod2(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine($"2 - {isSame2}"); // False
        }

        public static async Task<bool> MyAsyncMethod(int callingThreadId)
        {
            return callingThreadId == Thread.CurrentThread.ManagedThreadId;
        }

        public static async Task<bool> MyAsyncMethod2(int callingThreadId)
        {
            return await Task.Run(() => callingThreadId == Thread.CurrentThread.ManagedThreadId);
        }
    }
}
