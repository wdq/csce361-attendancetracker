using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceTracker.Models.User;
using Microsoft.AspNet.Identity;

namespace AttendanceTracker.Controllers.User
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserIndexTable()
        {
            var userTable = UserIndexTableModel.UserTable(Request);

            return Json(new
            {
                draw = userTable.Draw,
                recordsTotal = userTable.RecordsTotal,
                recordsFiltered = userTable.RecordsFiltered,
                data = userTable.Data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(string id)
        {
            return View(UserViewModel.ViewUser(id));
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

        public ActionResult EditBluetooth(string userId, string bluetoothId)
        {
            return View(UserBluetoothEditModel.UserBluetoothEdit(userId, bluetoothId));
        }

        [HttpPost]
        public ActionResult EditBluetoothPost(UserBluetoothEditModel userBluetoothModel)
        {
            JsonResult json = new JsonResult();
            json.Data = UserBluetoothEditModel.UserBluetoothEditPost(userBluetoothModel).UserId;
            return json;
        }

        public ActionResult StudentRegister()
        {
            var userId = User.Identity.GetUserId();
            return View(StudentRegisterModel.StudentRegister(userId));
        }
    }
}