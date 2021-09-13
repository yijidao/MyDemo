using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Grpc.Core;
using GrpcGreeterClient;

namespace WpfApp7
{
    class ControlHost : HwndHost
    {
        private readonly Channel _channel;

        public ControlHost(Channel channel)
        {
            _channel = channel;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var client = new WpfCommunication.WpfCommunicationClient(_channel);
            var response = client.GetUserControl1(new WpfRequest());
            var handle = new IntPtr(response.Handle);
            SetParent(new HandleRef((object) null, handle), hwndParent);

            return new HandleRef(this, handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);
    }
}
