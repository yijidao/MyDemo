﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Start...");

            //StockDemo();
            CreateDemo();


            CreateOperate.GetObservableByCreate();


            Console.ReadLine();
        }

        private static void StockDemo()
        {
            var stockTicker = new StockTicker();
            var stockMonitor = new StockMonitor(stockTicker);
            var rxStockMonitor = new RxStockMonitor(stockTicker);
            ChangeStock("600000", 10, stockTicker);
            ChangeStock("600000", 10, stockTicker);
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

        private static void CreateDemo()
        {
            // ByCreate
            var byCreate = CreateOperate.GetObservableByCreate();
            byCreate.SubscribeConsole("ByCreate");

            // ByDefer
            var byDefer = CreateOperate.GetObservableByDefer();
            byDefer.SubscribeConsole("ByDefer");

            // 事件生成
            var eventMock = new EventMock();
            CreateOperate.GetObservableForEventPattern(eventMock).SubscribeConsole("ForEventPattern");
            CreateOperate.GetObservableForEventPatternSimplest(eventMock).SubscribeConsole("ForEventPatternSimplest");
            CreateOperate.GetObservableForNotFollowEventPattern(eventMock).SubscribeConsole("ForNotFollowEventPattern");
            CreateOperate.GetObservableForMultipleParameters(eventMock).SubscribeConsole("ForMultipleParameters");
            CreateOperate.GetObservableForNotArgument(eventMock).SubscribeConsole("ForNotArgument");
            eventMock.RaiseEvent();
            eventMock.RaiseEvent();
            eventMock.RaiseEvent();

            // Enumerable 转 Observable
            CreateOperate.EnumerableToObservable().SubscribeConsole("Enumerable 转 Observable");
            CreateOperate.EnumerableToObservableWithException().SubscribeConsole("抛异常的Enumerable 转 Observable");
            CreateOperate.EnumerableToObservableWithConcat()
                .SubscribeConsole("Enumerable 转 Observable 后使用 Concat() 拼接多个 Observable");
        }

    }
}
