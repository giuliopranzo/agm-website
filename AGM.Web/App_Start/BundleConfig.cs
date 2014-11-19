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
                "~/App/http.config.js",
                "~/App/route.config.js",
                "~/App/app.service.js"
                
            ));

            bundles.Add(new ScriptBundle("~/bundles/applibs").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/satellizer.js",
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/jquery.cookie.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
