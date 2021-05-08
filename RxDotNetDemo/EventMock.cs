using System;
using System.Collections.Generic;
using System.Text;

namespace RxDotNetDemo
{
    public class EventMock
    {
        public event EventHandler MessageEvent;

        public void RaiseEvent() => MessageEvent?.Invoke(this, new EventArgs());
    }
}
