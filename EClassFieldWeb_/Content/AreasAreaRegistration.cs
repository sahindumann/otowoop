using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas
{
    public class AreasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Areas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Areas_default",
                "Panel/{controller}/{action}/{id}",
                new { action = "Index",controller="Default", id = UrlParameter.Optional }
            );
        }
    }
}