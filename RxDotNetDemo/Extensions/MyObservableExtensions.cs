using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo.Extensions
{
    public static class MyObservableExtensions
    {
        public static IDisposable SubscribeConsole<T>(this IObservable<T> observable, string name = "") =>
            observable.Subscribe(new ConsoleObserver<T>(name));

    }
}
