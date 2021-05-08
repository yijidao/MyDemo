using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
                var message = new [] {"Msg1", "Msg2", "Msg3"};
                return message.ToObservable();
            });
        }

        /// <summary>
        /// Observable.FromEventPattern<TDelegate, TEventArgs>() 是从标准的事件模式生成 Observable。
        /// 1. 标准事件模式是指 public event EventHandler MessageEvent 或者 public event EventHandler<T> MessageEvent 签名的。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<string> GetObservableByEventPattern(EventMock eventMock)
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>(h => eventMock.MessageEvent += h,
                h => eventMock.MessageEvent -= h).Select(change => "eventRaise");
        }

        /// <summary>
        /// 从标准事件模式生成 Observable 的最简化版本，但是这个方法需要传一个魔法数，所以一定要用 nameof 减少Bug。
        /// </summary>
        /// <param name="eventMock"></param>
        /// <returns></returns>
        public static IObservable<string> GetObservableByEventPatternSimplest(EventMock eventMock) =>
            Observable.FromEventPattern(eventMock, nameof(eventMock.MessageEvent)).Select(change => "eventRaiseSimplest");
    }
}
