using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Web.Optimization;


namespace AgroApp
{
   
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Css
            bundles.Add(new StyleBundle("~/Content/ui").Include("~/Content/css/icomoon.css",
                "~/Content/css/bootstrap.css",
                "~/Content/css/core.css",
                "~/Content/css/components.css",
                "~/Content/css/colors.css",
                "~/Content/css/bootbox.css"));

          

          
            #endregion

            #region Scripts
          

            var bundleJquery = new ScriptBundle("~/Scripts/JQueryScripts").Include("~/Scripts/jquery.min.js",
                     "~/Scripts/jquery.validate.min",
                     "~/Scripts/jquery.validate.unobtrusive.min.js");
            bundleJquery.Orderer = new AsIsBundleOrderer();
            bundles.Add(bundleJquery);


            #endregion
        }
    }
    public class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }

    }
}
