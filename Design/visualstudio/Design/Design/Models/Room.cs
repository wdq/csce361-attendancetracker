using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Building getBuilding()
        {
            return new Building();
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