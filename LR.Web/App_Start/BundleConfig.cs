using System.Web;
using System.Web.Optimization;

namespace LR.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //脚本
            bundles.Add(new ScriptBundle("~/Scripts/library").Include(
                "~/Scripts/common/utils.js",
                "~/Scripts/common/common.js",
                 "~/Scripts/jquery-{version}.min.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/jquery-plugin/jnotify/jquery.jnotify.js"
                ));
        }
    }
}