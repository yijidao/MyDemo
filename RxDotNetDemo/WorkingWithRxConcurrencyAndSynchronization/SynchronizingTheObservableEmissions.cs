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
            new[] { 0, 1, 2, 3, 4, 5 }.ToObservable()
                .Take(3).LogWithThread("A")
                .Where(x => x % 2 == 0).LogWithThread("B")
                .SubscribeOn(NewThreadScheduler.Default).LogWithThread("C") // SubscribeOn 会影响所有操作的线程，所以他的位置在哪，其实不重要
                .Select(x => x * x).LogWithThread("D")
                .ObserveOn(TaskPoolScheduler.Default).LogWithThread("E") // ObserveOn 会影响后续操作的线程，也就是 LogWithThread 和 SubscribeConsole
                .SubscribeConsole("squares by time");
        }

        /// <summary>
        /// Rx 的每个操作符的输入应该保证序列化输入，这样才不会因为并发导致各种各样的问题
        /// 但是并发的情况总是存在的，如 Observable 是第三方提供的，或者在多线程的情况下调用
        /// Synchronize 可以确保通知是序列化执行的，Synchronize 内部实现了 lock，用于 lock 的对象被称为 gate
        /// 这个demo 展示了如何通过 Synchronize 确保通知序列化
        /// </summary>
        public static void SynchronizingNotifications()
        {
            var messenger = new Messenger();

            Observable.FromEventPattern<string>(h => messenger.MessageReceived += h,
                    h => messenger.MessageReceived -= h)
                .Select(x => x.EventArgs)
                .Synchronize()
                .Subscribe(msg =>
                {
                    Console.WriteLine($"Message {msg} arrived");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    Console.WriteLine($"Message {msg} exit");
                });

            Task.Run(() => messenger.RaiseEvent("msg1"));
            Task.Run(() => messenger.RaiseEvent("msg2"));
            Task.Run(() => messenger.RaiseEvent("msg3"));
        }

        /// <summary>
        /// Synchronize 接受一个 object 对象用于 lock，这个特性可以用于多个 Observable 的订阅序列进行
        /// </summary>
        public static void SynchronizingNotificationsWithGate()
        {
            var message = new Messenger();
            var gate = new object();

            Observable.FromEventPattern<string>(h => message.MessageReceived += h, h => message.MessageReceived -= h)
                .Select(x => x.EventArgs)
                .Synchronize(gate)
                .Subscribe(msg =>
                {
                    Console.WriteLine($"Message {msg} arrived");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    Console.WriteLine($"Message {msg} exit");
                });

            Observable.FromEventPattern<string>(h => message.MessageReceived2 += h, h => message.MessageReceived2 -= h)
                .Select(x => x.EventArgs)
                .Synchronize(gate)
                .Subscribe(msg =>
                {
                    Console.WriteLine($"Message2 {msg} arrived");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    Console.WriteLine($"Message2 {msg} exit");
                });

            message.RaiseAllEvent("msg1");
            message.RaiseAllEvent("msg2");
            message.RaiseAllEvent("msg3");
        }

    }

    class Messenger
    {
        public event EventHandler<string> MessageReceived;

        public event EventHandler<string> MessageReceived2;

        public void RaiseEvent(string value)
        {
            MessageReceived?.Invoke(this, value);
        }

        public void RaiseAllEvent(string value)
        {
            MessageReceived?.Invoke(this, value);
            MessageReceived2?.Invoke(this, value);
        }
    }
}
