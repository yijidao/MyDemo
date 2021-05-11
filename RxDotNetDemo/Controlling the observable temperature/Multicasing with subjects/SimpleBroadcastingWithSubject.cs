using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.Controlling_the_observable_temperature.Multicasing_with_subjects
{
    /// <summary>
    /// Subject 既可以充当Observable，也可以充当Observer，所以它经常被作为 Observable 和 Observer 沟通的媒介
    /// ISubject 实现了三个接口，ISubject, IObservable（所以它拥有 Subscribe()）, IObserver（所以它拥有 OnNext()、OnError()、OnComplete()）
    /// Rx 提供了四种 ISubject 的实现
    /// 1. Subject<T>: 广播每个接收到的消息给所有 Observer，当订阅后，可以收到 subject 发射的通知，但是无法收到之前发射的通知
    /// 2. AsyncSubject<T>: 代表一个异步操作，并且在异步完成时，发射出异步操作的返回值
    /// 3. ReplaySubjec<T>: 广播当前和未来收到的消息给Observer
    /// 4. BehaviorSuject<T>: 广播并缓存最新值
    /// </summary>
    public class SimpleBroadcastingWithSubject
    {
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
        /// Subject 实现广播， 当订阅后，可以收到 subject 发射的通知，但是无法收到之前发射的通知
        /// </summary>
        public static void SubjectToBroad()
        {
            var sbj = new Subject<int>();
            //sbj.OnNext(1); // 在订阅前发射通知，则订阅无法收到
            //sbj.OnNext(2);

            sbj.SubscribeConsole("First");
            sbj.OnNext(1);
            sbj.SubscribeConsole("Second");
            sbj.OnNext(2);

            sbj.OnCompleted();
        }

        /// <summary>
        /// 一个典型的错误使用 Subject 的方式，就是用来合并两个 Observable，合并应该用 Merge
        /// 使用 Subject 前先思考一下，有没有重新造轮子了
        /// </summary>
        public static void ClassicMisuse()
        {
            var sbj = new Subject<string>();
            sbj.SubscribeConsole();

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Select(x => $"IEnumerable {x}").Subscribe(sbj); // 这个 Observable 没有机会完成，因为另一个 Observable 先完成了，所以整个Subject 就完成了
            PrimeGenerator.GenerateAsync(5).ToObservable().SelectMany(x => x.Select(y => $"Task {y}")).Subscribe(sbj);
        }

        /// <summary>
        /// 异步操作的Subject
        /// </summary>
        public static void AsyncSubjectConvertTask()
        {
            //var tcs = new TaskCompletionSource<bool>();
            //var task = tcs.Task;
            var sbj = new AsyncSubject<string>();
            sbj.SubscribeConsole();
            var task = PrimeGenerator.GenerateAsync(5);

            task.ContinueWith(t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        sbj.OnNext(string.Join(",", t.Result));
                        sbj.OnCompleted();
                        break;
                    case TaskStatus.Canceled:
                        sbj.OnError(t.Exception.InnerException);
                        break;
                    case TaskStatus.Faulted:
                        sbj.OnError(new TaskCanceledException(t));
                        break;
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
            //tcs.SetResult(true);
        }

        /// <summary>
        /// BehaviorSubject 可以设置初始值，并且每次都会发射并缓存最新值
        /// BehaviorSubject.Value 可以获取到初始值或者最新值
        /// 适合用于一些状态变化的场景
        /// </summary>
        public static void PreserveLastStateWithBehaviorSubject()
        {
            var sbj = new BehaviorSubject<int>(2);
            Console.WriteLine($"BehaviorSubject.Value:{sbj.Value}");
            sbj.SubscribeConsole("first");
            sbj.OnNext(3);
            sbj.SubscribeConsole("second");
            sbj.OnCompleted();
            Console.WriteLine($"BehaviorSubject.Value:{sbj.Value}");
        }

        /// <summary>
        /// ReplaySubject 会缓存一个序列的通知，可以接受 bufferSize 或者 window 的时间滑窗来限制缓存的大小，防止内存溢出
        /// </summary>
        public static void CacheSequenceWthReplaySubject()
        {
            var sbj = new ReplaySubject<long>(bufferSize: 5, window:TimeSpan.FromSeconds(3));
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(sbj);
            //sbj.SubscribeConsole("first");

            Task.Delay(5000).Wait();
            sbj.SubscribeConsole("second");
        }

        /// <summary>
        /// 使用 AsObservable 将 subject 转型成 observable 提供给用户
        /// 不能直接提供 subject，因为这样用户可以直接调用 onNext() onComplete() 等方法造成破坏性的修改
        /// </summary>
        public static void ProtectSubject()
        {
            var sbj = new Subject<int>();
            var proxy = sbj.AsObservable();

            var subject = proxy as Subject<int>;
            Console.WriteLine($"subject is null ? {subject is null}");

        }
    }
}
