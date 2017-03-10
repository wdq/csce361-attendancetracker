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

                DateTime now = DateTime.Now;
                int nowInt = (now.Hour * 100) + (now.Minute); // Database stop/start times are in the form: 1430 instead of 2:30 PM.
                var dayOfWeek = now.DayOfWeek;
                nowInt = 1531; // testing
                dayOfWeek = DayOfWeek.Friday; // testing
                using (var context = new AttendanceSchedulerEntities())
                {
                    var coursesInSession = context.Courses.Where(x => (nowInt >= x.StartTime) && (nowInt < x.StopTime) && (x.IsActive));

                    if (dayOfWeek == DayOfWeek.Sunday) coursesInSession = coursesInSession.Where(x => x.IsOnSunday == true);
                    if (dayOfWeek == DayOfWeek.Monday) coursesInSession = coursesInSession.Where(x => x.IsOnMonday == true);
                    if (dayOfWeek == DayOfWeek.Tuesday) coursesInSession = coursesInSession.Where(x => x.IsOnTuesday == true);
                    if (dayOfWeek == DayOfWeek.Wednesday) coursesInSession = coursesInSession.Where(x => x.IsOnWednesday == true);
                    if (dayOfWeek == DayOfWeek.Thursday) coursesInSession = coursesInSession.Where(x => x.IsOnThursday == true);
                    if (dayOfWeek == DayOfWeek.Friday) coursesInSession = coursesInSession.Where(x => x.IsOnFriday == true);
                    if (dayOfWeek == DayOfWeek.Saturday) coursesInSession = coursesInSession.Where(x => x.IsOnSaturday == true);

                    foreach (var course in coursesInSession)
                    {
                        Console.Write(course.Id + " is in session, getting student information.");

                        // Get students in the course from Canvas API
                        // Get the student Bluetooth device addresses from the database (not in schema yet)
                        // Get the IP address of the room device in the room
                        // Send a request to the room device to take attendance

                    }


                }


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
