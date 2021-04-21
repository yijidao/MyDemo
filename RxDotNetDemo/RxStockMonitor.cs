using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace RxDotNetDemo
{
    class RxStockMonitor : IDisposable
    {
        public decimal MaxChangeRadio => 0.1m;

        private readonly IDisposable _subscribe;

        public RxStockMonitor(StockTicker ticker)
        {
            var ticks = Observable.FromEventPattern<EventHandler<StockTick>, StockTick>(
                    h => ticker.StockTick += h,
                    h => ticker.StockTick -= h)
                .Select(x => x.EventArgs)
                .Synchronize();

            var drasticChange = from tick in ticks
                                group tick by tick.Symbol
                                into company
                                from tickPair in company.Buffer(2, 1)
                                let changeRatio = Math.Abs((tickPair[1].Price - tickPair[0].Price) / tickPair[0].Price)
                                where changeRatio > MaxChangeRadio
                                select new DrasticChange(company.Key, changeRatio, tickPair[0].Price, tickPair[1].Price);


            _subscribe = drasticChange.Subscribe(change =>
            {
                Console.WriteLine(
                    $"Stock:{change.Symbol} has changed with {change.ChangeRadio} radio, Old Price:{change.OldPrice} New Price:{change.NewPrice}");
            }, ex =>
                {

                });

        }

        public void Dispose()
        {
            _subscribe.Dispose();
        }
    }

    class DrasticChange
    {


        public string Symbol { get; set; }

        public decimal ChangeRadio { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public DrasticChange(string symbol, decimal changeRadio, decimal oldPrice, decimal newPrice)
        {
            Symbol = symbol;
            ChangeRadio = changeRadio;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
}
