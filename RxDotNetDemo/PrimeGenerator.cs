using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxDotNetDemo
{
    /// <summary>
    /// 测试用质数生成器
    /// </summary>
    public class PrimeGenerator
    {
        private static int[] Primes { get; set; } = GeneratePrime(20);

        /// <summary>
        /// 穷举法生成质数
        /// </summary>
        /// <param name="amout"></param>
        /// <returns></returns>
        private static int[] GeneratePrime(int amout)
        {
            var current = 2;
            var set = new HashSet<int>();
            while (set.Count < amout)
            {
                if (CheckPrime(current))
                {
                    set.Add(current);
                }

                current++;
            }
            return set.ToArray();
        }

        /// <summary>
        /// 同步生成质数,这里加了延时,模拟计算
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static IEnumerable<int> Generate(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return GetPrime(i);
            }
        }

        /// <summary>
        /// 异步生成质数，这里只是为了测试用
        /// </summary>
        /// <param name="amout"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<int>> GenerateAsync(int amout)
        {
            return await Task.Run(() => Generate(amout).ToList().AsReadOnly());
        }

        /// <summary>
        /// 模拟计算质数的过程
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int GetPrime(int index)
        {
            Task.Delay(1000).Wait();
            return index < Primes.Length ? Primes[index] : Primes.Last();
        }

        public static bool CheckPrime(long value)
        {
            var isPrime = true;
            if (value < 2)
            {
                return false;
            }

            for (int i = 2; i < value; i++)
            {
                if (value % i == 0) // 不是质数
                {
                    isPrime = false;
                    break;
                }
            }

            return isPrime;
        }

        public static async Task<bool> CheckPrimeAsync(int value)
        {
            await Task.Delay(1000);
            return CheckPrime(value);
        }
    }
}
