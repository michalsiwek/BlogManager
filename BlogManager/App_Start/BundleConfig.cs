using System.Web;
using System.Web.Optimization;

namespace BlogManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/data_tables").Include(
                        "~/Scripts/DataTables/jquery.datatables.js",
                        "~/Scripts/DataTables/datatables.bootstrap.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            // Custom scripts
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/Custom/preview.js",
                        "~/Scripts/Custom/open-modal.js",
                        "~/Scripts/Custom/pictures-upload.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom-datatables").Include(
                        "~/Scripts/Custom/custom-data-table.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom-cascade-dropdowns").Include(
                "~/Scripts/Custom/cascade-dropdown.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-darkly.css",
                      "~/Content/site.css",
                      "~/Content/DataTables/css/datatables.bootstrap.css",
                      "~/Content/my-styles.css"));
        }
    }
}
