using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.BasicQueryOperators
{
    /// <summary>
    /// flatten 是函数式的一种操作,在 rx.net 中实现为 SelectMany
    /// 可以在 nuget 中引入方言包 System.Reactive.Observable.Aliases 来调用 flatten
    /// </summary>
    public static class FlatteningOperator
    {
        /// <summary>
        /// 展平通知是数组的 Observable
        /// </summary>
        public static void FlatteningObservableOfEnumerable()
        {
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5))
                .Select(x => GetNumbers(x * 10))
                .SelectMany(x => x)
                .Where(PrimeGenerator.CheckPrime)
                .SubscribeConsole();
        }

        /// <summary>
        /// 展平通知是数组的 Observable
        /// selectMany 重载,可以传一个两个参数的委托,两个参数为 源 和 当前值
        /// </summary>
        public static void FlatteningObservableOfEnumerableWithSource()
        {
            // 也可以不用 lambda 实现,而是直接 linq实现
            // 这个demo有点不贴切,不过大概这个理
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5))
                .Select(x => GetNumbers(x * 10))
                .SelectMany(x => x, (items, i) => new { index = Array.IndexOf(items, i), value = i })
                .SubscribeConsole();
        }

        /// <summary>
        /// 展平通知是 Observable 的 Observable
        /// </summary>
        public static void FlatteningObservableOfObservable()
        {
            var o = Observable.Create<ChatRoom>(o =>
            {
                o.OnNext(new ChatRoom("Y",3));
                Task.Delay(10000).Wait();
                o.OnNext(new ChatRoom("X"));
                return Disposable.Empty;
            });

            o.Log("Room")
                .SelectMany(r => r.Messages)
                .Timestamp()
                .SubscribeConsole();
        }

        /// <summary>
        /// 展平通知是 Observable 的 Observable
        /// selectMany 重载,可以传一个两个参数的委托,两个参数为 源 和 当前值
        /// </summary>
        public static void FlatteningObservableOfObservableWithSource()
        {
            var o = Observable.Create<ChatRoom>(o =>
            {
                o.OnNext(new ChatRoom("Y", 3));
                Task.Delay(10000).Wait();
                o.OnNext(new ChatRoom("X"));
                return Disposable.Empty;
            });

            o.Log("Room")
                .SelectMany(r => r.Messages, (room, s) => $"name:{room.Name}  value:{s}")
                .Timestamp().SubscribeConsole();

        }

        public static long[] GetNumbers(long start, int count = 10)
        {
            var list = new long[count];
            for (int i = 0; i < count; i++)
            {
                list[i] = start++;
            }
            return list;
        }

    }

}
