using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxDotNetDemo
{
    /// <summary>
    /// 创建 Observer 的 Demo
    /// </summary>
    public class CreateObserver
    {
        /// <summary>
        /// 通过 Subscribe 创建 Observer 是最常见的
        /// Subscribe 有三个委托参数，OnError 应该都实现一下，这样可以减少不少 bug
        /// Subscribe 返回值是一个 IDispose 对象
        /// </summary>
        public static void GetObserverBySubscribe()
        {
            var name = nameof(GetObserverBySubscribe);
            Observable.Range(1, 10)
                .Select(value => value / (value - 3))
                .Subscribe(value => OnNext(name, value)); // 如果不传 OnError，则会直接抛异常

            //Observable.Range(1, 10)
            //    .Select(value => value / (value - 3))
            //    .Subscribe(value => OnNext(name, value)); // 如果传 OnError，不会直接抛异常，而是打印日志
        }

        /// <summary>
        /// 如果没有给 OnError 传值，并且执行了异步代码，就算异步代码抛异常了，也无法捕获到异常，而是直接结束
        /// </summary>
        public static void NoPassOnErrorAndAsync()
        {
            var name = nameof(NoPassOnErrorAndAsync);
            Observable.Range(1, 10)
                .Select(value => Task.Run(() => value / (value - 3)))
                .Concat()
                .Subscribe(value => OnNext(name, value)); // 如果不传 OnError，既不会抛异常，也不会打印错误信息，而是直接结束

            // 如果传了 OnError，则会打印错误信息
            //Observable.Range(1, 10)
            //    .Select(value => Task.Run(() => value / (value - 3)))
            //    .Concat()
            //    .Subscribe(value => OnNext(name, value), ex => OnError(name, ex)); 
        }

        /// <summary>
        /// 通过CancellationTokenSource实现带取消功能的 subscribe
        /// 只是取消，不是完成，所以不会执行 OnComplete()
        /// </summary>
        public static void SubscribeWithCancellation()
        {
            var name = nameof(SubscribeWithCancellation);
            var cts = new CancellationTokenSource();;
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(value => OnNext(name, value), ex => OnError(name, ex), () => OnCompleted(name), cts.Token);
            cts.CancelAfter(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// 通过 Create<T>() 生成 Observer，常用于共享 observer 的业务场景下
        /// </summary>
        public static void GetObserverByCreate()
        {
            var name = nameof(GetObserverByCreate);
            var o = Observer.Create<string>(value => OnNext(name, value));
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(value => $"X{value}")
                .Subscribe(o);
            Observable.Interval(TimeSpan.FromSeconds(2))
                .Select(value => $"YY{value}")
                .Subscribe(o);
        }


        private static void OnNext<T>(string name, T value)
        {
            Console.WriteLine($"{name} - OnNext({value})");
        }

        private static void OnError(string name, Exception ex)
        {
            Console.WriteLine($"{name} - OnError:");
            Console.WriteLine(ex);
        }

        private static void OnCompleted(string name)
        {
            Console.WriteLine($"{name} - OnComplete");
        }
    }
}
