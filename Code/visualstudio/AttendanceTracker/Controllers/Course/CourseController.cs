using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceTracker.Models.Course;

namespace AttendanceTracker.Controllers.Course
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CourseIndexTable()
        {
            var courseTable = CourseIndexTableModel.CourseTable(Request);

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
            return View(CourseViewModel.ViewCourse(id));
        }

        public ActionResult Edit(string id)
        {
            return View(CourseEditModel.CourseEdit(id));
        }

        [HttpPost]
        public ActionResult EditPost(CourseEditModel courseModel)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = CourseEditModel.CourseEditPost(courseModel).Id;

            return jsonResult;
        }
    }
}