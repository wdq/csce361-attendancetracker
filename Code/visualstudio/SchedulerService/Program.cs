using System;
using System.Collections.Generic;
using System.Threading;
using System.ServiceProcess;
using System.IO;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace SchedulerService
{
    public static class Program
    {
        public const string ServiceName = "SchedulerService";
        public static string notification = "";
        // public static WebSocketServer NodeSocket = new WebSocketServer("ws://192.168.0.50");

        public class Service : ServiceBase
        {
            public Service()
            {
                ServiceName = Program.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }

        public class NodeConnection : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                Console.WriteLine(e.Data);
                Send(e.Data);
                
                // base.OnMessage(e);
            }

            protected override void OnOpen()
            {
                Console.WriteLine("Opening a connection.");
                base.OnOpen();
            }

            protected override void OnClose(CloseEventArgs e)
            {
                Console.WriteLine("Closing Connection\n");
                base.OnClose(e);
            }

            protected override void OnError(WebSocketSharp.ErrorEventArgs e)
            {
                Console.WriteLine(e); 
                base.OnError(e);
            }

            protected int Send_To_Node(Dictionary<String, String> input)
            {

                return 0;
            }
            private string Dictionary_To_JSON(Dictionary<String, String> input)
            {

                return "";
            }
        }


        static void Main(string[] args)
        {
            if (!Environment.UserInteractive) // service
            {
                using (var service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
            else // console
            {
                Start(args);
                // Console.ReadKey();
                // Stop();
            }


        }

        private static void Start(string[] args)
        {
            notification = "---------------------\n";
            notification += "-------Starting------\n";
            notification = "---------------------\n";
            Console.Write(notification);

            var NodeSocket = new WebSocketServer(4565);

            Console.Write("Starting websocket port.....");
            NodeSocket.AddWebSocketService<NodeConnection>("/node", ()=> new NodeConnection { IgnoreExtensions = true});
            NodeSocket.Start();

            if (NodeSocket.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", NodeSocket.Port);
                foreach (var path in NodeSocket.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            Console.Write("Websocket running");

            using (StreamWriter w = System.IO.File.AppendText(@"ServiceLog.txt"))
            while (true)
            {
                notification = "Running... " + DateTime.Now.ToString() + "\n";
                Console.Write(notification);
                
                
                


                notification = "----------\n";
                Console.Write(notification);
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        private static void Stop()
        {
            notification = "---------------------\n";
            notification += "----Shutting Down----\n";
            notification = "---------------------\n";
            Console.Write(notification);

            // NodeSocket.Stop();

        }

    }
}
