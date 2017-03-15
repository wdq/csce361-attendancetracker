using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class StudentRegisterModel
    {
        public string AspNetUserId { get; set; }
        public Guid UserId { get; set; }

        public StudentRegisterModel()
        {
            
        }

        public static UserEditModel StudentRegister(string userId)
        {
            StudentRegisterModel model = new StudentRegisterModel();
            model.AspNetUserId = userId;
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                var user = context.Users.FirstOrDefault(x => x.AspNetUsersId == userId);
                if (user != null)
                {
                    UserEditModel userEdit = new UserEditModel(user, "Edit");
                    return userEdit;
                }
                else
                {
                    user = new AttendanceTracker.User();
                    user.Role = 0;
                    user.AspNetUsersId = userId;
                    UserEditModel userEdit = new UserEditModel(user, "Add");
                    return userEdit;
                }
            }
        }
    }
}