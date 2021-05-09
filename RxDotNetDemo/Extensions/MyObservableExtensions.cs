using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.Extensions
{
    public static class MyObservableExtensions
    {
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "", Action<T> onNext = null) =>
            observable.Subscribe(new ConsoleObserver<T>(name));

    }
}
