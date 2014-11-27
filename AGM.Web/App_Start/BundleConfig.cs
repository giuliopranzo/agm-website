using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace AGM.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/App/app.js",
                "~/App/app.config.js",
                "~/App/http.config.js",
                "~/App/route.config.js",
                "~/App/app.service.js",
                "~/App/Authentication/authentication.config.js",
                "~/App/Authentication/authentication.service.js",
                "~/App/Authentication/authentication.js"
                
            ));

            bundles.Add(new ScriptBundle("~/bundles/applibs").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-local-storage.js",
                "~/Scripts/angular-strap.js",
                "~/Scripts/angular-strap.tpl.js",
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/jquery.cookie.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
