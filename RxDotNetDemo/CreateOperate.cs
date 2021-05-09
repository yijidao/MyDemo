using System;
using System.Collections.Generic;
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

        public static IEnumerable<string> LengthArray { get;  } = new[] { "1", "22", "333", "444" };

        /// <summary>
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



    }
}
