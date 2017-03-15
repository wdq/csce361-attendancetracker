using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.Course
{
    public class CourseStudentEditModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }

        public CourseStudentEditModel()
        {
            
        }

        public static CourseStudentEditModel CourseStudentEdit(string courseId)
        {
            CourseStudentEditModel model = new CourseStudentEditModel();
            model.CourseId = new Guid(courseId);
            return model;
        }

        public static CourseStudent CourseStudentEditPost(CourseStudentEditModel model)
        {
            CourseStudent student = new CourseStudent();
            student.Id = Guid.NewGuid();
            student.CourseId = model.CourseId;
            student.UserId = model.StudentId;

            using (var context = new AttendanceTrackerDatabaseConnection())
            {
                context.CourseStudents.Add(student);
                context.SaveChanges();
            }

            return student;
        }

        public static string CourseStudentRemovePost(string id)
        {
            var courseId = "";
            using (var context = new AttendanceTrackerDatabaseConnection())
            {
                var courseStudent = context.CourseStudents.FirstOrDefault(x => x.Id == new Guid(id));
                courseId = courseStudent.CourseId.ToString();
                context.CourseStudents.Remove(courseStudent);
                context.SaveChanges();
            }
            return courseId;
        }
    }
}