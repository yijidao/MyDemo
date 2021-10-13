using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static int _numThread = 4;
        static void Main(string[] args)
        {
            //AnonymousPipeServer();
            //NamedPipeServer();

            var watcher = new FileSystemWatcher($"{Environment.CurrentDirectory}");

            watcher.NotifyFilter = 
                                    NotifyFilters.CreationTime
                                  
                                   | NotifyFilters.FileName
                                   
                                   | NotifyFilters.LastWrite
                                   
                                   | NotifyFilters.Size;

            watcher.Changed += (sender, eventArgs) => { };
            watcher.Created += (sender, eventArgs) => { };
            watcher.Deleted += (sender, eventArgs) => { };
            watcher.Renamed += (sender, eventArgs) => { };
            watcher.Error += (sender, eventArgs) => { };

            watcher.Filter = "PrismDemo.config";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

        }

        /// <summary>
        /// 匿名管道服务端
        /// </summary>
        static void AnonymousPipeServer()
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


        static void NamedPipeServer()
        {
            int i;
            var servers = new Thread[_numThread];

            Console.WriteLine("*** Named pipe server stream with impersonation example ***");
            Console.WriteLine("Waiting for client connect...");

            for (i = 0; i < _numThread; i++)
            {
                servers[i] = new Thread(ServerThread);
                servers[i].Start();
            }
            Thread.Sleep(250);
            while (i > 0)
            {
                for (int j = 0; j < _numThread; j++)
                {
                    if (servers[j] != null)
                    {
                        if (servers[j].Join(250))
                        {
                            Console.WriteLine($"Server thread [{servers[j].ManagedThreadId}] finished.");
                            servers[j] = null;
                            i--;
                        }
                    }
                }
            }

            Console.WriteLine("Server threads exhausted,exiting.");
        }

        private static void ServerThread(object data)
        {
            var pipeServer = new NamedPipeServerStream("TestPipe", PipeDirection.InOut, _numThread);
            var threadId = Thread.CurrentThread.ManagedThreadId;

            pipeServer.WaitForConnection();

            Console.WriteLine($"Client connected on thread [{threadId}]");

            try
            {
                var ss = new StreamString(pipeServer);
                ss.WriteString("I am the one true server!");
                var filename = ss.ReadString();
                var fileReader = new ReadFileToStream(ss, filename);

                Console.WriteLine($"Reading file:{filename} on thread [{threadId}] as users: {pipeServer.GetImpersonationUserName()}");
                pipeServer.RunAsClient(fileReader.Start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    /// <summary>
    /// 定义在流中读写的协议
    /// </summary>
    class StreamString
    {
        private readonly Stream _ioStream;
        private readonly UnicodeEncoding _streamEncoding = new UnicodeEncoding();

        public StreamString(Stream ioStream)
        {
            _ioStream = ioStream;
        }

        public string ReadString()
        {
            int len = 0;

            len = _ioStream.ReadByte() * 256;
            len += _ioStream.ReadByte();

            var inBuffer = new byte[len];
            _ioStream.Read(inBuffer, 0, len);
            return _streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            var outBuffer = _streamEncoding.GetBytes(outString);
            var len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = UInt16.MaxValue;
            }

            _ioStream.WriteByte((byte)(len / 256));
            _ioStream.WriteByte((byte)(len & 255));
            _ioStream.Write(outBuffer, 0, len);
            _ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }


    class ReadFileToStream
    {
        private readonly StreamString _ss;
        private readonly string _filename;


        public ReadFileToStream(StreamString ss, string filename)
        {
            _ss = ss;
            _filename = filename;
        }

        public void Start()
        {
            var content = File.ReadAllText(_filename);
            _ss.WriteString(content);
        }
    }
}
