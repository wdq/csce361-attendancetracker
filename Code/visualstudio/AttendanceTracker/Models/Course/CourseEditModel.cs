using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.Course
{
    public class CourseEditModel
    {
        public string AddOrEdit { get; set; }

        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseNumber { get; set; }
        public int CourseSection { get; set; }
        public string CourseName { get; set; }
        public int ClassNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnSunday { get; set; }
        public bool IsOnMonday { get; set; }
        public bool IsOnTuesday { get; set; }
        public bool IsOnWednesday { get; set; }
        public bool IsOnThursday { get; set; }
        public bool IsOnFriday { get; set; }
        public bool IsOnSaturday { get; set; }
        public int StartTime { get; set; }
        public int StopTime { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public Guid LocationRoomId { get; set; }
        public string ActiveAttendanceCode { get; set; }

        public CourseEditModel()
        {
            
        }

        public CourseEditModel(AttendanceTracker.Course course, string addOrEdit)
        {
            AddOrEdit = addOrEdit;
            Id = course.Id;
            CourseCode = course.CourseCode;
            CourseNumber = course.CourseNumber;
            CourseSection = course.CourseSection;
            CourseName = course.CourseName;
            ClassNumber = course.ClassNumber;
            IsActive = course.IsActive;
            IsOnSunday = course.IsOnSunday;
            IsOnMonday = course.IsOnMonday;
            IsOnTuesday = course.IsOnTuesday;
            IsOnWednesday = course.IsOnWednesday;
            IsOnThursday = course.IsOnThursday;
            IsOnFriday = course.IsOnFriday;
            IsOnSaturday = course.IsOnSaturday;
            StartTime = course.StartTime;
            StopTime = course.StopTime;
            Semester = course.Semester;
            Year = course.Year;
            LocationRoomId = course.LocationRoomId;
            ActiveAttendanceCode = course.ActiveAttendanceCode;
        }

        public static CourseEditModel CourseEdit(string id)
        {
            string addOrEdit;
            if (string.IsNullOrEmpty(id))
            {
                CourseEditModel CourseEditModel = new CourseEditModel(new AttendanceTracker.Course(), "Add");

                return CourseEditModel;
            }
            else
            {
                AttendanceTracker.Course course = new AttendanceTracker.Course();
                using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
                {
                    course = context.Courses.FirstOrDefault(x => x.Id == new Guid(id));
                }
                CourseEditModel CourseEditModel = new CourseEditModel(course, "Edit");

                return CourseEditModel;;
            }
        }

        public static AttendanceTracker.Course CourseEditPost(CourseEditModel courseModel)
        {
            AttendanceTracker.Course databaseCourse = new AttendanceTracker.Course();
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                databaseCourse = context.Courses.FirstOrDefault(x => x.Id == courseModel.Id);

                if (databaseCourse != null)
                {
                    databaseCourse.CourseCode = courseModel.CourseCode;
                    databaseCourse.CourseNumber = courseModel.CourseNumber;
                    databaseCourse.CourseSection = courseModel.CourseSection;
                    databaseCourse.CourseName = courseModel.CourseName;
                    databaseCourse.ClassNumber = courseModel.ClassNumber;
                    databaseCourse.IsActive = courseModel.IsActive;
                    databaseCourse.IsOnSunday = courseModel.IsOnSunday;
                    databaseCourse.IsOnMonday = courseModel.IsOnMonday;
                    databaseCourse.IsOnTuesday = courseModel.IsOnTuesday;
                    databaseCourse.IsOnWednesday = courseModel.IsOnWednesday;
                    databaseCourse.IsOnThursday = courseModel.IsOnThursday;
                    databaseCourse.IsOnFriday = courseModel.IsOnFriday;
                    databaseCourse.IsOnSaturday = courseModel.IsOnSaturday;
                    databaseCourse.StartTime = courseModel.StartTime;
                    databaseCourse.StopTime = courseModel.StopTime;
                    databaseCourse.Semester = courseModel.Semester;
                    databaseCourse.Year = courseModel.Year;
                    databaseCourse.LocationRoomId = courseModel.LocationRoomId;
                    databaseCourse.ActiveAttendanceCode = courseModel.ActiveAttendanceCode;
                }
                else
                {
                    databaseCourse = new AttendanceTracker.Course();

                    databaseCourse.CourseCode = courseModel.CourseCode;
                    databaseCourse.CourseNumber = courseModel.CourseNumber;
                    databaseCourse.CourseSection = courseModel.CourseSection;
                    databaseCourse.CourseName = courseModel.CourseName;
                    databaseCourse.ClassNumber = courseModel.ClassNumber;
                    databaseCourse.IsActive = courseModel.IsActive;
                    databaseCourse.IsOnSunday = courseModel.IsOnSunday;
                    databaseCourse.IsOnMonday = courseModel.IsOnMonday;
                    databaseCourse.IsOnTuesday = courseModel.IsOnTuesday;
                    databaseCourse.IsOnWednesday = courseModel.IsOnWednesday;
                    databaseCourse.IsOnThursday = courseModel.IsOnThursday;
                    databaseCourse.IsOnFriday = courseModel.IsOnFriday;
                    databaseCourse.IsOnSaturday = courseModel.IsOnSaturday;
                    databaseCourse.StartTime = courseModel.StartTime;
                    databaseCourse.StopTime = courseModel.StopTime;
                    databaseCourse.Semester = courseModel.Semester;
                    databaseCourse.Year = courseModel.Year;
                    databaseCourse.LocationRoomId = courseModel.LocationRoomId;
                    databaseCourse.ActiveAttendanceCode = courseModel.ActiveAttendanceCode;

                    databaseCourse.Id = Guid.NewGuid();

                    context.Courses.Add(databaseCourse);
                }
                context.SaveChanges();
            }
            return databaseCourse;
        }


    }
}