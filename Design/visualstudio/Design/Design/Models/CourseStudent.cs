using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class CourseStudent
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }

        public void addCourseStudent(CourseStudent owner)
        {

        }

        public void updateCourseStudent(CourseStudent owner)
        {

        }

        public void deleteCoursStudent()
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