using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class PossibleUsersModelUser
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public PossibleUsersModelUser(AttendanceTracker.User user)
        {
            Id = user.Id.ToString();
            Name = user.LastName + ", " + user.FirstName + " (" + user.NUID + ")";
        }
    }

    public class PossibleUsersModel
    {
        public List<PossibleUsersModelUser> Users { get; set; }

        public PossibleUsersModel()
        {
            List<PossibleUsersModelUser> usersTemp = new List<PossibleUsersModelUser>();
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                foreach (var user in context.Users.OrderBy(x => (x.LastName + ", " + x.FirstName)).ToList())
                {
                    usersTemp.Add(new PossibleUsersModelUser(user));
                }
            }
            Users = usersTemp;
        }
    }
}