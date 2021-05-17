using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.WorkingWithRxConcurrencyAndSynchronization
{
    /// <summary>
    /// rx 默认是可能执行在任何线程的，所以 rx 提供了一些工具去指定 scheduler，从而实现指定线程
    /// </summary>
    public class SynchronizingTheObservableEmissions
    {
        /// <summary>
        /// 使用 ObserveOn 改变执行线程
        /// WPF 一般可以通过 ObserveOn(DispatcherScheduler.Current) 指定之后的操作在UI线程执行，可以用 ObserveOnDispatcher 简写代替
        /// 因为 Throttle 是默认的 scheduler，所以一般是从线程池直接取数据
        /// </summary>
        public static void ChangingTheObservationsExecutionContext()
        {
            //Observable.FromEventPattern(TextBox, "TextChanged")
            //    .Select(_ => TextBox.Text)
            //    .Throttle(TimeSpan.FromMilliseconds(400))
            //    .ObserveOn(DispatcherScheduler.Current)
            //    .Subscribe(t => ThrottledResults.Items.Add(t));
        }

        /// <summary>
        /// 使用 SubscribeOn 改变订阅和取消订阅的上下文，也就是说在 subscribe 和 dispose 会在指定线程上执行
        /// </summary>
        public static void ChangingTheSubscriptionOrUnSubscriptionExecutionContext()
        {
            var eventLoopScheduler = new EventLoopScheduler();
            

            var subscription = Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
                .Do(_ => Console.WriteLine("Inside Do"))
                .SubscribeOn(eventLoopScheduler)
            .SubscribeConsole();


            eventLoopScheduler.Schedule(1, (scheduler, station) =>
            {
                Console.WriteLine("Before sleep");
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Console.WriteLine("After sleep");
                return Disposable.Empty;
            });

            subscription.Dispose();
            Console.WriteLine("Subscription disposed");
        }

        /// <summary>
        /// ObserveOn 和 SubscribeOn 结合使用
        /// SubscribeOn 一般用于切换所有操作到后台线程，位置不重要
        /// ObserveOn 一般用于切换后续操作到UI线程，位置影响结果
        /// subscribe 是从下到上执行，而消息的通知，也就是 onNext、OnComplete 等则是从上到下执行
        /// </summary>
        public static void UsingSubscribeOnAndObserveOnTogether()
        {
            new[] {0, 1, 2, 3, 4, 5}.ToObservable()
                .Take(3).LogWithThread("A")
                .Where(x => x % 2 == 0).LogWithThread("B")
                .SubscribeOn(NewThreadScheduler.Default).LogWithThread("C") // SubscribeOn 会影响所有操作的线程，所以他的位置在哪，其实不重要
                .Select(x => x * x).LogWithThread("D")
                .ObserveOn(TaskPoolScheduler.Default).LogWithThread("E") // ObserveOn 会影响后续操作的线程，也就是 LogWithThread 和 SubscribeConsole
                .SubscribeConsole("squares by time");
        }

    }
}
