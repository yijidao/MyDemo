using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;

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




    }
}
