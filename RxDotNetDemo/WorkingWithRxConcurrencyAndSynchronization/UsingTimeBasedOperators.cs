using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.WorkingWithRxConcurrencyAndSynchronization
{
    /// <summary>
    /// Observable 和 IEnumerable 的一个区别就是，Observable 是有时间线概念的，每个通知之间具有间隔时间，可以根据这个时间做一些过滤、合并等操作
    /// </summary>
    public class UsingTimeBasedOperators
    {
        /// <summary>
        /// Timestamp 记录每个通知的时间
        /// Timestamp 包装后的通知，有一个value 属性，可以取回原始值
        /// </summary>
        public static void AddTimestamp()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(5));
            o.Take(3)
                .Timestamp()
                .SubscribeConsole("Timestamp");
        }



        /// <summary>
        /// TimeInterval 记录每个通知之间时间的间隔
        /// </summary>
        public static void AddTimeIntervalBetweenNotifications()
        {
            var o = Observable.Timer(TimeSpan.FromSeconds(1))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(2)))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)));
            o.TimeInterval()
                .SubscribeConsole("TimeInterval");
        }

        /// <summary>
        /// Timeout 会在两个通知之间增加一个超时检测，一旦超时，就会抛出 TimeoutException，并执行 OnError
        /// </summary>
        public static void AddingTimeoutPolicy()
        {
            var o = Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "first")
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "second"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)).Select(_ => "third"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)).Select(_ => "fourth"))
                .Timeout(TimeSpan.FromSeconds(2))
                .SubscribeConsole("Timeout");
        }

        /// <summary>
        /// Delay 可以延时发射通知，一般都用优先级场景，比如说低优先级延迟时间
        /// </summary>
        public static void DelayingTheNotifications()
        {
            var o = Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "first")
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "second"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)).Select(_ => "third"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(4)).Select(_ => "fourth"))
                .Timestamp() // 记录原始的时间
                .Delay(TimeSpan.FromSeconds(2))
                .Timestamp() // 记录延时后的时间
                .Take(5)
                .SubscribeConsole("Delay");
        }

        /// <summary>
        /// 动态延时
        /// 使用 delay 的重载接收一个 Observable，可以用来实现动态延时延时
        /// 这个demo 就很好地展示了，根据不同的优先级来进行延时
        /// </summary>
        public static void AddingVariableDelay()
        {
            var o = new[] { 4, 1, 2, 3 }.ToObservable()
                .Timestamp()
                .Delay(x => Observable.Timer(TimeSpan.FromSeconds(x.Value)))
                .Timestamp()
                .SubscribeConsole("Variable Delay");
        }

        /// <summary>
        /// 使用 Throttle 来进行限流
        /// </summary>
        public static void ThrottlingNotifications()
        {
            Observable.Return("first")
                .Concat(Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "second"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "third"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "fourth"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "fifth"))
                .Timestamp()
                .Throttle(TimeSpan.FromSeconds(2))
                .Timestamp()
                .SubscribeConsole("Throttle");
        }

        /// <summary>
        /// 动态限流，既可以避免接收大量无用信息，也可以避免错过重要的消息
        /// Throttle 有一个重载，接受Observable，可以用来动态限流
        /// </summary>
        public static void VariableThrottle()
        {
            Observable.Return("first")
                .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "second"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "third"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "Immediate Update"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => "fourth"))
                .Concat(Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "fifth"))
                .Timestamp()
                .Throttle(x => x.Value == "Immediate Update" ? Observable.Empty<long>() : Observable.Timer(TimeSpan.FromSeconds(2)))
                .Timestamp()
                .SubscribeConsole("Variable Throttle");
        }

        /// <summary>
        /// Sample 固定时间取样，实在定时从 Observable 中获取最新的通知
        /// </summary>
        public static void SamplingObservableInIntervals()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(x => x + 1)
                .Sample(TimeSpan.FromSeconds(3.5))
                .SubscribeConsole("Sample");
        }
    }
}
