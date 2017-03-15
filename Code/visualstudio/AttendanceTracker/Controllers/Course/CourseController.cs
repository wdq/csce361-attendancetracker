﻿using System;
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
            if (UserRolesModel.IsStudent(userId) || UserRolesModel.IsTeacher(userId) || UserRolesModel.IsAdmin(userId))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Unauthorized", "User");
            }
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
            jsonResult.Data = CourseEditModel.CourseEditPost(courseModel, User.Identity.GetUserId()).Id;

            return jsonResult;
        }

        public ActionResult AddStudent(string courseId)
        {
            return View(CourseStudentEditModel.CourseStudentEdit(courseId));
        }

        [HttpPost]
        public ActionResult AddStudentPost(CourseStudentEditModel model)
        {
            JsonResult json = new JsonResult();
            json.Data = CourseStudentEditModel.CourseStudentEditPost(model).CourseId;
            return json;
        }

        public ActionResult RemoveStudentPost(string id)
        {
            JsonResult json = new JsonResult();
            var run = CourseStudentEditModel.CourseStudentRemovePost(id);
            json.Data = "ok";
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}