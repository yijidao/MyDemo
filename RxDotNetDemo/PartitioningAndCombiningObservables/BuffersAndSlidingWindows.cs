using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using RxDotNetDemo.Extensions;

namespace RxDotNetDemo.PartitioningAndCombiningObservables
{
    /// <summary>
    /// 流往往是没有边界的，所以有时候不利于操作。Rx 提供了两个Buffer 和 Window 用来给无界流限定一定的边界。
    /// 使用 Buffer 和 Window 来操作流，其实有点分治的思想。
    /// Buffer 会将 observable 的通知序列装进一个个集合里。
    /// Window 会将 observable 的通知序列装进一个个 observable 里。
    /// Buffer 会在边界闭合时，再将整个序列发射出来。
    /// Window 收到一个消息就直接发射一个消息。
    /// Window 和 Buffer  都有需要重载，来确定什么时候打开边界和关闭边界。
    /// </summary>

    class BuffersAndSlidingWindows
    {
        /// <summary>
        /// 使用 buffer(count, skin) 来实现缓存两个值，跳过一个值，来实现计算车速
        /// </summary>
        public static void BufferSpeed()
        {
            var speeds = new[] { 50, 51, 51.5, 53, 52 }; // 模拟每秒车速

            var speedReadings = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Select(x => speeds[x]);

            var timeDelta = 0.0002777777778; // 一秒在一小时中的占比

            var accelerations = from buffer in speedReadings.Buffer(2, 1)
                                where buffer.Count == 2
                                let speedDelta = buffer[1] - buffer[0]
                                select Math.Abs(speedDelta) / timeDelta;
            accelerations.SubscribeConsole("Acceleration");

        }

        /// <summary>
        /// 模拟一个聊天系统，使用 buffer 缓存100毫秒的消息再更新界面，免得频繁更新界面，使得界面卡顿
        /// </summary>
        public static void BufferChatMessage()
        {
            var coldMessages = Observable.Interval(TimeSpan.FromMilliseconds(50))
                .Take(4)
                .Select(x => $"Message {x}");

            // 模拟高频聊天消息
            var messages = coldMessages.Concat(coldMessages.DelaySubscription(TimeSpan.FromMilliseconds(200)))
                .Publish()
                .RefCount();

            // 按 100 毫秒缓存
            // 第一次打印两条，第二次打印两条，第三次打印四条
            messages.Buffer(messages.Throttle(TimeSpan.FromMilliseconds(100)))
                .SelectMany((b, i) => b.Select(x => $"Buffer {i} - {x}"))
                .SubscribeConsole("High-Rate Messages");

        }

        /// <summary>
        /// 使用 window 模拟一个捐款系统，十秒更新一次，并且每次接受接受捐款都会累计一下
        /// </summary>
        public static void WindowDonations()
        {
            var donations = Observable.Interval(TimeSpan.FromSeconds(2)).Select(x => (decimal)x);

            var windows = donations.Window(TimeSpan.FromSeconds(10));
            var donationsSums = from window in windows.Do(_ => Console.WriteLine("New Window"))
                                from sum in window.Scan((pre, cur) => pre + cur)
                                select sum;
            donationsSums.SubscribeConsole("donations in shift");

        }

    }
}
