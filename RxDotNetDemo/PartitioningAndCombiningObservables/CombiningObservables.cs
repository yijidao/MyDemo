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

        /// <summary>
        /// merge可以把两个源的通知发射到 merge 后的源里，而且是实时的，所以这个demo，就算 o 在前面，o2 在后面，但是因为 o delay了一下，所以发射出的消息还是排在 o2 后面
        /// </summary>
        public static void MergingObservables()
        {
            var o = Task.Delay(500).ContinueWith(_ => new[] {"A1", "A2"}).ToObservable();
            var o2 = Task.FromResult(new []{"B1", "B2"}).ToObservable();
            o.Merge(o2)
                .SelectMany(x => x)
                .SubscribeConsole("merge");
        }

        /// <summary>
        /// 第一个 observer 的值生成第二个 observer，然后 merge 第二个 observer 发射的值
        /// 这个 demo 模拟当用户在界面中输入，然后调用服务去查询结果
        /// select + merge 的操作也可以使用 selectMany 直接实现
        /// </summary>
        public static void DynamicConcatenatingAndMerging()
        {
            var texts = new[] {"Hello", "World"}.ToObservable();
            texts.Select(txt => Observable.Return($"{txt}-Result"))
                .Merge()
                .SubscribeConsole("SelectAndMerge");

            Task.Delay(1000).Wait();

            texts.SelectMany(txt => Observable.Return($"{txt}-Result"))
                .SubscribeConsole("SelectMany");
        }

        /// <summary>
        /// 同时订阅太多的 observable 可能会有性能问题
        /// 所以可以给 Merge 设置参数，设定 Merge 最多同时订阅的 observable 个数
        /// </summary>
        public static void ControllingTheConcurrencyOfMerge()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i => $"first{i}").Take(2);
            var o2 = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i => $"second{i}").Take(3);
            var o3 = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i => $"third{i}").Take(2);

            new[] {o, o2, o3}.ToObservable()
                .Merge(2) // 设置 merge 每次最多订阅两个 Observable 进行消息的合并
                .SubscribeConsole("concurrency merge");
        }

        /// <summary>
        /// switch　接收一个 observable，这个 Observable 发射的通知也是 Observable, switch 会切换到最新收到的 observable 并订阅该对象，跟merge 不一样，merge 是订阅所有对象
        /// </summary>
        public static void SwitchLatestObservable()
        {
            var textSubject = new Subject<string>();
            var o = textSubject.AsObservable();
            o.Select(txt => Observable.Return($"{txt}-Result").Delay(TimeSpan.FromSeconds(txt == "R1" ? 1 : 0)))
                .Switch()
                .SubscribeConsole("SwitchLatest");
            textSubject.OnNext("R1");
            textSubject.OnNext("R2");
            Task.Delay(500).Wait();
            textSubject.OnNext("R3");
        }

        /// <summary>
        /// switch 这个 demo 更形象点
        /// </summary>
        public static void SwitchLatestObservable2()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => $"first-{x}");
            var o2 = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1)).Select(x => $"second-{x}");
            var o3 = Observable.Timer(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1)).Select(x => $"third-{x}");
            new[] { o,  o3, o2, }.ToObservable()
                .Switch()
                .SubscribeConsole("SwitchLatest");

            //var sbj = new Subject<IObservable<string>>();
            //var o4 = sbj.AsObservable();
            //o4.Switch().SubscribeConsole("SwitchLatest");

            //sbj.OnNext(o);
            //Task.Delay(3000).Wait();
            //sbj.OnNext(o2);
            //Task.Delay(8000).Wait();
            //sbj.OnNext(o3);



        }

        /// <summary>
        /// amb 会切换到最先发射出通知的 Observable，并且订阅该对象
        /// </summary>
        public static void SwitchingToTheFirstObservableToEmit()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(2)).Select(i => $"Server1-{i}");
            var o2 = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i => $"Server2-{i}");
            o.Amb(o2)
                .SubscribeConsole("Amb");
        }
    }
}
