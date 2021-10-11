using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CacheDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test();
            Console.ReadLine();
        }


        public static async Task Test()
        {
            var demo = new MemoryCacheDemo();

            for (int i = 1; i < 11; i++)
            {
                var value = await demo.Get(CacheKeys.Entry);
                Log("Cache", value);
                await Task.Delay(TimeSpan.FromSeconds(1));
                if (i % 3 == 0 && demo.Cache.TryGetValue(CacheKeys.Cts, out CancellationTokenSource cts))
                {
                    cts.Cancel();
                }
            }
        }

        public static void Log<T>(string tag, T msg) => Console.WriteLine($"[{tag}]  {msg}");


    }

    class MemoryCacheDemo
    {
        public IMemoryCache Cache { get; }

        public MemoryCacheDemo()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<DateTime> Get(string key)
        {
            return await Cache.GetOrCreateAsync<DateTime>(key, entry =>
            {
                //entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                //entry.AbsoluteExpiration = DateTimeOffset.Now.Add(TimeSpan.FromSeconds(3));
                //entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3);

                var cts= new CancellationTokenSource();
                Cache.Set(CacheKeys.Cts, cts);

                entry.ExpirationTokens.Add(new CancellationChangeToken(cts.Token));

                entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
                {
                    EvictionCallback = (o, value, reason, state) =>
                    {
                        Program.Log("EvictionCallback", $"[reason]  {reason}  [state]  {state}");
                    }
                });
                
                return Task.FromResult<DateTime>(DateTime.Now);
            });
        }

    }

    public static class CacheKeys
    {
        public static string Entry => "_Entry";

        public static string Cts => "_CancelTokenSource";
    }
}
