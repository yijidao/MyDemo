using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.BasicQueryOperators
{
    public class FilteringObservable
    {
        /// <summary>
        /// 使用 where 进行过滤
        /// </summary>
        public static void FilteringWithTheWhere()
        {
            var values = new[] { "aa", "AA", "bb", "bA", "Ab" };
            values.ToObservable()
                .Where(x => x.StartsWith("A"))
                .SubscribeConsole();
        }

        /// <summary>
        /// 使用 distinct 进行去重，distinct 有多个重载
        /// 使用 distinct 后，observable 中之前出现过的值，会被过滤掉，如果没有出现过的，则不会被过滤。
        /// </summary>
        public static void CreatingDistinctSequence()
        {
            var sub = new Subject<string>();
            var o = sub.AsObservable()
                .Log()
                .Distinct();

            o.SubscribeConsole("first");

                //.SubscribeConsole("distinct");

            sub.OnNext("a");
            sub.OnNext("b");
            o.SubscribeConsole("second"); 

            sub.OnNext("a"); // second 不会过滤掉 a， 但是 first 会过滤掉a
            sub.OnNext("c");
            sub.OnNext("c");
        }

        /// <summary>
        /// 使用 DistinctUntilChanged 进行相邻元素的去重
        /// DistinctUntilChanged 经常和 Throttle 一起组合在 wpf 的搜索框中使用
        /// </summary>
        public static void RemovingDuplicateContiguousValues()
        {
            var sbj = new Subject<string>();
            var o = sbj.AsObservable().Log().DistinctUntilChanged();

            o.SubscribeConsole("first");

            sbj.OnNext("a");
            sbj.OnNext("b");
            sbj.OnNext("a");
            o.SubscribeConsole("second");
            sbj.OnNext("a");
            sbj.OnNext("c");
            sbj.OnNext("c");

        }

    }
}
