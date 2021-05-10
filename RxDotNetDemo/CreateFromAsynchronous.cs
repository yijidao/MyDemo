using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
        /// 异步方法生成Observable，不阻塞线程，可取消
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
        /// 简化版的异步方法生成Observable，不阻塞线程，可取消
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

        /// <summary>
        /// AsyncAwait 模式生成Observable
        /// 执行时间等于所有task总和
        /// </summary>
        /// <param name="amout"></param>
        /// <returns></returns>
        public static IObservable<int> AsyncAwaitGenerate(int amout)
        {
            return Observable.Create<int>(async o =>
            {
                var primes1 = await PrimeGenerator.GenerateAsync(5);
                foreach (var prime in primes1)
                {
                    o.OnNext(prime);
                }

                var primes2 = await PrimeGenerator.GenerateAsync(5);
                foreach (var prime in primes2)
                {
                    o.OnNext(prime);
                }
                o.OnCompleted();
            });
        }

        /// <summary>
        /// Task 生成 Observable，使用 ToObservable。
        /// 这个写法很简单，而且执行时间是耗时最长的 Task 时间，跟 AsyncAwaitGenerate 不一样
        /// </summary>
        /// <param name="amout"></param>
        /// <returns></returns>
        public static IObservable<int> TaskToObservable(int amout)
        {
            var o1 = PrimeGenerator.GenerateAsync(5).ToObservable();
            var o2 = PrimeGenerator.GenerateAsync(5).ToObservable();
            return o1.Concat(o2)  // 先用 Concat 连接两个集合
                .SelectMany(primes => primes); // 再用 SelectMany 投影每个值
        }

        /// <summary>
        /// 在执行链路中运行异步方法
        /// 比如想在 Where() 中运行异步判断，但是 Where 不支持返回值 Task<bool>，所以可以使用 SelectMany 的重载方法来进行投影，并生成新的带原始数据的对象
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> RunAsyncCodeInPipeline()
        {
            return Observable.Range(2, 9)
                .SelectMany(x => PrimeGenerator.CheckPrimeAsync(x), (number, isPrime) => new { number, isPrime })
                .Where(x => x.isPrime)
                .Select(x => x.number);
        }

        /// <summary>
        /// RunAsyncCodeInPipeline 的 linq 实现，更直观
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> RunAsyncCodeInPipelineByLinq()
        {
            var resule = from number in Observable.Range(2, 9)
                         from isPrime in PrimeGenerator.CheckPrimeAsync(number)
                         where isPrime
                         select number;
            return resule;
        }

        /// <summary>
        /// 在链路中中运行异步方法，而且结果顺序输出
        /// 使用 Select 和 Concat  代替 SelectMany 就可以顺序输出
        /// </summary>
        /// <returns></returns>
        public static IObservable<int> RunAsyncCodeInPipelineWithOrder()
        {
            return Observable.Range(2, 9)
                .Select(async x => new {number = x, isPrime = await PrimeGenerator.CheckPrimeAsync(x)})
                .Concat()
                .Where(x => x.isPrime)
                .Select(x => x.number);
        }


    }
}
