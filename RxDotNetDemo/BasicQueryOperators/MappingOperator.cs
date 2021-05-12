using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.BasicQueryOperators
{
    /// <summary>
    /// 函数式编程中,有一个 Map 操作,在 Rx 实现为 Select
    /// 可以在 nuget 中引入方言包 System.Reactive.Observable.Aliases 来调用 map
    /// </summary>
    public class MappingOperator
    {
        public static void Select()
        {
            Observable.Range(1, 5)
                .Select(x => x * 2)
                .SubscribeConsole();
        }

        /// <summary>
        /// 带索引的select 重载
        /// </summary>
        public static void SelectWithIndex()
        {
            Observable.Range(1, 5)
                .Select((x, index) => $"index [{index}], value [{x * 2}]")
                .SubscribeConsole();
        }
    }
}
