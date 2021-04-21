using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    class StockMonitor : IDisposable
    {
        private readonly StockTicker _stockTicker;

        public Dictionary<string, StockInfo> StockInfos { get; } = new Dictionary<string, StockInfo>();
        //public ConcurrentDictionary<string, StockInfo> StockInfos { get; } = new ConcurrentDictionary<string, StockInfo>();

        public decimal MaxChangeRadio => (decimal)0.1;

        public object StockLock { get; } = new object();

        public StockMonitor(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
            _stockTicker.StockTick += OnStockTick;
        }

        private void OnStockTick(object? sender, StockTick e)
        {
            var symbol = e.Symbol;
            var price = e.Price;
            lock (StockLock)
            {
                if (StockInfos.TryGetValue(symbol, out var stockInfo))
                {
                    var prevPrice = stockInfo.PrevPrice;
                    var changeRatio = Math.Round(Math.Abs(prevPrice - price) / prevPrice, 3);
                    if (changeRatio > MaxChangeRadio)
                    {
                        Console.WriteLine($"Stock:{symbol} has change with {changeRatio} ratio, Old Price:{prevPrice} New Price:{price}");
                    }

                    StockInfos[symbol].PrevPrice = price;
                }
                else
                {
                    StockInfos.Add(symbol, new StockInfo(symbol, price));
                    //StockInfos.TryAdd(symbol, new StockInfo(symbol, price));
                }
            }

        }

        public void Dispose()
        {
            _stockTicker.StockTick -= OnStockTick;
        }
    }
}
