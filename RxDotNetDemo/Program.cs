using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RxDotNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Start...");

            var stockTicker = new StockTicker();
            //var stockMonitor = new StockMonitor(stockTicker);
            //var rxStockMonitor = new RxStockMonitor(stockTicker);
            //ChangeStock("600000", 10, stockTicker);
            //ChangeStock("600000", 10, stockTicker);

            Console.ReadLine();
        }


        private static void ChangeStock(string symbol, decimal price, StockTicker stockTicker)
        {
            Task.Run(async () =>
            {
                var random = new Random();
                while (true)
                {
                    var radio = random.Next(-100, 100) / 1000m;
                    Debug.WriteLine($"Radio:{radio}");
                    stockTicker.StockChange(symbol, (decimal)(price + price * radio));
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            });
        }

    }
}
