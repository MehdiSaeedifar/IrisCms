using System.Web.Optimization;

namespace Iris.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-migrate-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/load.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/PersianCalender/calendar.js",
                "~/Scripts/PersianCalender/jquery.ui.datepicker-cc-fa.js",
                "~/Scripts/PersianCalender/jquery.ui.datepicker-cc.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryuitools").Include(
                "~/Scripts/PersianCalender/calendar.js",
                "~/Scripts/PersianCalender/jquery.ui.datepicker-cc-fa.js",
                "~/Scripts/PersianCalender/jquery.ui.datepicker-cc.js",
                "~/Scripts/jquery-ui-1.10.2.autocomplete.js",
                "~/Scripts/jquery-validator-combined.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/sitejs").Include(
                "~/Scripts/myscript.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                "~/Scripts/jquery.form.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/bootstrap-rtl.js",
                "~/Scripts/noty/jquery.noty.js",
                "~/Scripts/noty/layouts/top.js",
                "~/Scripts/noty/layouts/topCenter.js",
                "~/Scripts/noty/themes/default.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/redactor").Include(
                "~/Scripts/redactor/redactor.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/chosen/chosen.jquery.js",
                "~/Scripts/adminjs.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include(
                "~/Content/bootstrap/bootstrap-rtl.css",
                "~/Content/bootstrap/responsive-rtl.css"
                ));
            bundles.Add(new StyleBundle("~/Content/redactor/css").Include(
                "~/Scripts/redactor/redactor.css"
                ));

            bundles.Add(new StyleBundle("~/Content/sitecss").Include("~/Content/Style.css"));


            bundles.Add(new StyleBundle("~/Content/themes/start/autocompleteandanimations").Include(
                "~/Content/themes/start/jquery-ui-1.10.2.autocomplete.css",
                "~/Content/animate.css",
                "~/Content/themes/start/jquery.ui.datepicker.css"
                ));


            bundles.Add(new StyleBundle("~/Content/themes/start/css").Include(
                "~/Content/themes/start/jquery.ui.core.css",
                "~/Content/themes/start/jquery.ui.resizable.css",
                "~/Content/themes/start/jquery.ui.selectable.css",
                "~/Content/themes/start/jquery.ui.accordion.css",
                "~/Content/themes/start/jquery.ui.autocomplete.css",
                "~/Content/themes/start/jquery.ui.button.css",
                "~/Content/themes/start/jquery.ui.dialog.css",
                "~/Content/themes/start/jquery.ui.menu.css",
                "~/Content/themes/start/jquery.ui.slider.css",
                "~/Content/themes/start/jquery.ui.tabs.css",
                "~/Content/themes/start/jquery.ui.datepicker.css",
                "~/Content/themes/start/jquery.ui.progressbar.css",
                "~/Content/themes/start/jquery.ui.tooltip.css",
                "~/Content/themes/start/jquery.ui.theme.css"));


            //bundles.Add(new StyleBundle("~/Content").Include(
            //"~/Content/bootstrap-rtl.css", "~/Content/responsive-rtl.css"));
        }
    }
}