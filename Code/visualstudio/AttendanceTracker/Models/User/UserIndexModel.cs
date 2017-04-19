using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceTracker.Models.User
{
    public class UserIndexModel
    {
    }

    public class UserIndexTableResultModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<UserIndexTableModel> Data { get; set; }

        public UserIndexTableResultModel(int draw, int recordsTotal, int recordsFiltered, List<UserIndexTableModel> data)
        {
            Draw = draw;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            Data = data;
        }
    }

    public class UserIndexTableModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NUID { get; set; }

        public static UserIndexTableResultModel UserTable(HttpRequestBase Request)
        {
            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            List<UserIndexTableModel> usersTable = new List<UserIndexTableModel>();

            using (AttendanceTrackerDatabaseConnection database = new AttendanceTrackerDatabaseConnection())
            {
                var users = database.Users.ToList();

                foreach (var user in users)
                {
                    usersTable.Add(FromUser(user, database));
                }
            }


            int totalRecords = usersTable.Count;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                usersTable = usersTable.Where(x =>
                    (x.FirstName != null ? x.FirstName.ToLower() : "").Contains(search.ToLower()) ||
                    (x.LastName != null ? x.LastName.ToLower() : "").Contains(search.ToLower()) ||
                    (x.Email != null ? x.Email.ToLower() : "").Contains(search.ToLower()) ||
                    (x.NUID != null ? x.NUID.ToLower() : "").Contains(search.ToLower())
                ).ToList();
            }

            usersTable = SortByColumnWithOrder(order, orderDir, usersTable);

            int recFilter = usersTable.Count;
            usersTable = usersTable.Skip(startRec).Take(pageSize).ToList();

            return new UserIndexTableResultModel(Convert.ToInt32(draw), totalRecords, recFilter, usersTable);
        }

        public static UserIndexTableModel FromUser(AttendanceTracker.User user, AttendanceTrackerDatabaseConnection database)
        {
            var model = new UserIndexTableModel();
            var requestContext = HttpContext.Current.Request.RequestContext;
            var Url = new UrlHelper(requestContext);

            var commandButtonLeftHtml = "<a href='" + Url.Action("View", "User") + "?id=" + user.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-search'></i></span></a>";
            commandButtonLeftHtml += "<a href='" + Url.Action("Edit", "User") + "?id=" + user.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-pencil'></i></span></a>";
            model.Id = commandButtonLeftHtml;

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = "<a href='mailto:" + user.EmailAddress + "'>" + user.EmailAddress + "</a>";
            model.NUID = user.NUID.ToString();

            return model;
        }

        private static List<UserIndexTableModel> SortByColumnWithOrder(string order, string orderDir, List<UserIndexTableModel> data)
        {
            List<UserIndexTableModel> list = new List<UserIndexTableModel>();
            try
            {
                switch (order)
                {
                    case "0":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Id).ToList() : data.OrderBy(x => x.Id).ToList();
                        break;
                    case "1":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.FirstName).ToList() : data.OrderBy(x => x.FirstName).ToList();
                        break;
                    case "2":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.LastName).ToList() : data.OrderBy(x => x.LastName).ToList();
                        break;
                    case "3":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Email).ToList() : data.OrderBy(x => x.Email).ToList();
                        break;
                    case "4":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.NUID).ToList() : data.OrderBy(x => x.NUID).ToList();
                        break;
                    default:
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.LastName).ToList() : data.OrderBy(x => x.LastName).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

    }
}