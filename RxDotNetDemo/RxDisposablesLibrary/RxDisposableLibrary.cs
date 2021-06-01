using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RxDotNetDemo.RxDisposablesLibrary
{
    /// <summary>
    /// Rx 提供了一些实现了 IDisposable 的类，可以直接使用，而不需要手动实现 IDisposable 接口，很方便。
    /// Disposable.Create:这个最常用
    /// Disposable.Empty
    /// ContextDisposable：这个也常用
    /// ScheduledDisposable
    /// MultipleAssignmentDisposable
    /// RefCountDisposable
    /// CompositeDisposable
    /// CancellationDisposable
    /// BooleanDisposable
    /// </summary>
    public static class RxDisposableLibrary
    {
        /// <summary>
        /// Disposable.Create 是最灵活的一个方法，传入的委托会在 Dispose 中执行
        /// 这个 Demo 使用 using 和 Disposable.Create 来实现界面加载是弹出加载动画的效果
        /// </summary>
        /// <returns></returns>
        public static async Task Create()
        {
            var isBusy = true;
            using (Disposable.Create(() => isBusy = false))
            {
                await Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(1)));
            }
        }

        /// <summary>
        /// Disposable.Empty 就是直接 Dispose 中什么都没实现，主要用于 Observable.Create 或者用处初始化 IDisposable 类型的变量
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> Empty()
        {
            return Observable.Create<string>(o =>
            {
                o.OnNext("1");
                o.OnNext("2");
                o.OnCompleted();
                return Disposable.Empty;
            });
        }


        /// <summary>
        /// ContextDisposable 主要用于指定 Dispose 执行的上下文，主要用于 UI 刷新等场景
        /// </summary>
        /// <returns></returns>
        public static IDisposable ContextDisposable()
        {
            var isBusy = true;
            return new ContextDisposable(SynchronizationContext.Current, Disposable.Create(() => isBusy = false));
        }

        /// <summary>
        /// ScheduledDisposable 与 ContextDisposable 类似，只是 ScheduledDisposable 指定的是 Scheduler
        /// 在使用 SubscribeOn 的时候，就使用了 ScheduledDisposable
        /// </summary>
        /// <returns></returns>
        public static IDisposable ScheduledDisposable()
        {
            var isBusy = true;
            return new ScheduledDisposable(Scheduler.CurrentThread, Disposable.Create(() => isBusy = false));
        }

        /// <summary>
        /// SerialDisposable 返回一个可以重复执行的 Disposable，当使用新的 替换掉 Disposable 的时候，被替换掉的 Disposable 会被回收
        /// Subscribe 就使用了 SerialDisposable，主要使用场景还是自定义 Rx 操作符吧。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static IObservable<TSource> MySubscribeOn<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            return Observable.Create<TSource>(observer =>
            {
                var d = new SerialDisposable();
                d.Disposable = scheduler.Schedule(() =>
                {
                    d.Disposable = new ScheduledDisposable(scheduler, source.SubscribeSafe(observer));
                });
                return d;
            });
        }

        /// <summary>
        /// RefCountDisposable 可以使用 GetDisposable 来生成新的 Disposable，并且只会在生成的 Disposable 全部调用完 Dispose 之后，才会执行自己的 Dispose
        /// </summary>
        public static void RefCountDisposable()
        {
            var refCount = new RefCountDisposable(Disposable.Create(() => Console.WriteLine("Disposing refCount")));
            var d1 = refCount.GetDisposable();
            var d2 = refCount.GetDisposable();

            refCount.Dispose();
            Console.WriteLine("Disposing 1st");
            d1.Dispose();
            Console.WriteLine("Disposing 2nd");
            d2.Dispose();
        }

        /// <summary>
        /// 跟 SerialDisposable 类似，可以重复设置的 Disposable，但是不会自动 Dispose 上一个 IDisposable，感觉是一个比较底层的方法，估计用不上。
        /// </summary>
        public static void MultipleAssignmentDisposable()
        {
            //new MultipleAssignmentDisposable()
        }

        /// <summary>
        /// 只能设置一次的 IDisposable，如果已经被设置了，则会抛异常
        /// </summary>
        public static void SingleAssignmentDisposable()
        {
            //new SingleAssignmentDisposable()
        }

        /// <summary>
        /// CompositeDisposable 可以添加多个 IDisposable，当调用 Dispose时，会同时调用内部 IDisposable 的 Dispose
        /// 这个经常用于对 Disposable 进行分组，然后按组调用 Dispose
        /// </summary>
        public static void CompositeDisposable()
        {
            var compositeDisposable = new CompositeDisposable(
                Disposable.Create(() => Console.WriteLine($"1st disposed")),
                Disposable.Create(() => Console.WriteLine($"2nd disposed")));

            compositeDisposable.Dispose();
        }

        /// <summary>
        /// CancellationDisposable 是 IDisposable 和 CancellationTokenSource 之间的适配器层
        /// 当调用 CancellationDisposable.Dispose() 时，CancellationTokenSource 会被 canceled
        /// </summary>
        public static void CancellationDisposable()
        {
            var cancelable = new CancellationDisposable();
            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.WriteLine("Inner");
            }, cancelable.Token);
            cancelable.Dispose();
        }

        /// <summary>
        /// BooleanDisposable 只是封装了一下布尔值，调用完 Dispose，IsDisposed 会为 True
        /// </summary>
        public static void BoolDisposable()
        {
            var boolDisposable = new BooleanDisposable();
            Console.WriteLine($"Before, IsDisposed = {boolDisposable.IsDisposed}");
            boolDisposable.Dispose();
            Console.WriteLine($"After, IsDisposed = {boolDisposable.IsDisposed}");
        }
    }
}
