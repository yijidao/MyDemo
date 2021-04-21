using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    class StockTicker
    {
        public event EventHandler<StockTick> StockTick;

        public void StockChange(string symbol, decimal price)
        {
            StockTick?.Invoke(this, new StockTick(symbol, price));
        }

    }
}
