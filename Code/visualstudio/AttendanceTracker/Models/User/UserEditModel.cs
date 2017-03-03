using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class UserEditModel
    {
        public string AddOrEdit { get; set; }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int NUID { get; set; }
        public int Role { get; set; }

        public UserEditModel()
        {
            
        }

        public UserEditModel(AttendanceTracker.User user, string addOrEdit)
        {
            AddOrEdit = addOrEdit;
            FirstName = user.FirstName;
            LastName = user.LastName;
            EmailAddress = user.EmailAddress;
            NUID = user.NUID;
            Role = user.Role;
        }

        public static UserEditModel UserEdit(string id)
        {
            string addOrEdit;
            if (string.IsNullOrEmpty(id))
            {
                UserEditModel userEditModel = new UserEditModel(new AttendanceTracker.User(), "Add");

                return userEditModel;
            }
            else
            {
                AttendanceTracker.User user = new AttendanceTracker.User();
                using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
                {
                    user = context.Users.FirstOrDefault(x => x.Id == new Guid(id));
                }
                UserEditModel userEditModel = new UserEditModel(user, "Edit");

                return userEditModel;;
            }
        }

        public static AttendanceTracker.User UserEditPost(UserEditModel userModel)
        {
            AttendanceTracker.User databaseUser = new AttendanceTracker.User();
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                databaseUser = context.Users.FirstOrDefault(x => x.Id == userModel.Id);

                if (databaseUser != null)
                {
                    databaseUser.FirstName = userModel.FirstName;
                    databaseUser.LastName = userModel.LastName;
                    databaseUser.EmailAddress = userModel.EmailAddress;
                    databaseUser.NUID = userModel.NUID;
                    databaseUser.Role = userModel.Role;
                }
                else
                {
                    databaseUser = new AttendanceTracker.User();

                    databaseUser.FirstName = userModel.FirstName;
                    databaseUser.LastName = userModel.LastName;
                    databaseUser.EmailAddress = userModel.EmailAddress;
                    databaseUser.NUID = userModel.NUID;
                    databaseUser.Role = userModel.Role;

                    databaseUser.Id = Guid.NewGuid();

                    context.Users.Add(databaseUser);
                }
                context.SaveChanges();
            }
            return databaseUser;
        }


    }
}