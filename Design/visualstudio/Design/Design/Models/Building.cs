using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class Building
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<Room> getRooms()
        {
            return new List<Room>();
        }

        public List<RoomDevice> getDevices()
        {
            return new List<RoomDevice>();
        }

        public List<Course> getCourses()
        {
            return new List<Course>();
        }


    }
}