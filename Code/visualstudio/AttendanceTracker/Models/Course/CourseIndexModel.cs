using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.Course
{
    public class CourseIndexModel
    {
    }

    public class CourseIndexTableResultModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CourseIndexTableModel> Data { get; set; }

        public CourseIndexTableResultModel(int draw, int recordsTotal, int recordsFiltered, List<CourseIndexTableModel> data)
        {
            Draw = draw;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            Data = data;
        }
    }

    public class CourseIndexTableModel
    {
        public string Id { get; set; }
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSemesterYear { get; set; }

        public static CourseIndexTableResultModel CourseTable(HttpRequestBase Request, string userId)
        {
            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            List<CourseIndexTableModel> CoursesTable = new List<CourseIndexTableModel>();

            using (AttendanceTrackerDatabaseConnection database = new AttendanceTrackerDatabaseConnection())
            {
                var Courses = database.Courses.ToList();
                List<AttendanceTracker.Course> filteredCourses = new List<AttendanceTracker.Course>();

                var user = database.Users.FirstOrDefault(x => x.AspNetUsersId == userId);
                if (user.Role == 0)
                {
                    foreach (var course in Courses)
                    {
                        if (database.CourseStudents.Any(x => x.UserId == user.Id && x.CourseId == course.Id))
                        {
                            filteredCourses.Add(course);
                        }
                    }
                }
                if (user.Role == 1)
                {
                    foreach (var course in Courses)
                    {
                        if (database.CourseOwners.Any(x => x.UserId == user.Id && x.CourseId == course.Id))
                        {
                            filteredCourses.Add(course);
                        }
                    }
                }
                if (user.Role == 2)
                {
                    filteredCourses = Courses;
                }
                foreach (var Course in filteredCourses)
                {
                    CoursesTable.Add(FromCourse(Course, database));
                }
            }


            int totalRecords = CoursesTable.Count;

            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                CoursesTable = CoursesTable.Where(x =>
                    (x.CourseName != null ? x.CourseName.ToLower() : "").Contains(search.ToLower()) ||
                    (x.CourseNumber != null ? x.CourseNumber.ToLower() : "").Contains(search.ToLower()) ||
                    (x.CourseTitle != null ? x.CourseTitle.ToLower() : "").Contains(search.ToLower()) ||
                    (x.CourseSemesterYear != null ? x.CourseSemesterYear.ToLower() : "").Contains(search.ToLower())
                ).ToList();
            }

            CoursesTable = SortByColumnWithOrder(order, orderDir, CoursesTable);

            int recFilter = CoursesTable.Count;
            CoursesTable = CoursesTable.Skip(startRec).Take(pageSize).ToList();

            return new CourseIndexTableResultModel(Convert.ToInt32(draw), totalRecords, recFilter, CoursesTable);
        }

        public static CourseIndexTableModel FromCourse(AttendanceTracker.Course course, AttendanceTrackerDatabaseConnection database)
        {
            var model = new CourseIndexTableModel();

            var commandButtonLeftHtml = "<a href='Course/View?id=" + course.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-search'></i></span></a>";
            commandButtonLeftHtml += "<a href='Course/Edit?id=" + course.Id.ToString() + "' class='btn btn-default hl-view' style='margin-right: 3px;'><i class='fa fa-pencil'></i></span></a>";
            model.Id = commandButtonLeftHtml;

            model.CourseName = course.CourseCode + " " + course.CourseNumber + "-" + course.CourseSection;
            model.CourseTitle = course.CourseName;
            model.CourseNumber = course.ClassNumber.ToString();
            model.CourseSemesterYear = course.Semester + " " + course.Year;

            return model;
        }

        private static List<CourseIndexTableModel> SortByColumnWithOrder(string order, string orderDir, List<CourseIndexTableModel> data)
        {
            List<CourseIndexTableModel> list = new List<CourseIndexTableModel>();
            try
            {
                switch (order)
                {
                    case "0":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.Id).ToList() : data.OrderBy(x => x.Id).ToList();
                        break;
                    case "1":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.CourseName).ToList() : data.OrderBy(x => x.CourseName).ToList();
                        break;
                    case "2":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.CourseNumber).ToList() : data.OrderBy(x => x.CourseNumber).ToList();
                        break;
                    case "3":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.CourseTitle).ToList() : data.OrderBy(x => x.CourseTitle).ToList();
                        break;
                    case "4":
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.CourseSemesterYear).ToList() : data.OrderBy(x => x.CourseSemesterYear).ToList();
                        break;
                    default:
                        list = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(x => x.CourseName).ToList() : data.OrderBy(x => x.CourseName).ToList();
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