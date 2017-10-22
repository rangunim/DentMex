﻿using System.Web;
using System.Web.Optimization;

namespace DentMex.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/sidebar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/sidebar.css"));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.fixes.js"
                ));

            bundles.Add(new StyleBundle("~/Content/timepicker").Include(
    "~/Content/bootstrap-timepicker.min.css"
    ));
            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                "~/Scripts/bootstrap-timepicker.min.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/combobox").Include(
                "~/Scripts/bootstrap-combobox.js"
                ));

            bundles.Add(new StyleBundle("~/Content/combobox").Include(
                "~/Content/bootstrap-combobox.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/bootstrap-datepicker.min.js",
                "~/Scripts/bootstrap-datepicker.pl.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                "~/Content/bootstrap-datepicker3.min.css"
                ));
        }
    }
}
