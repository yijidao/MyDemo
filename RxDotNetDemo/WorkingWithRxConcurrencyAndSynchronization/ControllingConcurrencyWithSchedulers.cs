using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
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

        /// <summary>
        /// scheduler 测试类
        /// 一个 scheduler 执行两个调度
        /// </summary>
        /// <param name="scheduler"></param>
        public static void TestScheduler(IScheduler scheduler)
        {
            scheduler.Schedule(Unit.Default, (s, _) => Console.WriteLine($"Action1 - Thread:{Thread.CurrentThread.ManagedThreadId}"));
            scheduler.Schedule(Unit.Default, (s, _) => Console.WriteLine($"Action2 - Thread:{Thread.CurrentThread.ManagedThreadId}"));
        }

        /// <summary>
        /// NewThreadScheduler 会在新线程运行调度，一般不会通过构造函数去生成 Scheduler，而是调用 Scheduler.Default 去接受一个共享的实例
        /// 构造函数也可以传一个工厂委托去生成线程
        /// 需要注意的一个问题是，NewThreadScheduler 虽然每次都会新建一个线程，但是内部递归调度并不会再次新建线程，而是共享之前创建的线程
        /// 因为新建线程开销大，所以只有执行长时间的任务，采用 NewThreadScheduler，其他时间短的任务，应该用其他 Scheduler，如ThreadPoolScheduler
        /// </summary>
        public static void TestNewThreadScheduler()
        {
            TestScheduler(NewThreadScheduler.Default); // 
            //new NewThreadScheduler(threadFactory) // 也接受一个工厂参数
        }

        /// <summary>
        /// 新建一个线程开销比较大，比如说内存就要1M左右，所以最好线程要能共享
        /// ThreadPoolScheduler 是基于线程池实现的调度
        /// 跟NewThreadScheduler 不一致的是，ThreadPoolScheduler 的内部递归调度，是以队列的方式，排队从线程池中取数据，所以每次运行的线程可能不同
        /// PoolScheduler 是一个用的最多的 Scheduler
        /// </summary>
        public static void TestThreadPoolScheduler()
        {
            TestScheduler(ThreadPoolScheduler.Instance);
        }

        /// <summary>
        /// TaskPoolScheduler 工作方式跟 ThreadPoolScheduler 相似，只是 TaskPoolScheduler 是基于 TPL(Task Parallel Library) 的
        /// 在一些没有实现线程池平台，那么可以用 TaskPoolScheduler
        /// </summary>
        public static void TestTaskPoolScheduler()
        {
            TestScheduler(TaskPoolScheduler.Default);
        }

        /// <summary>
        /// CurrentThreadScheduler 的线程跟执行的线程是一样的，而且内部递归调用也是以队列的方式，排队执行
        /// 所以这个Scheduler 会阻塞线程
        /// </summary>
        public static void TestCurrentThreadScheduler()
        {
            Console.WriteLine($"Call thread:{Thread.CurrentThread.ManagedThreadId}");
            TestScheduler(CurrentThreadScheduler.Instance);
        }

        /// <summary>
        /// ImmediateScheduler 跟 CurrentThreadScheduler 有点相似，都是线程跟执行的线程一样，并且会阻塞线程
        /// 不同的是，内部的 scheduler 递归调用不会等上一个 scheduler 执行完毕，再执行下一个，而是立刻执行下一个
        /// 所以 ImmediateScheduler 很适合一些定时执行小工作
        /// </summary>
        public static void TestImmediateScheduler()
        {
            var immediateScheduler = ImmediateScheduler.Instance;

            Console.WriteLine($"Calling thread:{Thread.CurrentThread.ManagedThreadId} Current time:{immediateScheduler.Now}");

            immediateScheduler.Schedule(Unit.Default, TimeSpan.FromSeconds(2), (IScheduler scheduler, Unit action) =>
            {
                Console.WriteLine($"Outer Action - Thread:{Thread.CurrentThread.ManagedThreadId} Current time:{immediateScheduler.Now}");
                scheduler.Schedule(Unit.Default, (IScheduler scheduler2, Unit action2) =>
                {
                    Console.WriteLine($"Inner Action - Thread:{Thread.CurrentThread.ManagedThreadId}"); // 这里会立刻打印，而不会等 Outer Action - Done 打印完再打印
                    return Disposable.Empty;
                });
                Console.WriteLine($"Outer Action - Done");
                return Disposable.Empty;
            });

            Console.WriteLine($"After the Schedule,Time: {immediateScheduler.Now}");
        }

        /// <summary>
        /// EventLoopScheduler 会把所有的操作都运行到一个线程上，内部的递归操作是按时间队列进行执行的，上一个执行完并出队，下一个才会执行
        /// </summary>
        public static void TestEventLoopScheduler()
        {
            Console.WriteLine($"Calling thread:{Thread.CurrentThread.ManagedThreadId}");
            TestScheduler( new EventLoopScheduler());
        }

        /// <summary>
        /// SynchronizationContext 是在特定上下文下执行
        /// System.Reactive.Windows.Threading 中有 DispatcherScheduler 用于WPF执行
        /// </summary>
        public static void TestSynchronizationContext()
        {
            
        }

    }
}
