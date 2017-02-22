using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseNumber { get; set; }
        public int SectionNumber { get; set; }
        public int ClassNumber { get; set; }
        public string ClassTitle { get; set; }
        public bool IsClassOnSunday { get; set; }
        public bool IsClassOnMonday { get; set; }
        public bool IsClassOnTuesday { get; set; }
        public bool IsClassOnWednesday { get; set; }
        public bool IsClassOnThursday { get; set; }
        public bool IsClassOnFriday { get; set; }
        public bool IsClassOnSaturday { get; set; }
        public int StartTime { get; set; }
        public int StopTime { get; set; }
        public Guid LocationRoomId { get; set; }
        public bool IsActive { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public string ActiveAttendanceCode { get; set; }

        public void addCourse(Course course)
        {
            
        }

        public void updateCourse(Course course)
        {
            
        }

        public void deleteCourse()
        {
            
        }

        public List<User> getStudents()
        {
            return new List<User>();
        }

        public List<User> getOwners()
        {
            return new List<User>();
        }

        public void addStudent(Guid id)
        {
            
        }

        public void approveStudent(Guid id)
        {
            
        }

        public void removeStudent(Guid id)
        {
            
        }

        public void addOwner(Guid id)
        {
            
        }

        public void removeOwner(Guid id)
        {
            
        }

        public Room getRoom()
        {
            return new Room();
        }
    }
}