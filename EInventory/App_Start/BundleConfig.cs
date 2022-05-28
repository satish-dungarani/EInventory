using System.Web;
using System.Web.Optimization;

namespace EInventory
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //                "~/Scripts/jquery-{version}.js"));

            //    bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //                "~/Scripts/jquery-ui-{version}.js"));

            //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //                "~/Scripts/jquery.unobtrusive*",
            //                "~/Scripts/jquery.validate*"));

            //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //                "~/Scripts/modernizr-*"));

            //    bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //    bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //                "~/Content/themes/base/jquery.ui.core.css",
            //                "~/Content/themes/base/jquery.ui.resizable.css",
            //                "~/Content/themes/base/jquery.ui.selectable.css",
            //                "~/Content/themes/base/jquery.ui.accordion.css",
            //                "~/Content/themes/base/jquery.ui.autocomplete.css",
            //                "~/Content/themes/base/jquery.ui.button.css",
            //                "~/Content/themes/base/jquery.ui.dialog.css",
            //                "~/Content/themes/base/jquery.ui.slider.css",
            //                "~/Content/themes/base/jquery.ui.tabs.css",
            //                "~/Content/themes/base/jquery.ui.datepicker.css",
            //                "~/Content/themes/base/jquery.ui.progressbar.css",
            //                "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new StyleBundle("~/bootstrapcss").Include("~/Content/bootstrap/css/bootstrap.css",
                                                                          "~/Content/dist/css/AdminLTE.css",
                                                                          "~/Scripts/plugins/iCheck/square/blue.css",
                                                                          "~/Content/dist/css/skins/_all-skins.css",
                                                                          "~/Scripts/plugins/datatables/dataTables.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bootstrapjs").Include("~/Scripts/plugins/jQuery/jQuery-{version}.js",
                                                                         "~/Content/bootstrap/js/bootstrap.js",
                                                                         "~/Scripts/plugins/iCheck/icheck.js",
                                                                         "~/Scripts/jquery.unobtrusive-ajax.min.js"));


            bundles.Add(new ScriptBundle("~/bootstrapAddonJs").Include("~/Scripts/plugins/slimScroll/jquery.slimscroll.js",
                                                                        "~/Scripts/plugins/fastclick/fastclick.js",
                                                                        "~/Content/dist/js/app.js",
                                                                        "~/Content/dist/js/demo.js"));


            bundles.Add(new ScriptBundle("~/bootstrapDataTableJs").Include("~/Scripts/plugins/datatables/jquery.dataTables.js",
                                                                    "~/Scripts/plugins/datatables/dataTables.bootstrap.js"));
        }
    }
}