using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Design.Models;

namespace Design.Controllers
{
    public class UserController : Controller
    {
        public Guid addUser(User user)
        {
            return new Guid();
        }

        public bool updateUser(User user)
        {
            return new bool();
        }

        public bool deleteUser(Guid id)
        {
            return new bool();
        }

        public User getUser(Guid id)
        {
            return new User();
        }

        public List<User> searchUsers(string filter)
        {
            return new List<User>();
        }
    }
}