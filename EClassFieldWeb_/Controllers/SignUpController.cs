using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(FormCollection col)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                string email = col["email"] ?? "";
                string tel = col["tel"] ?? "!1";
                string facebook = col["facebook"] ?? "";
                tel = tel.Replace("-", "").Replace(" ", "");


                string result = "";

                if (tel.Length < 11 && facebook == "")
                {
                    result = "Telefon numarası 11 haneli olmalı";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (tel.Length > 1 && tel[0] != '0' && facebook == "")
                {
                    result = "Telefon numarasının başında 0 olmalı";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var useremail = ctx.Set<User>().FirstOrDefault(d => d.Email == email);
                var usertel = ctx.Set<User>().FirstOrDefault(d => d.Email != "otomarketdestek@gmail.com" && ((tel != "" && tel != null) && (d.Cep.Replace("-", "").Replace(" ", "") == tel) || d.Cep.Contains(tel)));
                if (useremail != null || usertel != null)
                {

                    result = useremail != null ? "Bu kullanıcı adı ile daha önce kaydolunmuş" : usertel != null ? "Bu Telefon Numarası Daha Önce Kaydedilmiş" : "Kayıt Başarısız";


                }
                else
                {
                    string siffre = Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + email + col["sifre"]);
                    User u = new EClassField.Core.Domain.User.User();
                    u.Name = col["name"];
                    u.SurName = col["surname"];
                    u.Cep = col["tel"];
                    u.Email = col["email"];
                    u.CreationTime = DateTime.Now;
                    u.Password = siffre;
                    u.SmsSifre = Function.RandomString(5);
                    u.IsActive = false;
                    ctx.Set<User>().Add(u);
                    ctx.SaveChanges();

                    Function.MesajGonder(u.Cep,u,ctx, "otomarket ailesine hoşgeldiniz cepten onay şifreniz : " + u.SmsSifre);
                    HttpCookie cookie = new HttpCookie("Userotomarket", siffre);


                    cookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Clear();
                    Response.Cookies.Add(cookie);

                    ViewBag.Message = new Input { Text = "Başarı ile Kayıt Yapıldı", Value = "success" };


                    return Json(new { Result = true, ID = u.Id }, JsonRequestBehavior.AllowGet);
                }


                ViewBag.Message = result;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult CeptenOnay(string userID)
        {


            return View();
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CeptenOnay(int userID, string pass)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var result = false;
                var user = ctx.Set<User>().FirstOrDefault(d => d.Id == userID);
                if (user != null)
                {
                    if (user.SmsSifre == pass)
                    {

                        user.IsActive = true;
                        ctx.SaveChanges();

                       return Redirect("/");
                    }
                }
                else
                {
                    return RedirectToAction("CeptenOnay", new { userID = userID });
                }

            }


            return RedirectToAction("CeptenOnay","SignUp", new { userID = userID });
        }


    }
}