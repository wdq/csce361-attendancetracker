using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceTracker.Controllers.User;
using AttendanceTracker.Models;
using AttendanceTracker.Models.Course;
using Microsoft.AspNet.Identity;

namespace AttendanceTracker.Controllers.Course
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if ( userId != null && UserRolesModel.IsStudent(userId) || UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }

        public FileResult GetAttendanceCsv(string courseId, string date)
        {
            string csvString = "Date,Class,StudentName,StudentNUID,Attended";
            DateTime dateTime = DateTime.Parse(date);

            using (var context = new AttendanceTrackerDatabaseConnection())
            {
                var course = context.Courses.FirstOrDefault(x => x.Id == new Guid(courseId));
                if (course != null)
                {
                    if (course.CourseAttendances.Any())
                    {
                        var attendances = course.CourseAttendances.Where(x => x.Date == dateTime);
                        foreach (var attendance in attendances)
                        {
                            csvString += "\n" + attendance.Date.ToShortDateString() + "," + course.CourseCode + " " + course.CourseNumber + "-" + course.CourseSection + "," + attendance.User.FirstName + " " + attendance.User.LastName + "," + attendance.User.NUID + "," + attendance.Attendance;                            
                        }
                    }
                }
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csvString), "text/csv", "AttendanceExport.csv");            
        }

        [HttpPost]
        public JsonResult CourseIndexTable()
        {
            var userId = User.Identity.GetUserId();
            var courseTable = CourseIndexTableModel.CourseTable(Request, userId);

            return Json(new
            {
                draw = courseTable.Draw,
                recordsTotal = courseTable.RecordsTotal,
                recordsFiltered = courseTable.RecordsFiltered,
                data = courseTable.Data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(string id)
        {
            var userId = User.Identity.GetUserId();
            return View(CourseViewModel.ViewCourse(id, UserRolesModel.IsAdmin(userId), UserRolesModel.IsTeacher(userId), UserRolesModel.IsStudent(userId)));
        }

        public ActionResult Edit(string id)
        {
            var userId = User.Identity.GetUserId();
            if (UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                return View(CourseEditModel.CourseEdit(id));
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }

        [HttpPost]
        public ActionResult EditPost(CourseEditModel courseModel)
        {
            var userId = User.Identity.GetUserId();
            if (UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                JsonResult jsonResult = new JsonResult();
                jsonResult.Data = CourseEditModel.CourseEditPost(courseModel, User.Identity.GetUserId()).Id;

                return jsonResult;
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }

        public ActionResult AddStudent(string courseId)
        {
            var userId = User.Identity.GetUserId();
            if (UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                return View(CourseStudentEditModel.CourseStudentEdit(courseId));
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }

        [HttpPost]
        public ActionResult AddStudentPost(CourseStudentEditModel model)
        {
            var userId = User.Identity.GetUserId();
            if (UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                JsonResult json = new JsonResult();
                json.Data = CourseStudentEditModel.CourseStudentEditPost(model).CourseId;
                return json;
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }

        public ActionResult RemoveStudentPost(string id)
        {
            var userId = User.Identity.GetUserId();
            if (UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                JsonResult json = new JsonResult();
                var run = CourseStudentEditModel.CourseStudentRemovePost(id);
                json.Data = "ok";
                json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return json;
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
        }
    }
}