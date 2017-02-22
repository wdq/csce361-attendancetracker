using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class RoomDevice
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string IpAddress { get; set; }

        public Room getRoom()
        {
            return new Room();
        }
    }
}