using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EClassFieldWeb_
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
   name: "CBS",
   url: "cbs",
   defaults: new { controller = "Cbs", action = "Index" }
);


            routes.MapRoute(
name: "Defaultbase",
url: "Base/{action}/{id}",
defaults: new { controller = "Base", id = UrlParameter.Optional }
);

            
            routes.MapRoute(
   name: "NasilKazanirim",
   url: "NasilKazanirim",
   defaults: new { controller = "Home", action = "NasilParaKazanirim" }
);


            routes.MapRoute(
   name: "Hakkimizda",
   url: "Hakkimizda",
   defaults: new { controller = "Home", action = "Hakkimizda" }
);

            routes.MapRoute(
   name: "iletisim",
   url: "iletisim",
   defaults: new { controller = "Home", action = "iletisim" }
);
            routes.MapRoute(
            name: "Yardim",
            url: "YardimDestek",
            defaults: new { controller = "Home", action = "YardimDestek" }
        );

            routes.MapRoute(
                name: "Kategori",
                url: "Kategori/{id}/{attrs}",
                defaults: new { controller = "Home", action = "Kategori", id = UrlParameter.Optional, attrs = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Blog",
            url: "Blog/{id}/{attrs}",
            defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional, attrs = UrlParameter.Optional }
        );


            routes.MapRoute(
            name: "BlogDetay",
            url: "blog/detay/{title}/{id}",
            defaults: new { controller = "Blog", action = "Detail", id = UrlParameter.Optional, title = UrlParameter.Optional }
        );

            routes.MapRoute(
                name: "ilan",
                url: "ilan/{detail}/{id}",
                defaults: new { controller = "Home", action = "Detail", id = UrlParameter.Optional, detail = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
