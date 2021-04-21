using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    class StockTick
    {
        public string Symbol { get; set; }

        public decimal Price { get; set; }

        public StockTick(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
        }
    }
}
