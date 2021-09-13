using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace ClassLibrary2
{
    public class PluginLoader
    {

        private IntPtr _handle;
        public Process LoadPlugin(string assemblyPath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo()
            {
                FileName = "ConsoleApp3.exe",
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = assemblyPath
            };


            using (var pipeServer = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable))
            {
                Console.WriteLine($"[SERVER] Current TransmissionMode: {pipeServer.TransmissionMode}");

                startInfo.Arguments += $" {pipeServer.GetClientHandleAsString()}";
                process.StartInfo = startInfo;
                process.Start();

                pipeServer.DisposeLocalCopyOfClientHandle();

                try
                {
                    using (var sw = new StreamReader(pipeServer))
                    {
                        //sw.AutoFlush = true;
                        //sw.WriteLine("SYNC");
                        //pipeServer.WaitForPipeDrain();
                        //Console.WriteLine("[SERVER] Enter text:");
                        //sw.WriteLine(Console.ReadLine());

                        var viewHandle = sw.ReadLine();
                        _handle = new IntPtr(int.Parse(viewHandle));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

            return process;
            //return Process.Start(new ProcessStartInfo
            //{
            //    FileName = "ConsoleApp3.exe",
            //    UseShellExecute = false,
            //    CreateNoWindow = true,
            //    Arguments = assemblyPath
            //});
        }

        public FrameworkElement GetView() => new ControlHost(_handle) ;

    }

    class ControlHost : HwndHost
    {
        private readonly IntPtr _handle;

        public ControlHost(IntPtr handle)
        {
            _handle = handle;
        }
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            SetParent(new HandleRef((object)null, _handle), hwndParent);

            return new HandleRef(this, _handle);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);
    }
}
