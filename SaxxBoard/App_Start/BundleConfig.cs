using System.Web.Optimization;

namespace MailSendbox
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/libs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/flot/jquery.flot.js",
                "~/Scripts/flot/jquery.flot.time.js",
                "~/Scripts/flot/jquery.flot.resize.js",
                "~/Scripts/SaxxBoard/jquery.saxxBoardWidget.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/Site.css"));
        }
    }
}