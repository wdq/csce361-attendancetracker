using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceTracker.Models.User;

namespace AttendanceTracker.Controllers.User
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View(UserEditModel.UserEdit(id));
        }

        [HttpPost]
        public ActionResult EditPost(UserEditModel userModel)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = UserEditModel.UserEditPost(userModel).Id;

            return jsonResult;
        }
    }
}