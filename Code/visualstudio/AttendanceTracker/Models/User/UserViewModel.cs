using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class UserViewModel
    {
        public AttendanceTracker.User User { get; set; }

        public static UserViewModel ViewUser(string id)
        {
            UserViewModel userViewModel = new UserViewModel();

            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                AttendanceTracker.User user = context.Users.Where(x => x.Id == new Guid(id)).FirstOrDefault();
                userViewModel.User = user;
            }

            return userViewModel;
        }
    }
}