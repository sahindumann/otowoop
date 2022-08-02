using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class DefaultController : Controller
    {
        // GET: Areas/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}