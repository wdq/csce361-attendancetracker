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
        public List<CourseStudentStudent> Students { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }

        public class CourseStudentStudent
        {
            public CourseStudent Student { get; set; }
            public AttendanceTracker.User User { get; set; }

            public CourseStudentStudent()
            {
                
            }

            public CourseStudentStudent(CourseStudent student, AttendanceTracker.User user)
            {
                Student = student;
                User = user;
            }
        }

        public static CourseViewModel ViewCourse(string id, bool isAdmin, bool isTeacher, bool isStudent)
        {
            CourseViewModel CourseViewModel = new CourseViewModel();

            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                AttendanceTracker.Course Course = context.Courses.FirstOrDefault(x => x.Id == new Guid(id));
                CourseViewModel.Course = Course;
                CourseViewModel.IsAdmin = isAdmin;
                CourseViewModel.IsTeacher = isTeacher;
                CourseViewModel.IsStudent = isStudent;

                CourseViewModel.Room = Course.Room;
                CourseViewModel.Building = Course.Room.Building;

                List<CourseStudentStudent> studentsTemp = new List<CourseStudentStudent>();
                foreach (var student in Course.CourseStudents)
                {
                    studentsTemp.Add(new CourseStudentStudent(student, student.User));
                }
                CourseViewModel.Students = studentsTemp.OrderBy(x => x.User.LastName).ToList();
            }

            return CourseViewModel;
        }
    }
}