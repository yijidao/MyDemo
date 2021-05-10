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
        /// 周期性地生成 Observable
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> CreateByInterval()
        {
            return Observable.Interval(TimeSpan.FromSeconds(2))
                .SelectMany(_ => PrimeGenerator.GenerateAsync(3))
                .SelectMany(primes => primes);
        }


    }
}
