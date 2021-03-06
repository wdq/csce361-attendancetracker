﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.ServiceProcess;
using System.IO;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Web.Script.Serialization;
using System.Linq;

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

                var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
                var input = e.Data;
                Dictionary<string, string> values = serializer.Deserialize<Dictionary<string, string>>(input);
                

                if(values.ContainsKey("ping"))
                {
                    Console.WriteLine("Got a ping command");
                }
                else if(values.ContainsKey("verify_connection"))
                {
                    Send(e.Data);
                }
                else if(values.ContainsKey("bt_scan_results"))
                {

                    values.Remove("bt_scan_results");
                    

                    Console.WriteLine("Node:" + values["node_id"] + " reporting back! \n Bluetooth device \t Present");

                    var nodeId = values["node_id"];
                    values.Remove("node_id");
                    Console.WriteLine("scan results Connecting to DB");
                    using (AttendanceTrackerEntities1 context = new AttendanceTrackerEntities1())
                    {
                        Console.WriteLine("scan results Connected to DB");
                        var device = context.RoomDevices.FirstOrDefault(x => x.IpAddress == nodeId);
                        var room = device.Room;
                        var courses = context.Courses.Where(x => x.LocationRoomId == room.Id);
                        var currentCourse = courses.FirstOrDefault(x => x.Id == new Guid("7dc7d22b-ccdb-4b4c-94bb-77b8df245041")); // todo: base this on the class currently in session, this is just for the demo/testing
                        var students = currentCourse.CourseStudents;

                        foreach (var student in students)
                        {
                            bool here = false;

                            var user = student.User;
                            var bluetooths = user.UserBlueteeth;
                            foreach (var bluetooth in user.UserBlueteeth)
                            {
                                try
                                {
                                    if (values[bluetooth.Address] == "True")
                                    {
                                        here = true;
                                        break;
                                    }
                                }
                                catch (Exception ex) { 
                                }
                            }
                            CourseAttendance record = new CourseAttendance();
                            record.Id = Guid.NewGuid();
                            record.Date = DateTime.Now.Date;
                            record.CourseId = currentCourse.Id;
                            record.UserId = user.Id;
                            record.Attendance = here;
                            context.CourseAttendances.Add(record);
                        }
                        context.SaveChanges();
                    }

                    foreach (var result in values)
                    {
                        Console.WriteLine(result.Key + "\t " + result.Value);
                    }
                    Console.WriteLine("\n");
                }
                else if(values.ContainsKey("request"))
                {

                    Dictionary<string, string> node_return = new Dictionary<string, string>();

                    if (values.ContainsValue("bt_data_set"))
                    {
                        var nodeId = values["node_id"];
                        using (AttendanceTrackerEntities1 context = new AttendanceTrackerEntities1())
                        {
                            var device = context.RoomDevices.FirstOrDefault(x => x.IpAddress == nodeId);
                            var room = device.Room;
                            var courses = context.Courses.Where(x => x.LocationRoomId == room.Id);
                            var currentCourse = courses.FirstOrDefault(x => x.Id == new Guid("7dc7d22b-ccdb-4b4c-94bb-77b8df245041")); // todo: base this on the class currently in session, this is just for the demo/testing
                            var students = currentCourse.CourseStudents;

                            foreach (var student in students)
                            {
                                var user = student.User;
                                foreach (var bluetooth in user.UserBlueteeth)
                                {
                                    Console.WriteLine("Adding address: " + bluetooth.Address);
                                    node_return.Add(bluetooth.Address, "False");
                                }
                            }
                        }
                        
                        //node_return.Add("38:CA:DA:BF:84:02", "False");
                        //node_return.Add("B8:C6:8E:1F:B9:3D", "False");
                        //node_return.Add("24:da:9b:13:7e:2b", "False");
                        //node_return.Add("24:da:9b:13:7e:2c", "False");
                    }
                    else if(values.ContainsValue("sleep_time"))
                    {
                        Console.WriteLine("Sending sleep time.");
                        node_return.Add("sleep_timer", "600"); // 600 seconds is for testing/demo (10 minutes), would actually be the next time there is a class in that room
                    }
                    var str = serializer.Serialize(node_return);
                    Send(str);

                }
                else if(values.ContainsKey("error"))
                {

                    Console.WriteLine(values["error"]);
                }

                
                
                // base.OnMessage(e);
            }

            protected override void OnOpen()
            {
                base.OnOpen();
            }

            protected override void OnClose(CloseEventArgs e)
            {
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

            var NodeSocket = new WebSocketServer(989);

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
