using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.WorkingWithRxConcurrencyAndSynchronization
{
    /// <summary>
    /// rx 有时间线的概念，然后 rx 又是通过 IScheduler 来实现调度的
    /// rx 的设计理念： 任何涉及到并发的事情，都必须使用 Scheduler 类。
    /// Scheduler 是一个 rx 用于并发和事件的抽象层
    /// IScheduler 定义了当前时间属性和几个调度重载
    /// 除了实现 ISchedule，还可以实现 ISchedulerPeriodic（增加了一个周期性的方法定义） 和 ISchedulerLongRunning（增加了一个长期运行的方法定义）。
    /// </summary>
    public class ControllingConcurrencyWithSchedulers
    {
        /// <summary>
        /// 一个简单的两秒执行 demo
        /// 不是通过定时器，也不是 interval，而是通过递归向下调度来实现这个功能的
        /// </summary>
        public static void FirstScheduler()
        {
            var scheduler = NewThreadScheduler.Default;
            Func<IScheduler, int, IDisposable> action = null;

            action = (scdlr, callNumber) =>
            {
                Console.WriteLine(
                    $"Hello {callNumber}, Now: {scdlr.Now}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                return scdlr.Schedule(callNumber + 1, TimeSpan.FromSeconds(2), action);
            };

            var scheduling = scheduler.Schedule(0, TimeSpan.FromSeconds(2), action);

            //ISchedulerPeriodic
            //IScheduler
            //ISchedulerLongRunning
        }

        /// <summary>
        /// Rx 的操作符都有一个重载，接受 IScheduler 参数，用于指定默认的执行模式
        /// CurrentThreadScheduler.Instance 可以指定在当前线程，这会阻塞线程变成同步
        /// </summary>
        public static void ParameterizingConcurrency()
        {
            Console.WriteLine($"Before - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Observable.Interval(TimeSpan.FromSeconds(1), CurrentThreadScheduler.Instance) //
                .Take(3)
                .Subscribe(x => Console.WriteLine($"Inside - Thread:{Thread.CurrentThread.ManagedThreadId}"));
                //.SubscribeConsole($"Inside - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"After - Thread:{Thread.CurrentThread.ManagedThreadId}"); // 只有上面执行完，才会执行这一行
        }

        /// <summary>
        /// 不指定 CurrentThreadScheduler.Instance 的话，Interval 会切换到其他线程执行
        /// </summary>
        public static void ParameterizingConcurrency2()
        {
            Console.WriteLine($"Before - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(3)
                .Subscribe(x => Console.WriteLine($"Inside - Thread:{Thread.CurrentThread.ManagedThreadId}")); // 直接打印会切换到不同线程
                //.SubscribeConsole($"Inside - Thread:{Thread.CurrentThread.ManagedThreadId}"); // 用拓展方法就会切换到同一个线程，为啥呢？
            Console.WriteLine($"After - Thread:{Thread.CurrentThread.ManagedThreadId}"); // 不需要上面执行完才执行这一行
        }

        /// <summary>
        /// Rx 并非所有操作都在后台线程执行，如果需要明确执行线程，最好指定 IScheduler
        /// 这个 demo 展示了没有指定 IScheduler 的情况下，rx 却在同一线程上执行，导致阻塞方法执行
        /// </summary>
        public static void ParameterizingConcurrency3()
        {
            Console.WriteLine($"Before - Thread:{Thread.CurrentThread.ManagedThreadId}");
            var o = Observable.Range(1, 5)
                .Repeat() // repeat 会不停地在这个方法的线程不停地重新订阅这个observable，导致方法阻塞
                .Subscribe(x => Console.WriteLine($"Inside - Thread:{Thread.CurrentThread.ManagedThreadId}"));
            o.Dispose(); // 这一行永远不会执行，所以会不停地在控制台上打印

            Console.WriteLine($"After - Thread:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
