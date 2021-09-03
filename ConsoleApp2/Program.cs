using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                using (var pipeClient = new AnonymousPipeClientStream(PipeDirection.In, args[0]))
                {
                    Console.WriteLine($"[CLIENT] Current TransmissionMode: {pipeClient.TransmissionMode}");
                    
                    using (var sr = new StreamReader(pipeClient))
                    {
                        string temp;
                        do
                        {
                            Console.WriteLine("[CLIENT] Wait for sync");
                            temp = sr.ReadLine();
                        } while (!temp.StartsWith("SYNC"));

                        while ((temp = sr.ReadLine())!= null)
                        {
                            Console.WriteLine($"[CLIENT] Echo: {temp}");
                        }
                    }
                }
            }
            Console.Write("[CLIENT] Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
