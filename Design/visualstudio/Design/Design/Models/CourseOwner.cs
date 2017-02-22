using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class CourseOwner
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }

        public void addCourseOwner(CourseOwner owner)
        {
            
        }

        public void updateCourseOwner(CourseOwner owner)
        {
            
        }

        public void deleteCourseOwner()
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