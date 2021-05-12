using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.BasicQueryOperators
{
    public class BasicAggregationOperator
    {
        /// <summary>
        /// 常见的聚合操作
        /// </summary>
        public static void BasicAggregationOperatorDemo()
        {
            var o = Observable.Range(1, 5);
            o.SubscribeConsole("Range");
            o.Sum().SubscribeConsole("Sum");
            o.Count().SubscribeConsole("Count");
            o.Average().SubscribeConsole("Average");
            o.Max().SubscribeConsole("Max");
            o.Min().SubscribeConsole("Min");
        }

        /// <summary>
        /// MaxBy 和 MinBy，可以根据属性，筛选出对应的消息序列
        /// </summary>
        public static void FindingMaximumAndMinimumItems()
        {
            var o = Observable.Create<string>(observer =>
            {
                observer.OnNext("aaaaaa");
                observer.OnNext("aaa");

                observer.OnNext("aaaa");
                observer.OnNext("aaaaa");
                observer.OnNext("aaa");
                observer.OnNext("aaaaaa");

                observer.OnCompleted();
                return Disposable.Empty;
            });

            o.MaxBy(x => x.Length)
                .SelectMany(x => x)
                .SubscribeConsole("MaxBy");
            o.MinBy(x => x.Length)
                .SelectMany(x => x)
                .SubscribeConsole("MinBy");
        }

        /// <summary>
        /// Aggregate 是在接收完 Observable 所有通知，并且 Observable OnComplete 之后，对所有通知进行计算，并发射一个通知
        /// Scan 是序列地接收 Observable 的通知，并且立刻进行计算，然后序列地发射出计算后的通知
        /// </summary>
        public static void ScanAndAggregateDemo()
        {
            var o = Observable.Range(1, 5);
            o.Aggregate((a, b) => a + b).SubscribeConsole("Aggregate");
            o.Aggregate(1, (a, b) => a + b).SubscribeConsole("AggregateWithSeed"); // 带初始值的 Aggregate 函数
            o.Scan((a, b) => a + b).SubscribeConsole("Scan");
            o.Scan(1, (a, b) => a + b).SubscribeConsole("ScanWithSeed"); // 带初始值的 Scan 函数
        }

        /// <summary>
        /// 找到一组序列中的第二大值
        /// 一般实现这个场景是使用变量缓存，然后利用闭包给rx使用
        /// 使用变量会引入复杂性，不符合无状态原则，所以这里提供一种利用 Aggregate 方法来实现这个需求的思路
        /// </summary>
        public static void SecondLargestItem()
        {
            var o = Observable.Range(1, 5);
            o.Aggregate(new SortedSet<int>(), (largest, i) =>
                {
                    largest.Add(i);
                    if (largest.Count > 2)
                    {
                        largest.Remove(largest.FirstOrDefault());
                    }

                    return largest;
                }, largest => largest.FirstOrDefault())
                .SubscribeConsole("SecondLargest");
        }

    }
}
