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
    ///
    /// Publish 的实现中使用了 Multicast 方法，这是一个底层的方法，如果需要实现自己的 Publish，使用具体的 Subject，可以参考一下
    /// 
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
        /// PublishLast 可以实现 AsyncSubject
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

        /// <summary>
        /// 使用 IDispose.Dispose() 和 Connect() 来实现切换 subject 的源，主要用于切换服务或者数据源等场景吧
        /// 1. 必须先调用 Dispose 再调用 Connect
        /// 2. 必须还没调用 OnComplete
        /// </summary>
        public static void ReconnectConnectableObservable()
        {
            var co = Observable.Defer(() => Get()).Publish();
            co.SubscribeConsole("first");
            var s = co.Connect();
            Task.Delay(2000).Wait();
            s.Dispose();
            s = co.Connect();

            IObservable<long> Get()
            {
                return Observable.Interval(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// 调用 RefCount() 可以实现当没有Observer 订阅 subject 时，subject 自动取消对 observable 的订阅
        /// </summary>
        public static void AutoDisconnectWithRefCount()
        {
            var co = Observable.Interval(TimeSpan.FromSeconds(1)).Do(n =>
            {
                Console.WriteLine($"generate {n}");
            }).Publish().RefCount();
            //co.Connect();
            var s = co.SubscribeConsole("first");
            Task.Delay(5000).Wait();
            s.Dispose(); // 调用 dispose 取消对 subject 的订阅
            Task.Delay(5000).Wait();
            var s2 = co.SubscribeConsole("second");
            //s2.Dispose();
        }

        /// <summary>
        /// 调用 Replay() 可以实现 ReplaySubject 的功能，当调用 Replay 时，会缓存当前的通知和未来的通知，然后将整个序列发射给 observable
        /// 1. Replay() 可以实现 Hot To Cold
        /// 2. Replay() 无法缓存调用之前的通知
        /// 3. Replay() 有多个重载，支持个数或者时间滑动窗设置缓存大小
        /// </summary>
        public static void HotObserverToCold()
        {
            var co = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Replay();
            co.Connect();
            co.SubscribeConsole("first");

            Task.Delay(3000).Wait();
            co.SubscribeConsole("second");
        }
    }
}
