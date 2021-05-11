using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reactive.Subjects;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.Controlling_the_observable_temperature.Multicasing_with_subjects
{
    /// <summary>
    /// Subject 既可以充当Observable，也可以充当Observer，所以它经常被作为 Observable 和 Observer 沟通的媒介
    /// ISubject 实现了三个接口，ISubject, IObservable（所以它拥有 Subscribe()）, IObserver（所以它拥有 OnNext()、OnError()、OnComplete()）
    /// Rx 提供了四种 ISubject 的实现
    /// 1. Subject<T>: 广播每个接收到的消息给所有 Observer
    /// 2. AsyncSubject<T>: 代表一个异步操作，并且在异步完成时，发射出异步操作的返回值
    /// 3. ReplaySubjec<T>: 广播当前和未来收到的消息给Observer
    /// 4. BehaviorSuject<T>: 只广播最新的值。
    /// </summary>
    public class SimpleBroadcastingWithSubject
    {
        /// <summary>
        /// Subject 实现广播，这里会有多个OnComplete 被调用
        /// </summary>
        public static void SubjectToBroad()
        {
            var sbj = new Subject<int>();
            sbj.SubscribeConsole("First");
            sbj.SubscribeConsole("Second");

            sbj.OnNext(1);
            sbj.OnNext(2);
            sbj.OnCompleted();
        }

        
    }
}
