using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace RxDotNetDemo
{
    /// <summary>
    /// 周期性地生成 Observable
    /// </summary>
    public static class CreateOfPeriodicBehavior
    {
        /// <summary>
        /// 周期性地生成 Observable，均匀时间间隔
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> CreateByInterval()
        {
            return Observable.Interval( TimeSpan.FromSeconds(2))
                .SelectMany(_ => PrimeGenerator.GenerateAsync(3))
                .SelectMany(primes => primes);
        }

        /// <summary>
        /// Timer 相对时间传参，这是传启动时间和间隔时间参数的
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> CreateByTimer1()
        {
            return  Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
                .Select(_ => "Timer(dueTime, period)");
        }

        /// <summary>
        /// Timer 相对时间传参，这是只传启动时间的
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> CreateByTimer2()
        {
            return Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "Timer(dueTime)");
        }

        /// <summary>
        /// Timer 绝对时间传参，用来在某个时刻定时启动
        /// </summary>
        /// <returns></returns>
        public static IObservable<string> CreateByTime3()
        {
            //return Observable.Timer(DateTimeOffset.Now.AddSeconds(5)).Select(_ => "Timer(DateTimeOffset dueTime)");
            return Observable.Timer(DateTimeOffset.Parse("10:05:00")).Select(_ => "Timer(DateTimeOffset dueTime)");
        }
    }
}
