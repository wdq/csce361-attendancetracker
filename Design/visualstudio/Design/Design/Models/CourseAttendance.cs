using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class CourseAttendance
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public bool Attendance { get; set; }

        public void addCourseAttendance(CourseAttendance attendance)
        {
            
        }

        public void updateCourseAttendance(CourseAttendance attendance)
        {
            
        }

        public void deleteCourseAttendance()
        {
            
        }

        public Course getCourse()
        {
            return new Course();
        }

        public User getUser()
        {
            return new User();
        }
    }
}