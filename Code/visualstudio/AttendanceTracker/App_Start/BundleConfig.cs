using System.Web;
using System.Web.Optimization;

namespace AttendanceTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery-2.2.0/jquery-2.2.0.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Bootstrap-3.3.6/js/bootstrap.js",
                      "~/Scripts/bootstrap-toggle/js/bootstrap-toggle.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables-1.10.11/js/datatables.js",
                "~/Scripts/DataTables-1.10.11/js/datatables-bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Scripts/Bootstrap-3.3.6/css/bootstrap.css",
                      "~/Scripts/Bootstrap-3.3.6/css/bootstrap-theme.css",
                      "~/Scripts/DataTables-1.10.11/css/jquery.dataTables.css",
                      "~/Scripts/DataTables-1.10.11/css/dataTables.bootstrap.css",
                      "~/Scripts/font-awesome-4.6.3/css/font-awesome.css",
                      "~/Scripts/bootstrap-toggle/css/bootstrap-toggle.css",
                      "~/Scripts/bootstrap-timepicker/css/bootstrap-timepicker.css",
                      "~/Scripts/bootstrap-datepicker-1.6.1/css/bootstrap-datepicker3.css",
                      "~/Content/site.css"));
        }
    }
}
