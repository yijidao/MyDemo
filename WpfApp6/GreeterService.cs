using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
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
        public override Task<WpfReply> GetUserControl1(WpfRequest request, ServerCallContext context)
        {
            try
            {
                var p = new HwndSourceParameters("UserControl1")
                {
                    ParentWindow = new IntPtr(-3),
                    WindowStyle = 1073741824
                };
                
                var hwndSource = App.Current.Dispatcher.Invoke(() =>
                {
                    var source = new HwndSource(p);
                    source.RootVisual = new UserControl1()
                    {
                        Height = 300,Width = 500
                    };
                    source.CompositionTarget.BackgroundColor = Colors.White;
                    source.SizeToContent = SizeToContent.Manual;
                    return source;
                });

                return Task.FromResult(new WpfReply { Handle = hwndSource.Handle.ToInt32() });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                
            }

            return Task.FromResult(new WpfReply());

        }
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
            //var p = new HwndSourceParameters("UserControl1")
            //{
            //    ParentWindow = hwndParent.Handle,
            //    WindowStyle = 0x40000000
            //};
            var p = new HwndSourceParameters("UserControl1")
            {
                ParentWindow = new IntPtr(-3),
                WindowStyle = 1073741824
            };
            var source = new HwndSource(p)
            {
                RootVisual = new UserControl1()
                {
                    Height = _height,
                    Width = _width
                }
            };

            return new HandleRef(this, source.Handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {

        }
    }
}
