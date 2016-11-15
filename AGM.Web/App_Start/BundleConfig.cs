using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using AGM.Web.Infrastructure;
using System.Configuration;

namespace AGM.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var libsBundle = new ScriptBundle("~/bundles/libs.js");
            libsBundle.Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/i18n/angular-locale_it-it.js",
                "~/Scripts/angular-sanitize.min.js",
                "~/Scripts/angular-ui-router.min.js",
                "~/Scripts/angular-local-storage.min.js",
                "~/Scripts/angular-strap.min.js",
                "~/Scripts/angular-strap.tpl.min.js",
                "~/Scripts/angular-file-upload.min.js",
                "~/Scripts/angular-ui/ui-bootstrap.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/underscore.min.js",
                "~/Scripts/bootstrap-toggle.min.js",
                "~/Scripts/jquery.nanoscroller.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/editor.js"

            );
            bundles.Add(libsBundle);

            var scriptBundle = new ScriptBundle("~/bundles/app.js");
            scriptBundle.Include("~/App/app.js");
            scriptBundle.IncludeDirectory("~/App", "*.js", true);
            bundles.Add(scriptBundle);

            var styleBundle = new StyleBundle("~/Content/app.css");
            styleBundle.Include(
                "~/Content/bootstrap.css",
                "~/css/font-awesome.css",
                "~/Content/animate.css",
                "~/Content/nanoscroller.css",
                "~/Content/bootstrap-toggle.css",
                "~/Content/Site.css",
                "~/App/JobAds/JobAds.css",
                "~/Content/editor.css");
            bundles.Add(styleBundle);

            System.Web.Optimization.BundleTable.EnableOptimizations = bool.Parse(ConfigurationManager.AppSettings["ScriptBundleOptimization"]);
        }
    }
}
