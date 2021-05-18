using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using RxDotNetDemo.ErrorHandlingAndRecovery;

namespace RxDotNetDemo.Extensions
{
    public static class MyObservableExtensions
    {
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "", Action<T> onNext = null) =>
            observable.Subscribe(new ConsoleObserver<T>(name));

        public static IObservable<T> Log<T>(this IObservable<T> observable, string msg = "")
        {
            return observable.Do(x =>
                {
                    Console.WriteLine($"{msg} - OnNext({x})");
                },
                ex =>
                {
                    Console.WriteLine($"{msg} - OnError");
                    Console.WriteLine($"\t {ex}");
                },
                () =>
                {
                    Console.WriteLine($"{msg} - OnCompleted()");
                });
        }

        public static IObservable<T> LogWithThread<T>(this IObservable<T> observable, string msg = "")
        {
            return Observable.Defer(() =>
            {
                Console.WriteLine($"{msg} Subscription happened on Thread:{Thread.CurrentThread.ManagedThreadId}");

                return observable.Do(x =>
                        Console.WriteLine($"{msg} - OnNext({x}) Thread: {Thread.CurrentThread.ManagedThreadId}"),
                    ex =>
                    {
                        Console.WriteLine($"{msg} - OnError Thread: {Thread.CurrentThread.ManagedThreadId}");
                        Console.WriteLine($"\t {ex}");
                    },
                    () => Console.WriteLine($"{msg} - OnCompleted() Thread {Thread.CurrentThread.ManagedThreadId}"));
            });
        }

        public static IObservable<T> AsWeakObservable<T>(this IObservable<T> source)
        {
            return Observable.Create<T>(o =>
            {
                var weakObserverProxy = new WeakObserverProxy<T>(o);
                var subscription = source.Subscribe(weakObserverProxy);
                weakObserverProxy.SetSubscription(subscription);
                return weakObserverProxy.AsDisposable();
            });
        }
    }
}
