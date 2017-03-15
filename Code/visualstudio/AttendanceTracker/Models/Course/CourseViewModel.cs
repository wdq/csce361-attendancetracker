using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.Course
{
    public class CourseViewModel
    {
        public AttendanceTracker.Course Course { get; set; }
        public AttendanceTracker.Room Room { get; set; }
        public AttendanceTracker.Building Building { get; set; }

        public static CourseViewModel ViewCourse(string id)
        {
            CourseViewModel CourseViewModel = new CourseViewModel();

            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                AttendanceTracker.Course Course = context.Courses.FirstOrDefault(x => x.Id == new Guid(id));
                CourseViewModel.Course = Course;

                CourseViewModel.Room = Course.Room;
                CourseViewModel.Building = Course.Room.Building;
            }

            return CourseViewModel;
        }
    }
}