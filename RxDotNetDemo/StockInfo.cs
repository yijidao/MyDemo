using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    class StockInfo
    {
        public string Symbol { get; set; }

        public decimal PrevPrice { get; set; }

        public StockInfo(string symbol, decimal prevPrice)
        {
            Symbol = symbol;
            PrevPrice = prevPrice;
        }
    }
}
