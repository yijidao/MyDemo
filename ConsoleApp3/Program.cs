using System;
using System.AddIn.Pipeline;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;


namespace ConsoleApp3
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length != 2 && !File.Exists(args[1])) return;
            var path = args[0];
            var dll = Assembly.LoadFile(path);

            //var dll = Assembly.LoadFile(@"D:\CSharpCode\MyDemo\PluginBin\WpfControlLibrary1.dll");

            var startup = dll.ExportedTypes.FirstOrDefault(x => x.Name == "PluginStartup");
            var instance = Activator.CreateInstance(startup);
            var view = (FrameworkElement)startup.GetMethod("CreateView").Invoke(instance, null);
            var handle= GetViewHandle(view);

            using (var pipeClient = new AnonymousPipeClientStream(PipeDirection.Out, args[1]))
            {
                Console.WriteLine($"[CLIENT] Current TransmissionMode: {pipeClient.TransmissionMode}");

                using (var sw = new StreamWriter(pipeClient))
                {


                    Console.ReadLine();

                    //string temp;
                    //do
                    //{
                    //    Console.WriteLine("[CLIENT] Wait for sync");
                    //    temp = sr.ReadLine();
                    //} while (!temp.StartsWith("SYNC"));

                    //while ((temp = sr.ReadLine()) != null)
                    //{
                    //    Console.WriteLine($"[CLIENT] Echo: {temp}");
                    //}

                    sw.AutoFlush = true;
                    sw.WriteLine(handle);
                    //sw.WriteLine("SYNC");
                    //pipeClient.WaitForPipeDrain();
                    

                }
            }
            Console.Write("[CLIENT] Press Enter to continue...");
            Console.ReadLine();


            Console.ReadLine();
        }

        static string GetViewHandle(FrameworkElement view)
        {
            var p = new HwndSourceParameters("UserControl1")
            {
                ParentWindow = new IntPtr(-3),
                WindowStyle = 1073741824
            };

            var source = new HwndSource(p);
            source.RootVisual = view;
            source.CompositionTarget.BackgroundColor = Colors.White;
            source.SizeToContent = SizeToContent.Manual;
            return source.Handle.ToInt32().ToString();
        }
    }
}
