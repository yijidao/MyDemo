using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.Controlling_the_observable_temperature
{
    /// <summary>
    /// Hot Observable：
    /// 1. Hot 是一个与 Observer 无关的 Observable，即使没有 Observable 订阅，依旧会发射通知，并且通知共享给所有 Observable。
    /// 2. FromEventPattern 产生的 Observable 就是 Hot 的。
    /// 
    /// Cold Observable：
    /// 1. Cold 只有在 Observer 订阅时才会发射通知，并且是发射完整序列的通知，每个 Observable 收到的通知都是互相隔离的。
    /// 2. Create、Defer、Range、Interval 等操作符产生的 Observable  都是 Cold 的。
    /// </summary>
    public class HeatingAndCoolingObservable
    {
        /// <summary>
        /// 将生产者从冷转到热分为以下几个步骤
        /// 1. 创建一个 subject 充当中间的媒介。
        /// 2. observer 订阅 subject。
        /// 3. subject 订阅 cold observable，并广播消息。
        ///
        /// Rx 提供了 Publish 和 Connect 完成以上几个步骤
        /// Publish() 返回一个 IConnectableObservable 的代理，它实现了对 Cold 的订阅。
        /// Publish 默认实现是生成一个普通的 Subject
        /// 
        /// Connect() 提供了一个时机，决定了什么时候开始发射消息
        /// </summary>
        public static void TurnColdObservableHot()
        {
            var cold = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var co = cold.Publish();
            co.SubscribeConsole("first");
            co.SubscribeConsole("second");

            co.Connect();
            Task.Delay(6000).Wait();
            co.SubscribeConsole("third");
        }

        /// <summary>
        /// Publish 重载，可以实现 BehaviorSubject
        /// </summary>
        public static void TurnColdObservableHotWithCacheValue()
        {
            var cold = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)).Select(index => $"timer_{index}");
            var o = cold.Publish("time_init");
            o.Connect();
            o.SubscribeConsole("first");

            Task.Delay(6000).Wait();
            o.SubscribeConsole("second");
        }

        /// <summary>
        /// 重用 Observable 是一个复杂的事情，所以这里提供了 ReusingThePublishObservable() 和 ReusingThePublishObservable2() 两种场景
        /// 使用 zip 重用 Observable
        /// </summary>
        public static void ReusingThePublishObservable()
        {
            int i = 0; // 这里使用了一个共享变量，每次都会被修改
            var numbers = Observable.Range(1, 5).Select(_ => i++);
            var o = numbers.Zip(numbers, (i1, i2) => i1 + i2);
            o.SubscribeConsole(); // 输出 1 5 9 13 17
            
        }

        /// <summary>
        /// 使用 Publish 的重载和 Zip 来实现重用 Observable
        /// </summary>
        public static void ReusingThePublishObservable2()
        {
            int i = 0; // 这里虽然使用了一个变量，但是这种实现不会被共享，所以不会每次都被修改
            var numbers = Observable.Range(1, 5).Select(_ => i++);
            var o = numbers.Publish(published => published.Zip(published, (a, b) => a + b));
            o.SubscribeConsole();// 输出 0 2 4 6 8
        }

        /// <summary>
        /// PubishLast 可以实现 AsyncSubject
        /// </summary>
        public static void PublishLastDemo()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Take(3);
            var co = o.PublishLast();
            co.Connect();
            co.SubscribeConsole("first");
            Task.Delay(5000).Wait();
            co.SubscribeConsole("second");
        }

    }
}
