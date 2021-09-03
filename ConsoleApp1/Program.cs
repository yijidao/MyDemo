using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeClient = new Process();
            pipeClient.StartInfo.FileName = @"D:\CSharpCode\MyDemo\ConsoleApp2\bin\Debug\ConsoleApp2.exe";

            using (var pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            {
                Console.WriteLine($"[SERVER] Current TransmissionMode: {pipeServer.TransmissionMode}");

                pipeClient.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                pipeClient.StartInfo.UseShellExecute = false;
                pipeClient.Start();

                pipeServer.DisposeLocalCopyOfClientHandle();

                try
                {
                    using (var sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("SYNC");
                        pipeServer.WaitForPipeDrain();
                        Console.WriteLine("[SERVER] Enter text:");
                        sw.WriteLine(Console.ReadLine());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            pipeClient.WaitForExit();
            pipeClient.Close();
            Console.WriteLine($"[SERVER] Client quit. Server terminating.");
        }
    }
}
