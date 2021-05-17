using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.ErrorHandlingAndRecovery
{
    /// <summary>
    /// Rx 中的异常就是 Error,Rx 中的 Error 发生的位置分为四种：
    /// 1. 在 Observable 的 Subscribe 中发生的。
    /// 2. 在 Observable 的源中发生的，例如 IEnumerable.ToObservable 的 IEnumerable 中发生的。
    /// 3. 在操作符中发生的，比如在 Select 中发生。
    /// 4. 在 Observer 中的 OnNext、OnError、OnComplete 中发生的。
    /// 前三种可以在 OnError 中做处理，第四种只能手动 try catch
    /// </summary>
    public class ReactingToErrors
    {
        /// <summary>
        /// Observable 引发的错误都会在 OnError 中，所以可以在 OnError 中做一些处理
        /// 这个Demo 模拟了一个执行大量计算的 Observable，所以可能会有内存溢出，这里当内存溢出的时候，做一下 GC
        /// </summary>
        public static void ErrorsFromTheObservableSide()
        {
            var o = Observable.Throw<string>(new OutOfMemoryException());
            o.Subscribe(_ => { },
                ex =>
                {
                    if (ex is OutOfMemoryException)
                    {
                        Console.WriteLine($"OutOfMemoryException");
                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                });
        }

        /// <summary>
        /// 在常规的命令式代码中，会串联多个 catch 进行异常处理，rx 也提供了 catch 操作符做到类似操作
        /// catch 可以捕获异常并且返回一个回退 observable
        /// catch 如果只传参 observable，则会捕获所有异常
        /// </summary>
        public static void CatchingErrors()
        {
            var o = Observable.Throw<string>(new OutOfMemoryException());
            o
                .Catch((OutOfMemoryException ex) =>
            {
                Console.Write("handling OMM exception");
                return Observable.Empty<string>();
            })
                .Catch((ArgumentException ex) =>
            {
                Console.Write("handling AE exception");
                return Observable.Empty<string>();
            })
            .Catch(Observable.Empty<string>())
                .SubscribeConsole("Catch (source throws)");
        }

        /// <summary>
        /// OnErrorResume 是 Catch 和 Concat 的组合体
        /// 1. 不管有没有抛异常，OnErrorResume 中的 Observable 依旧可以发射通知，此时类似于 concat
        /// 2. 如果OnErrorResume 之前的 Observable 出现了异常，那么OnErrorResume 则进行发射通知，此时类似于 catch
        /// </summary>
        public static void VariableCatch()
        {
            var o = Observable.Throw<string>(new OutOfMemoryException());
            var o2 = Observable.Return<string>("OK");

            o.OnErrorResumeNext(o2)
                .SubscribeConsole("OnErrorResumeNext (source throws)");

            o2.OnErrorResumeNext(o2)
                .SubscribeConsole("OnErrorResumeNext (source completed)");
        }

        /// <summary>
        /// 在 rx 的设计中，如果 error 发生，那么observable 和 observer 的连接就会断开， retry 可以在异常发生时，重新订阅 Observable
        /// retry 适合一些瞬态异常发生的情况，如连接因为网络波动断开，这时候 retry 进行重连，就很符合这种场景
        /// </summary>
        public static void RetryingToSubscribe()
        {
            var o = Observable.Throw<string>(new OutOfMemoryException());
            o.Log() // log 会打印出三条异常错误日志，这是因为 Retry(3)，所以可以执行三次
                .Retry(3)
                .SubscribeConsole("Retry");
        }
    }
}
