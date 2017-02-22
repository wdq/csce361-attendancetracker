using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public int Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int NUID { get; set; }

        public void addUser(User user)
        {
            
        }

        public void updateUser(User user)
        {
            
        }

        public void deleteUser()
        {
            
        }

        public List<Course> getCourses()
        {
            return new List<Course>();
        }

        public List<UserBluetooth> getBluetooths()
        {
            return new List<UserBluetooth>();
        }

        public void addBluetooth(UserBluetooth bluetooth)
        {
            
        }

        public void removeBluetooth(Guid id)
        {
            
        }


    }
}