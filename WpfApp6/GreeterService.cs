using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Grpc.Core;
using GrpcGreeterClient;

namespace WpfApp6
{
    class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hi!{request.Name}!"
            });
        }
    }

    class WpfCommunicationService : WpfCommunication.WpfCommunicationBase
    {
        //public HwndHost _host = new ControlHost();

        public WpfCommunicationService()
        {
            
        }

        //public override Task<WpfReply> GetUserControl1(WpfRequest request, ServerCallContext context)
        //{
        //    var handle = _host.Handle.ToInt32();

        //    return Task.FromResult(new WpfReply
        //    {
        //        Handle = handle
        //    });
        //}
    }


    class ControlHost : HwndHost
    {
        private readonly double _height;
        private readonly double _width;

        public ControlHost(double height, double width)
        {
            _height = height;
            _width = width;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var p = new HwndSourceParameters("UserControl1")
            {
                ParentWindow = hwndParent.Handle,
                WindowStyle = 0x40000000
            };
            var source = new HwndSource(p) {RootVisual = new UserControl1()
            {
                Height = _height,
                Width = _width
            }};
            return new HandleRef(this, source.Handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            
        }
    }
}
