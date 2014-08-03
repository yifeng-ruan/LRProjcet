using System.Web.Mvc;

namespace LR.Web.Areas.MS
{
    public class MSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MS_default",
                "MS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
