using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RxDotNetDemo
{
    class CreateOperate
    {
        public CreateOperate()
        {
        }

        /// <summary>
        /// 调用 Observable.Create<T>() 创建Observable，这是最简单的创建Observable 的方式。
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> GetObservableByCreate()
        {
            return Observable.Create<int>(observer =>
            {
                for (int i = 0; i < 5; i++)
                {
                    observer.OnNext(i);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Observable.Defer() 可以创建Observable的代理，当Observable 被订阅时，才生成真正的Observable。
        /// 1. 当创建可能不会被立刻订阅的 Observable 时，可以使用此方法节省资源。
        /// 2. Defer() 接收一个委托来生成 Observable，所以很适合在不修改原有的工厂方法时，转换成 Observable 时使用。
        /// 3. Defer() 可以将 observable 从 hot 转换成 cold。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByDefer()
        {
            return Observable.Defer(() =>
            {
                var message = new[] { "Msg1", "Msg2", "Msg3" };
                return message.ToObservable();
            });
        }

        /// <summary>
        /// Observable.FromEventPattern<TDelegate, TEventArgs>() 是从标准的事件模式生成 Observable。
        /// 1. 标准事件模式是指 public event EventHandler MessageEvent 或者 public event EventHandler<T> MessageEvent 签名的。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<string> GetObservableForEventPattern(EventMock eventMock)
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>(h => eventMock.MessageEvent += h,
                h => eventMock.MessageEvent -= h).Select(change => "eventRaise");
        }

        /// <summary>
        /// 从标准事件模式生成 Observable 的最简化版本，但是这个方法需要传一个魔法数，所以一定要用 nameof 减少Bug。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<string> GetObservableForEventPatternSimplest(EventMock eventMock) =>
            Observable.FromEventPattern(eventMock, nameof(eventMock.MessageEvent)).Select(change => "eventRaiseSimplest");

        /// <summary>
        /// 从非标准事件模式生成 Observable。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<string> GetObservableForNotFollowEventPattern(EventMock eventMock) =>
            Observable.FromEvent<Action<string>, string>(h => eventMock.NotFollowEventPatternEvent += h,
                h => eventMock.NotFollowEventPatternEvent -= h);

        /// <summary>
        /// 多个参数的事件生成 Observable。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<Tuple<int, string>> GetObservableForMultipleParameters(EventMock eventMock)
        {
            return Observable.FromEvent<Action<int, string>, Tuple<int, string>>(
                    handler => (value1, value2) => handler(Tuple.Create(value1, value2)), // 传一个转换参数的委托
                h => eventMock.MultipleParameterEvent += h, h => eventMock.MultipleParameterEvent -= h);
        }

        /// <summary>
        /// 没有参数的事件生成 Observable
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<Unit> GetObservableForNotArgument(EventMock eventMock)
        {
            return Observable.FromEvent(h => eventMock.NotArgumentEvent += h, h => eventMock.NotArgumentEvent -= h);
        }

        public static IEnumerable<string> LengthArray { get; } = new[] { "1", "22", "333", "4444" };

        /// <summary>
        /// Enumerable 是 Pull 模式，会阻塞线程
        /// Observable 是 Push 模式，不会阻塞线程
        /// Enumerable 转 Observable，只需要调用ToObservable()，迭代结束后会调用 OnComplete()。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> EnumerableToObservable()
        {
            return LengthArray.ToObservable();
        }

        /// <summary>
        /// Enumerable 转 Observable，抛出异常版本。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> EnumerableToObservableWithException()
        {

            return lengthArrayWithException().ToObservable();

            IEnumerable<string> lengthArrayWithException()
            {
                yield return "1";
                yield return "22";
                yield return "333";
                throw new Exception("测试异常");
                yield return "4444";
            }
        }

        /// <summary>
        /// Enumerable 转 Observable 用途：常用于静态数据拼接动态数据，比如数据库数据拼接MQ消息等。
        /// 经常搭配 Concat 或 StartWith 一起使用。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> EnumerableToObservableWithConcat()
        {
            var observable = GetObservableByDefer();
            return LengthArray.ToObservable()
                .Concat(observable); // Concat 会顺序串联两个 observable

            //observable.StartWith(LengthArray); StartWith 也有可以实现 Concat 一样的功能
        }

        /// <summary>
        /// Observable转Enumerable，只需要调用 ToEnumerable()
        /// 会阻塞线程，而且会把 Pull 模式改成 Push 模式
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ObservableToEnumerable()
        {
            return GetObservableByDefer().ToEnumerable();
        }

        /// <summary>
        ///  Observable转Dictionary, 调用 ToDictionary()
        ///  1. 这个方法跟 ToEnumerable() 的不同是返回 IObservable 对象，所以这个方法不会阻塞线程，而是生成结束字典后，将整个字典发射出去
        ///  2. 如果存在一个Key有多个 Value 的情况，会抛异常，可以使用 ToLookup() 实现一个 Key 多个 Value。
        /// </summary>
        /// <returns></returns>
        public static IObservable<IDictionary<int, string>> ObservableToDictionary()
        {
            return EnumerableToObservable().ToDictionary(x => x.Length);
        }

        /// <summary>
        /// Observable转Lookup, 调用 ToLookup()，用于一个 Key 多个 Value 的情况。
        /// </summary>
        /// <returns></returns>
        public static IObservable<ILookup<int, string>> ObservableToLookup()
        {
            return GetObservableByDefer().ToLookup(x => x.Length);
        }

        /// <summary>
        /// Observable 实现 ForEach 和 While 等循环操作，复杂用 Generate() 实现，简单用 Range() 实现
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByLoopWithGenerate()
        {
            return Observable.Generate(1, i => i <= 5, i => i + 1, i =>
            {
                var builder = new StringBuilder();
                for (int j = 1; j <= i; j++)
                {
                    builder.Append(i.ToString());
                }
                return builder.ToString();
            });
        }

        /// <summary>
        /// Observable 实现 ForEach 和 While 等循环操作，复杂用 Generate() 实现，简单用 Range() 实现
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByLoopWithRange()
        {
            return Observable.Range(1, 5).Select(i =>
            {
                var builder = new StringBuilder();
                for (int j = 1; j <= i; j++)
                {
                    builder.Append(i.ToString());
                }
                return builder.ToString();
            });
        }

        /// <summary>
        /// 从txt文件读取流，涉及到释放调用 IDispose 方法的，可以使用 Using 操作符
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByResource()
        {
            return Observable.Using(() => File.OpenText("TextFile1.txt"),
                    stream =>
                Observable.Generate(stream, s => !s.EndOfStream, s => s, s => s.ReadLine()));
        }

        /// <summary>
        /// Return 会直接返回一个Observable
        /// Return、Never、Throw、Empty 会生成简单的 Observable,一般是用于单元测试情况下使用。
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByReturn()
        {
            return Observable.Return("CreateByReturn");
        }

        /// <summary>
        /// Never 会生成一个永远不会结束的 Observable
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByNever()
        {
            return Observable.Never<string>();
        }

        /// <summary>
        /// Throw 会生成一个抛出异常的 Observable
        /// </summary>
        /// <returns></returns>
        public static IObservable<Exception> GetObservableByThrow()
        {
            return Observable.Throw<Exception>(new Exception("测试异常"));
        }

        /// <summary>
        /// Empty 会生成一个空的 Observable
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> GetObservableByEmpty()
        {
            return Observable.Empty<string>();
        }
    }
}
