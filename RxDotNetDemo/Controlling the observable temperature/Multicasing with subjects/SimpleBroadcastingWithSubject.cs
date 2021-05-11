using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
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
        /// Subject 实现广播
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

        /// <summary>
        /// 如果 subject 订阅了多个 Observable，只要其中一个调用了 OnComplete，那么 Subject 就会调用 OnComplete 并结束
        /// </summary>
        public static void MultipleSource()
        {
            var sbj = new Subject<string>();
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(x => $"First: {x}")
                .Take(5)
                .Subscribe(sbj);
            Observable.Interval(TimeSpan.FromSeconds(2))
                .Select(x => $"Second: {x}")
                .Take(5)
                .Subscribe(sbj);
            sbj.SubscribeConsole();
        }

        /// <summary>
        /// 一个典型的错误使用 Subject 的方式，就是用来合并两个 Observable，合并应该用 Meger
        /// 使用 Subject 前先思考一下，有没有重新造轮子了
        /// </summary>
        public static void ClassicMisuse()
        {
            var sbj = new Subject<string>();
            sbj.SubscribeConsole();

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Select(x => $"IEnumerable {x}").Subscribe(sbj); // 这个 Observable 没有机会完成，因为另一个 Observable 先完成了，所以整个Subject 就完成了
            PrimeGenerator.GenerateAsync(5).ToObservable().SelectMany(x => x.Select(y => $"Task {y}")).Subscribe(sbj);
        }
    }
}
