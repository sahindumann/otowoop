﻿using EClassField.API.Models;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace EClassField.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ClassFieldDbContext ctx = new ClassFieldDbContext();
            Cache.Load(ctx);
        }
    }
}
