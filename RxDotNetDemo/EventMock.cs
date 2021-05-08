using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    public class EventMock
    {
        public event EventHandler MessageEvent;

        public event Action<string> NotFollowEventPatternEvent; // NotFollowEventPatternHandler

        public event Action<int, string> MultipleParameterEvent; // MultipleParameterEventHandler

        public event Action NotArgumentEvent; 

        public void RaiseEvent()
        {
            MessageEvent?.Invoke(this, new EventArgs());
            NotFollowEventPatternEvent?.Invoke("NotFollowEventPattern");
            MultipleParameterEvent?.Invoke(1, "string1");
            NotArgumentEvent?.Invoke();
        }
    }

    public delegate void NotFollowEventPatternHandler(string value);

    public delegate void MultipleParameterEventHandler(int value1, string value2);

}
