using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        private static int _numClients = 4;
        static void Main(string[] args)
        {
            //AnonymousPipeClient(args);
            NamedPipeClient(args);
        }

        /// <summary>
        /// 匿名管道客户端
        /// </summary>
        /// <param name="args"></param>
        static void AnonymousPipeClient(string[] args)
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

                        while ((temp = sr.ReadLine()) != null)
                        {
                            Console.WriteLine($"[CLIENT] Echo: {temp}");
                        }
                    }
                }
            }
            Console.Write("[CLIENT] Press Enter to continue...");
            Console.ReadLine();
        }


        /// <summary>
        /// 命名管道客户端
        /// </summary>
        static void NamedPipeClient(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "SpawnClient")
                {
                    var pipeClient = new NamedPipeClientStream(".", "TestPipe", PipeDirection.InOut, PipeOptions.None,
                        TokenImpersonationLevel.Impersonation);

                    Console.WriteLine("Connecting to server...");
                    pipeClient.Connect();

                    var ss = new StreamString(pipeClient);
                    if (ss.ReadString() == "I am the one true server!")
                    {
                        ss.WriteString("c:\\temp\\textfile.txt"); 
                        Console.Write(ss.ReadString());
                    }
                    else
                    {
                        Console.WriteLine("Server could not be verified.");
                    }
                    pipeClient.Close();

                    Thread.Sleep(4000);

                }
            }
            else
            {
                Console.WriteLine("*** Named pipe client stream with impersonation example ***");
                StartClients();
            }
        }

        private static void StartClients()
        {
            var currentProcessName = Environment.CommandLine;

            currentProcessName = currentProcessName.Trim('"', ' ');

            currentProcessName = Path.ChangeExtension(currentProcessName, ".exe");
            var plist = new Process[_numClients];

            Console.WriteLine("Spawning client processes...");

            if (currentProcessName.Contains(Environment.CurrentDirectory))
            {
                currentProcessName = currentProcessName.Replace(Environment.CurrentDirectory, string.Empty);
            }

            currentProcessName = currentProcessName.Replace("\\", string.Empty);
            currentProcessName = currentProcessName.Replace("\"", string.Empty);

            int i;
            for (i = 0; i < _numClients; i++)
            {
                plist[i] = Process.Start(currentProcessName, "SpawnClient");
            }

            while (i > 0)
            {
                for (int j = 0; j < _numClients; j++)
                {
                    if (plist[j] != null)
                    {
                        if (plist[j].HasExited)
                        {
                            Console.WriteLine($"Client process [{plist[j].Id}] has exited.");
                        }

                        plist[j] = null;
                        i--;
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                }
            }
            Console.WriteLine("Client processes finished, exiting.");

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
