
using EClassField.Data;
using EClassFieldWeb_;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EClassField.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
             bool _isSqlTypesLoaded = false;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ClassFieldDbContext ctx = new ClassFieldDbContext();
            Cache.Load(ctx);
            if (!_isSqlTypesLoaded)
            {
                SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
                _isSqlTypesLoaded = true;
            }

        }
    }
}
