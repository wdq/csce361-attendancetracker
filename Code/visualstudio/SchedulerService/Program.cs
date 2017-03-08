using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.ServiceProcess;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace SchedulerService
{
    public static class Program
    {
        public const string ServiceName = "SchedulerService";
        public static string notification = "";

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
                Console.ReadKey();
                Stop();
            }


        }

        private static void Start(string[] args)
        {
            notification = "---------------------\n";
            notification += "-------Starting------\n";
            notification = "---------------------\n";
            Console.Write(notification);
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
        }

    }
}
