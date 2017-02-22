using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Design.Models;

namespace Design.Controllers
{
    public class CourseController : Controller
    {
        public Guid addCourse(Course course)
        {
            return new Guid();
        }

        public bool updateCourse(Course course)
        {
            return new bool();
        }

        public bool deleteCourse(Guid id)
        {
            return new bool();
        }

        public Course getCourse(Guid id)
        {
            return new Course();
        }

        public List<Course> searchCourses(string filter)
        {
            return new List<Course>();
        }

        public string getAttendanceCode(Guid id)
        {
            return "";
        }

        public bool submitAttendanceCode(Guid id, string code)
        {
            return new bool();
        }

        public bool addOwner(Guid courseId, Guid userId)
        {
            return new bool();            
        }

        public bool removeOwner(Guid courseId, Guid userId)
        {
            return new bool();
        }

        public bool addStudent(Guid courseId, Guid userId)
        {
            return new bool();
        }

        public bool removeStudent(Guid courseId, Guid userId)
        {
            return new bool();
        }

        public List<Course> getOwnedCourses(Guid id)
        {
            return new List<Course>();
        }

        public List<Course> getMemberCourses(Guid id)
        {
            return new List<Course>();
        }

        public byte[] exportAttendance(Guid id)
        {
            return new byte[''];
        }

        public List<User> getOwners(Guid id)
        {
            return new List<User>();
        }

        public List<User> getStudents(Guid id)
        {
            return new List<User>();
        }

        public List<CourseAttendance> getAttendance(Guid id)
        {
            return new List<CourseAttendance>();
        }

        public bool saveAttendance(List<CourseAttendance> attendance)
        {
            return new bool();
        }

        public bool requestAccess(Guid id)
        {
            return new bool();
        }
    }
}