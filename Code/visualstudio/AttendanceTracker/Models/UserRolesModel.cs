using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models
{
    public class UserRolesModel
    {
        public static bool IsStudent(string userId)
        {
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                var user = context.Users.FirstOrDefault(x => x.AspNetUsersId == userId);
                if (user != null)
                {
                    if (user.Role == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsTeacher(string userId)
        {
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                var user = context.Users.FirstOrDefault(x => x.AspNetUsersId == userId);
                if (user != null)
                {
                    if (user.Role == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsAdmin(string userId)
        {
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                var user = context.Users.FirstOrDefault(x => x.AspNetUsersId == userId);
                if (user != null)
                {
                    if (user.Role == 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}