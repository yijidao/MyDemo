using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = System.Console;

namespace RxDotNetDemo
{
    public class CreateFromAsynchronous
    {
        /// <summary>
        /// 命令式生成质数
        /// </summary>
        /// <param name="amount"></param>
        public static void GeneratePrimeByImperative(int amount)
        {
            foreach (var prime in PrimeGenerator.Generate(amount))
            {
                Console.WriteLine($"{prime}@{DateTime.Now}");
            }
        }

        /// <summary>
        /// 同步方法生成Observable，阻塞线程
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static IObservable<int> GeneratePrime(int amount)
        {
            return Observable.Create<int>(o =>
            {
                foreach (var prime in PrimeGenerator.Generate(amount))
                {
                    o.OnNext(prime);
                }
                o.OnCompleted();
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// 异步方法生成Observable，不阻塞线程
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static IObservable<int> GeneratePrimeFromTask(int amount)
        {
            return Observable.Create<int>(o =>
            {
                var cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    foreach (var prime in PrimeGenerator.Generate(amount))
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        o.OnNext(prime);
                    }

                    o.OnCompleted();
                }, cts.Token);
                return new CancellationDisposable(cts);
            });
        }

        /// <summary>
        /// 简化版的异步方法生成Observable，不阻塞线程
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static IObservable<int> SimpleGeneratePrimeFromTask(int amount)
        {
            return Observable.Create<int>((o, ct) =>
            {
                return Task.Run(() =>
                {
                    foreach (var prime in PrimeGenerator.Generate(amount))
                    {
                        ct.ThrowIfCancellationRequested();
                        o.OnNext(prime);
                    }
                    o.OnCompleted();
                }, ct);
            });
        }
    }
}
