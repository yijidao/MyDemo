using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.BasicQueryOperators;
using RxDotNetDemo.Controlling_the_observable_temperature;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Start...");

            //TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
            //{
            //    Console.WriteLine($"UnobservedTaskException： {eventArgs.Exception}");
            //};

            //StockDemo();
            //CreateDemo();
            //CreateFromDotNetAsynchronousType();
            //PeriodicCreate();
            //CreateObserverDemo();
            //ControlObservableAndObserver();
            //SubjectDemo();
            //HotAndColdDemo();
            BasicQueryOperatorsDemo();
            
            Console.WriteLine("Press Key...");
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

        /// <summary>
        /// 生成Observable的测试代码
        /// </summary>
        private static async void CreateDemo()
        {
            // ByCreate
            Console.WriteLine($"ByCreate Start... {DateTime.Now.ToLongTimeString()}");
            var byCreate = CreateOperate.GetObservableByCreate().Timestamp();
            await Task.Delay(TimeSpan.FromSeconds(2));
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

            // Observable 转 Enumerable
            var enumerable = CreateOperate.ObservableToEnumerable();
            foreach (var item in enumerable)
            {
                Console.WriteLine(item);
            }

            // ObservableToDictionary
            CreateOperate.ObservableToDictionary()
                .Select(x => string.Join(",", x))
                .SubscribeConsole("Enumerable 转 Dictionary");

            // ObservableToLookup
            CreateOperate.ObservableToLookup()
                .Select(lookup =>
                {
                    var groups = new StringBuilder();
                    foreach (var grouping in lookup)
                    {
                        groups.Append($"[Key => {grouping.Count()}]");
                    }

                    return groups.ToString();
                })
                .SubscribeConsole("Enumerable 转 Lookup");

            // 循环生成Observable
            CreateOperate.GetObservableByLoopWithGenerate().SubscribeConsole("Generate 循环");
            CreateOperate.GetObservableByLoopWithRange().SubscribeConsole("Range 循环");
            // Using
            CreateOperate.GetObservableByResource().SubscribeConsole("Using 读取流");

            // 生成简单的Observable
            CreateOperate.GetObservableByReturn().SubscribeConsole("Return");
            CreateOperate.GetObservableByThrow().SubscribeConsole("Throw");
            CreateOperate.GetObservableByNever().SubscribeConsole("Never");
            CreateOperate.GetObservableByEmpty().SubscribeConsole("Empty");
        }

        /// <summary>
        /// 从异步中生成Observable的测试代码
        /// </summary>
        private static void CreateFromDotNetAsynchronousType()
        {
            CreateFromAsynchronous.GeneratePrimeByImperative(5);
            CreateFromAsynchronous.GeneratePrime(5).Timestamp().SubscribeConsole("同步方法生成Observable");
            CreateFromAsynchronous.GeneratePrimeFromTask(5).SubscribeConsole("异步方法生成 Observable");
            CreateFromAsynchronous.SimpleGeneratePrimeFromTask(5).Timestamp().SubscribeConsole("简化版本异步方法生成 Observable");
            CreateFromAsynchronous.AsyncAwaitGenerate(5).Timestamp().SubscribeConsole("AsyncAwait 模式生成 Observable");
            CreateFromAsynchronous.TaskToObservable(5).Timestamp().SubscribeConsole("Task 生成 Observable");
            CreateFromAsynchronous.RunAsyncCodeInPipeline().Timestamp().SubscribeConsole("在执行链路中运行异步方法");
            CreateFromAsynchronous.RunAsyncCodeInPipelineByLinq().Timestamp().SubscribeConsole("在执行链路中运行异步方法的Linq实现");
            CreateFromAsynchronous.RunAsyncCodeInPipelineWithOrder().Timestamp().SubscribeConsole("在执行链路中运行异步方法，且结果有序");
        }

        /// <summary>
        /// 周期性的生成 Observable
        /// </summary>
        private static void PeriodicCreate()
        {
            CreateOfPeriodicBehavior.CreateByInterval().Timestamp().SubscribeConsole("Interval生成Observable");
            CreateOfPeriodicBehavior.CreateByTimer1().Timestamp().SubscribeConsole("ByTimer(dueTime, period)");
            CreateOfPeriodicBehavior.CreateByTimer2().Timestamp().SubscribeConsole("ByTimer(dueTime)");
            CreateOfPeriodicBehavior.CreateByTime3().Timestamp().SubscribeConsole("Timer(DateTimeOffset dueTime)");
        }

        /// <summary>
        /// 创建 Observer
        /// </summary>
        private static void CreateObserverDemo()
        {
            CreateObserver.GetObserverBySubscribe();
            CreateObserver.NoPassOnErrorAndAsync();
            CreateObserver.SubscribeWithCancellation();
            CreateObserver.GetObserverByCreate();
        }

        /// <summary>
        /// 控制 Observable 和 Observer 之间关系的操作符
        /// </summary>
        private static void ControlObservableAndObserver()
        {
            ControlOperate.DelayingSubscription();
            ControlOperate.StopEmitForAbsoluteTime();
            ControlOperate.StopEmitForAnotherObservable2();
            ControlOperate.SkinNotification();
            ControlOperate.StopByTakeWhile();
            ControlOperate.StartBySkinWhile();
            ControlOperate.ResubscribingByRepeat();
            ControlOperate.AddSideEffect();
        }

        /// <summary>
        /// subject 练习
        /// </summary>
        private static void SubjectDemo()
        {
            MulticasingWithSubjects.SimpleBroadcastingWithSubject();
            MulticasingWithSubjects.MultipleSource();
            MulticasingWithSubjects.ClassicMisuse();
            MulticasingWithSubjects.AsyncSubjectConvertTask();
            MulticasingWithSubjects.PreserveLastStateWithBehaviorSubject();
            MulticasingWithSubjects.CacheSequenceWthReplaySubject();
            MulticasingWithSubjects.ProtectSubject();
        }

        /// <summary>
        /// 通过subject,实现 Hot 和 Cold 的各种骚操作
        /// </summary>
        private static void HotAndColdDemo()
        {
            HeatingAndCoolingObservable.TurnColdObservableHot();
            HeatingAndCoolingObservable.ReusingThePublishObservable();
            HeatingAndCoolingObservable.ReusingThePublishObservable2();
            HeatingAndCoolingObservable.TurnColdObservableHotWithCacheValue();
            HeatingAndCoolingObservable.PublishLastDemo();
            HeatingAndCoolingObservable.ReconnectConnectableObservable();
            HeatingAndCoolingObservable.AutoDisconnectWithRefCount();
            HeatingAndCoolingObservable.HotObserverToCold();
        }

        /// <summary>
        /// Select、SelectMany、Where、Distinct、DistinctUntilChanged
        /// </summary>
        private static void BasicQueryOperatorsDemo()
        {
            //MappingOperator.Select();
            //MappingOperator.SelectWithIndex();
            //FlatteningOperator.FlatteningObservableOfEnumerable();
            //FlatteningOperator.FlatteningObservableOfEnumerableWithSource();
            //FlatteningOperator.FlatteningObservableOfObservable();
            //FlatteningOperator.FlatteningObservableOfObservableWithSource();
            //FilteringObservable.FilteringWithTheWhere();
            //FilteringObservable.CreatingDistinctSequence();
            FilteringObservable.RemovingDuplicateContiguousValues();

        }
    }
}
