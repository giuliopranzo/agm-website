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
            var appBundle = new ScriptBundle("~/bundles/app");
            appBundle.Orderer = new AppBundleOrderer(new List<string>() { "data-mock", "app.js"});
            appBundle.IncludeDirectory("~/App", "*.js", true);
            bundles.Add(appBundle);

            var scriptBundle = new ScriptBundle("~/bundles/applibs").Include(
                "~/Scripts/angular.js",
                "~/Scripts/i18n/angular-locale_it-it.js",
                "~/Scripts/angular-sanitize.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-local-storage.js",
                "~/Scripts/angular-strap.js",
                "~/Scripts/angular-strap.tpl.js",
                "~/Scripts/angular-file-upload.js",
                "~/Scripts/angular-ui/ui-bootstrap.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/underscore.js",
                "~/bower_components/ng-ckeditor/ng-ckeditor.js"
            );
            bundles.Add(scriptBundle);

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/App/JobAds/JobAds.css"));

            System.Web.Optimization.BundleTable.EnableOptimizations = bool.Parse(ConfigurationManager.AppSettings["ScriptBundleOptimization"]);
        }
    }
}
