using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceTracker.Models.Location;
using AttendanceTracker.Models.User;

namespace AttendanceTracker.Controllers.Shared
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult PossibleUsers()
        {
            return Json(new PossibleUsersModel(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PossibleLocations()
        {
            return Json(new PossibleLocationsModel(), JsonRequestBehavior.AllowGet);
        }
    }
}