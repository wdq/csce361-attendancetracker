using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class UserBluetooth
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public void addUserBluetooth(UserBluetooth bluetooth)
        {
            
        }

        public void removeUserBluetooth()
        {
            
        }

        public User getUser()
        {
            return new User();
        }
    }
}