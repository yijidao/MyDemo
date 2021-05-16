using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RxDotNetDemo.BasicQueryOperators;
using RxDotNetDemo.Controlling_the_observable_temperature;
using RxDotNetDemo.Extensions;
using RxDotNetDemo.PartitioningAndCombiningObservables;
using RxDotNetDemo.WorkingWithRxConcurrencyAndSynchronization;

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
            //BasicQueryOperatorsDemo();
            //PartitionAndCombine();
            //ConcurrencyAndSynchronization();
            TimeBasedOperators();


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
            // 映射
            MappingOperator.Select();
            MappingOperator.SelectWithIndex();

            // 展平
            FlatteningOperator.FlatteningObservableOfEnumerable();
            FlatteningOperator.FlatteningObservableOfEnumerableWithSource();
            FlatteningOperator.FlatteningObservableOfObservable();
            FlatteningOperator.FlatteningObservableOfObservableWithSource();

            // 过滤
            FilteringObservable.FilteringWithTheWhere();
            FilteringObservable.CreatingDistinctSequence();
            FilteringObservable.RemovingDuplicateContiguousValues();

            // 聚合
            BasicAggregationOperator.BasicAggregationOperatorDemo();
            BasicAggregationOperator.FindingMaximumAndMinimumItems();
            BasicAggregationOperator.ScanAndAggregateDemo();
            BasicAggregationOperator.SecondLargestItem();
        }

        /// <summary>
        /// 拆分和组合的操作符 demo
        /// </summary>
        public static void PartitionAndCombine()
        {
            // zip 和 combineLast 配对多个 observable 发射出的通知
            CombiningObservables.ParingItemsFromObservables();
            CombiningObservables.CombiningTheLatestEmittedValue();

            // concat 串联多个 observable
            CombiningObservables.ConcatenatingObservables();
            CombiningObservables.ConcatenatingObservables2();
            CombiningObservables.ConcatenatingObservables3();

            // merge 合并多个 observable　发射的通知
            CombiningObservables.MergingObservables();
            CombiningObservables.DynamicConcatenatingAndMerging();
            CombiningObservables.ControllingTheConcurrencyOfMerge();
            CombiningObservables.SwitchLatestObservable();
            CombiningObservables.SwitchLatestObservable2();
            CombiningObservables.SwitchingToTheFirstObservableToEmit();
        }

        /// <summary>
        /// rx 控制并发是利用 scheduler 机制，这里写了一些关于 rx scheduler 的 demo
        /// </summary>
        public static void ConcurrencyAndSynchronization()
        {
            ControllingConcurrencyWithSchedulers.FirstScheduler();
            ControllingConcurrencyWithSchedulers.ParameterizingConcurrency();
            ControllingConcurrencyWithSchedulers.ParameterizingConcurrency2();
            ControllingConcurrencyWithSchedulers.ParameterizingConcurrency3();

            ControllingConcurrencyWithSchedulers.TestNewThreadScheduler();
            ControllingConcurrencyWithSchedulers.TestThreadPoolScheduler();
            ControllingConcurrencyWithSchedulers.TestTaskPoolScheduler();
            ControllingConcurrencyWithSchedulers.TestCurrentThreadScheduler();
            ControllingConcurrencyWithSchedulers.TestImmediateScheduler();
            ControllingConcurrencyWithSchedulers.TestEventLoopScheduler();
        }

        public static void TimeBasedOperators()
        {
            UsingTimeBasedOperators.AddTimestamp();
            UsingTimeBasedOperators.AddTimeIntervalBetweenNotifications();
            UsingTimeBasedOperators.AddingTimeoutPolicy();
            UsingTimeBasedOperators.DelayingTheNotifications();
            UsingTimeBasedOperators.AddingVariableDelay();
            UsingTimeBasedOperators.ThrottlingNotifications();
            UsingTimeBasedOperators.VariableThrottle();
            UsingTimeBasedOperators.SamplingObservableInIntervals();
        }
    }
}
