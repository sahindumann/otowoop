using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CallPartial(string id="")
        {

            return PartialView("~/Views/Panel/" + id + ".cshtml");
        }
    }
}