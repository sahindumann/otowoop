using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Directory;
using EClassField.Data;

using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class CBSController : Controller
    {
        // GET: CBS
        public ActionResult Index()
        {
            ViewBag.footer = false;
            return View();
        }


        public JsonResult GetCities(string wkt)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {


           
                DbGeography geom = DbGeography.FromText(wkt).Buffer(1000);


                return null;
                //return Json(new { data = neighborhoods.Select(x => new { Name = x.Name, IlceAdi = x.TownName, ilAdi = x.ILADI, Geo = x.Location.WellKnownValue }) }, JsonRequestBehavior.AllowGet);

            }


        }

        Expression<Func<Product, bool>> WhereExpression(
           PropertyInfo property, MethodInfo method, string filter)
        {
            var param = Expression.Parameter(typeof(Product), "o");
            var propExpr = Expression.Property(param, property);
            var methodExpr = Expression.Call(propExpr, method, Expression.Constant(filter));
            return Expression.Lambda<Func<Product, bool>>(methodExpr, param);
        }
    
    }
}