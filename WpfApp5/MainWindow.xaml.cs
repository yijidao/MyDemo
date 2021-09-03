using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var process = new Process();
            process.StartInfo.FileName = @"D:\CSharpCode\MyDemo\WpfApp6\bin\Debug\WpfApp6.exe";

            process.Start();
            
            //var pipeClient = new Process();
            //pipeClient.StartInfo.FileName = @"D:\CSharpCode\MyDemo\WpfApp6\bin\Debug\WpfApp6.exe";
            //using (var pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            //{
            //    textBox.AppendText($"[Server] Current TransmissionMode:{pipeServer.TransmissionMode}{Environment.NewLine}");

            //    pipeClient.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
            //    pipeClient.StartInfo.UseShellExecute = false;
            //    pipeClient.Start();

            //    pipeServer.DisposeLocalCopyOfClientHandle();
            //    try
            //    {
            //        using (var sw = new StreamWriter(pipeServer))
            //        {
            //            sw.AutoFlush = true;
            //            sw.WriteLine("SYNC");
            //            pipeServer.WaitForPipeDrain();
            //            //conso
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        Console.WriteLine(exception);
            //        throw;
            //    }

            //}
        }
    }
}
