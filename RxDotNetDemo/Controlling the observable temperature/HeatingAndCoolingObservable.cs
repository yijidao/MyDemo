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
        /// Publish 返回一个 IConnectableObservable
        /// </summary>
        public static void TurnColdObservableHot()
        {
            var cold = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var co = cold.Publish();
            co.SubscribeConsole("first");
            co.SubscribeConsole("second");

            co.Connect();
            Task.Delay(2000).Wait();
            co.SubscribeConsole("third");
        }


    }
}
