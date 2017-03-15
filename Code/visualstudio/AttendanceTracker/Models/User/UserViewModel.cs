using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class UserViewModel
    {
        public AttendanceTracker.User User { get; set; }
        public List<AttendanceTracker.UserBluetooth> Bluetooths { get; set; }
        public bool IsAdmin { get; set; }

        public static UserViewModel ViewUser(string id, bool isAdmin)
        {
            UserViewModel userViewModel = new UserViewModel();


            userViewModel.IsAdmin = isAdmin;
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                AttendanceTracker.User user = context.Users.Where(x => x.Id == new Guid(id)).FirstOrDefault();
                userViewModel.User = user;
                userViewModel.Bluetooths = user.UserBlueteeth.ToList();
            }

            return userViewModel;
        }
    }
}