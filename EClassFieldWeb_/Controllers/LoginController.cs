using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public void Auth()
        {

            string value = Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + "abc@gmail.com" + "111111");
            HttpCookie cookie = new HttpCookie("Userotomarket", value);

            cookie.Expires = DateTime.Now.AddDays(10);



            Response.Cookies.Add(cookie);
            Response.Redirect("/");

        }


        public void LogOut()
        {

            Response.Cookies["Userotomarket"].Expires = DateTime.Now.AddDays(-1);

            Response.Redirect("/");
        }

        [HttpPost]
        public JsonResult Index(FormCollection col)
        {
            string email = col["email"];
            string sifre = col["sifre"];

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                string siffre = Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + email + sifre);
                User user = ctx.Set<User>().FirstOrDefault(d => d.Email == email && d.Password == siffre);
              
                if (user != null)
                {
                    string value = siffre;
                    HttpCookie cookie = new HttpCookie("Userotomarket",value);
              
                    cookie.Expires = DateTime.Now.AddDays(10);
                   


                    Response.Cookies.Add(cookie);


                    if (user.Email == "shnduman@gmail.com")
                    {
                       List<Category> cats = ctx.Set<Category>().ToList();
                        foreach (var item in cats)
                        {

                            item.MetaDescription = Function.getCategoriesPathString(item.Id);

                        }

                        Cache.Categories = cats;
                    }


                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Sistemde kaydınız bulunamadı Kaydol linkine tıklayarak facebook ile kaydolabilirsiniz", JsonRequestBehavior.AllowGet);
                }
            }





        }
    }
}