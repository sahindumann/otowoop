using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Models
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //&&user.Password==Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + "shnduman@gmail.com" + "06by5561A.123")
            var user = General.User;
            if (user!=null &&user.IsAdmin==true)
            {


  



            }
            else
            {

                HttpContext.Current.Response.Redirect("/Login");

            }
        }


    }
}