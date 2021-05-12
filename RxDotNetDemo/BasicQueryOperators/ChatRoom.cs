using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace RxDotNetDemo.BasicQueryOperators
{
    /// <summary>
    /// 聊天室模拟
    /// </summary>
    public class ChatRoom
    {
        private IObservable<string> _messages;
        public string Name { get; set; }

        /// <summary>
        /// 消息间隔
        /// </summary>
        public int Period { get; set; }

        public IObservable<string> Messages
        {
            get
            {
                return _messages ??= Observable.Interval(TimeSpan.FromSeconds(Period)).Select(x => $"{Name}{x}");
            }
            //set => _messages = value;
        }

        public ChatRoom(string name, int period = 5)
        {
            Name = name;
            Period = period;
        }

        public IObservable<string> Enter() => Messages;
    }
}
