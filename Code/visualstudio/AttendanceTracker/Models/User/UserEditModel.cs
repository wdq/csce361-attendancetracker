﻿using System;
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
        public string AspNetUsersId { get; set; }
        public bool IsAdmin { get; set; }

        public UserEditModel()
        {
            
        }

        public UserEditModel(AttendanceTracker.User user, string addOrEdit, bool isAdmin)
        {
            Id = user.Id;
            AddOrEdit = addOrEdit;
            FirstName = user.FirstName;
            LastName = user.LastName;
            EmailAddress = user.EmailAddress;
            NUID = user.NUID;
            Role = user.Role;
            AspNetUsersId = user.AspNetUsersId;
            IsAdmin = isAdmin;
        }

        public static UserEditModel UserEdit(string id, bool isAdmin)
        {
            string addOrEdit;
            if (string.IsNullOrEmpty(id))
            {
                UserEditModel userEditModel = new UserEditModel(new AttendanceTracker.User(), "Add", isAdmin);

                return userEditModel;
            }
            else
            {
                AttendanceTracker.User user = new AttendanceTracker.User();
                using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
                {
                    user = context.Users.FirstOrDefault(x => x.Id == new Guid(id));
                }
                UserEditModel userEditModel = new UserEditModel(user, "Edit", isAdmin);

                return userEditModel;
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
                    databaseUser.AspNetUsersId = userModel.AspNetUsersId;
                }
                else
                {
                    databaseUser = new AttendanceTracker.User();

                    databaseUser.FirstName = userModel.FirstName;
                    databaseUser.LastName = userModel.LastName;
                    databaseUser.EmailAddress = userModel.EmailAddress;
                    databaseUser.NUID = userModel.NUID;
                    databaseUser.Role = userModel.Role;
                    databaseUser.AspNetUsersId = userModel.AspNetUsersId;

                    databaseUser.Id = Guid.NewGuid();

                    context.Users.Add(databaseUser);
                }
                context.SaveChanges();
            }
            return databaseUser;
        }


    }
}