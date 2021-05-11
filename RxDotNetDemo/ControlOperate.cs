using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo
{
    /// <summary>
    /// 控制 Observable 和 Observer 之间关系的操作符
    /// DelaySubscription 用于延迟订阅
    /// TakeUntil 用于结束订阅
    /// 
    /// </summary>
    public class ControlOperate
    {
        /// <summary>
        /// 延迟订阅Observable，调用IObservable.DelaySubscription(),接收绝对时间或相对时间
        /// </summary>
        public static void DelayingSubscription()
        {
            Console.WriteLine($"---  延迟订阅  Start [{DateTime.Now}]  ---");
            var observable = Observable.Range(1, 5)
                .Timestamp()
                .DelaySubscription(TimeSpan.FromSeconds(5));
            Task.Delay(2000).Wait();
            observable.SubscribeConsole("DelaySubscription()延迟订阅");
        }

        /// <summary>
        /// 结束发射通知，调用 IObservable.TakeUntil()，接收绝对时间，该方法在条件满足后，会结束发射通知并调用 OnComplete()
        /// </summary>
        public static void StopEmitForAbsoluteTime()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp().TakeUntil(DateTimeOffset.Now.AddSeconds(5))
                .SubscribeConsole("TakeUntil()根据绝对时间结束发射通知");
        }
        /// <summary>
        /// 接收外部Observable的通知来结束发射通知，调用 IObservable.TakeUntil()，接收一个外部的Observable 对象
        /// </summary>
        public static void StopEmitForAnotherObservable()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(5)))
                .SubscribeConsole("TakeUntil()接收外部Observable的通知来结束发射通知");
        }

        /// <summary>
        /// TakeUntil() 通过Where() 实现消息发射的控制
        /// </summary>
        public static void StopEmitForAnotherObservable2()
        {
            var o = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .TakeUntil(o.Where(x => x == 5))
                .SubscribeConsole("TakeUntil() 通过Where() 实现消息发射的控制");
        }

        /// <summary>
        /// SkinUntil() 实现跳过消息
        /// Skin() 可以按数量跳过消息
        /// </summary>
        public static void SkinNotification()
        {
            var o = Observable.Interval(TimeSpan.FromSeconds(1));

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .SkipUntil(o.Where(x => x == 4))
                .SubscribeConsole("SkinUntil() 实现消息的跳过");
        }

        /// <summary>
        /// 当不满足 TakeWhile() 的条件时，结束发射消息
        /// </summary>
        public static void StopByTakeWhile()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .TakeWhile(n => n < 5)
                .Timestamp()
                .SubscribeConsole("StopByTakeWhile");
        }

        /// <summary>
        /// 当不满足 SkinWhile() 的条件时，开始发射消息
        /// </summary>
        public static void StartBySkinWhile()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .SkipWhile(n => n < 5)
                .Timestamp()
                .SubscribeConsole("StartBySkinWhile");
        }

        /// <summary>
        /// 通过 Repeat() 实现重新订阅，Repeat 接收整型参数
        /// 只有Observable 完成了，才会开始重新订阅
        /// </summary>
        public static void ResubscribingByRepeat()
        {
            Observable.Range(1, 5)
                .Repeat(2)
                .Timestamp()
                .SubscribeConsole("ResubscribingByRepeat");

            // 只有Observable 完成了，才会开始重新订阅，所以这里永远不会触发第二次
            //Observable.Interval(TimeSpan.FromSeconds(1))
            //    .Repeat(2)
            //    .Timestamp()
            //    .SubscribeConsole("");

        }

        /// <summary>
        /// 通过 Do() 在执行管道中实现切面增强功能，改变状态不符合函数式变成无状态的理念，所以谨慎用
        /// 主要用来做log，debug操作
        /// </summary>
        public static void AddSideEffect()
        {
            Observable.Range(1, 5)
                .Log("range")
                .Where(x => x % 2 == 0)
                .Log("where")
                .Select(x => x * 2)
                .SubscribeConsole("final");
        }

    }
}
