using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.PartitioningAndCombiningObservables
{
    public class CombiningObservables
    {
        /// <summary>
        /// zip 可以实现两个 observable 发射出的通知成对配对，并通过 resultSelector 组合成新的流
        /// zip 有个问题就是是按通知成对配对的，所以当两个 observable 的速率不一致时，会有内存压力，可以使用 CombineLatest 解决这个问题
        /// </summary>
        public static void ParingItemsFromObservables()
        {
            var o = Observable.Range(1, 5);
            var o2 = Observable.Interval(TimeSpan.FromSeconds(1));
            o.Zip(o2, (i, l) => i + l)
                .SubscribeConsole("Zip");
        }

        /// <summary>
        /// CombineLatest 可以实现两个 Observable 每次都组合最新发射出的通知
        /// 只有当两个 Observable 都已经发射出通知，CombineLatest 才会组合值，所以为了防止丢失值，可以使用 StartWith 来设置初始值
        /// </summary>
        public static void CombiningTheLatestEmittedValue()
        {
            var sbj = new Subject<int>();
            var sbj2 = new Subject<int>();

            sbj.StartWith(0) // 使用 StartWith 来设置初始值，防止丢失已发送的通知
                .CombineLatest(sbj2.StartWith(0),
                    (a, b) => $"a:{a}  b:{b}")
                .SubscribeConsole("CombineLatest");

            sbj.OnNext(100);
            sbj.OnNext(101);
            sbj2.OnNext(200);
            sbj2.OnNext(201);
            sbj.OnNext(102);
        }

        /// <summary>
        /// 使用 concat 串联两个 task 的 Observable，因为 task.OoObservable 会把 Hot 变成 Cold，所以不用担心顺序问题
        /// </summary>
        public static void ConcatenatingObservables()
        {
            var t = Task.Delay(1000).ContinueWith(_ => new[] {"A1","A2"});
            var t2 = Task.FromResult(new[] {"B1", "B2"});

            t.ToObservable().Concat(t2.ToObservable())
                .SelectMany(x => x)
                .SubscribeConsole("Concat");
        }

        /// <summary>
        /// 因为 concat 是等第一个 Observable OnComplete 之后再去订阅第二个 Observable，所以如果第二个 Observable 是 Hot，则可能会丢失通知
        /// </summary>
        public static void ConcatenatingObservables2()
        {
            var sbj = new Subject<string>();
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).Select(x => $"A{x}");
            var o2 = sbj.AsObservable();
            //var o = new[] {"A1", "A2"}.ToObservable();
            o.Concat(o2).SubscribeConsole("ColdAndHot");
            sbj.OnNext("B1");
            sbj.OnNext("B2");
            Task.Delay(TimeSpan.FromSeconds(7)).Wait();
            sbj.OnNext("B3");
            sbj.OnNext("B4");
        }

        /// <summary>
        ///  Concat 两个 Cold  Observable，不会丢失通知
        /// </summary>
        public static void ConcatenatingObservables3()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).Select(x => $"A{x}");
            var o2 = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).Select(x => $"B{x}");
            o.Concat(o2)
                .SubscribeConsole("ColdAndCold");
        }
    }
}
