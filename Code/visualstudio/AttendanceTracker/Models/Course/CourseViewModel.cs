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
                AttendanceTracker.Course Course = context.Courses.Where(x => x.Id == new Guid(id)).FirstOrDefault();
                CourseViewModel.Course = Course;

                CourseViewModel.Room = context.Rooms.FirstOrDefault(x => x.Id == Course.LocationRoomId);
                CourseViewModel.Building = context.Buildings.FirstOrDefault(x => x.Id == CourseViewModel.Room.BuildingId);
            }

            return CourseViewModel;
        }
    }
}