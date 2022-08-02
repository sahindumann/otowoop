using EClassField.API.Models;
using EClassField.Core.Domain.User;
using EClassField.Data;
using MesajPaneli.Business;
using MesajPaneli.Models;
using MesajPaneli.Models.JsonPostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{


    public enum MessageType
    {
        SifreTalebi = 0,
        UyeOlma = 1,
        OzelMesaj = 2
    }

    public class UserController : ApiController
    {

        public User GetUser(int Id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                return ctx.Set<User>().FirstOrDefault(d => d.Id == Id);

            }


        }

        [HttpGet]
        public IHttpActionResult GetUserInfo(int Id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var user = ctx.Set<User>().FirstOrDefault(d => d.Id == Id);
                if (user != null)
                    return Ok(new
                    {
                        Id = user.Id,
                        User = user.Name + " " + user.SurName,
                        Email = user.Email,
                        Tel1 = user.Cep ?? "",
                        Tel2 = user.EvTel ?? "",
                        Tel3 = user.IsTel ?? "",
                        Tel4 = user.IsTel2 ?? "",
                        IsCeptenOnay=string.IsNullOrEmpty(user.SmsSifre)?false:true
                    });
                else
                    return Ok("");

            }
        }

        [HttpPost]
        public IHttpActionResult SignUp(User user)
        {

            EClassField.Core.Domain.User.User u = null;

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                string email = user.Email.ToLower();

                var userr = ctx.Set<User>().FirstOrDefault(d => d.Email != "otowoopdestek@gmail.com" && d.Email.ToLower() == email);
                var tel = ctx.Set<User>().FirstOrDefault(d => d.Email != "otowoopdestek@gmail.com" && d.Cep.ToLower() == user.Cep);
                if (userr != null)
                {
                    return Ok(new { Message = "Email adresi kullanılmaktadır." });
                }

                if (tel != null)
                {
                    return Ok(new { Message = "Telefon numarası sistemde kayıtlıdır." });
                }



                if (user != null )
                {
                    user.SmsSifre = RandomString(5);

                

                    user.CreationTime = DateTime.Now;
                    user.IsActive = false;

                    user.Password = Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + email + user.Password);
                    u = ctx.Set<User>().Add(user);
                    bool b = MesajGonder(user.Cep, u, ctx, "otowoop ailesine hoşgeldiniz cepten onay şifreniz : " + user.SmsSifre);
                        //MesajGonder(user.Cep, u, ctx, "otowoop ailesine hoşgeldiniz cepten onay şifreniz : " + user.SmsSifre);
                    if (!b)
                    {
                        return Ok(new { Message = "Telefon numarası geçrli değil." });
                    }

                    ctx.SaveChanges();
                }




            }

      
            return Ok(new { Id = u.Id, Name = u.Name, Surname = u.SurName });
        }



        [HttpPost]
        public IHttpActionResult ActiveUser(User user)
        {




            EClassField.Core.Domain.User.User u = null;

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                string email = user.Email.ToLower();



                u = ctx.Set<User>().FirstOrDefault(d => d.Email == user.Email && d.SmsSifre == user.SmsSifre);
                if (u != null)
                {


                    u.IsActive = true;
                    ctx.SaveChanges();
                    return Ok(new { Id = u.Id, Name = u.Name, Surname = u.SurName,IsCeptenOnay=u.IsActive });

                }


            }



            return Ok(new { Message = "Telefon numarası geçerli değil." });
        }


        [HttpPost]
        [ActionName("UserLogin")]
        public IHttpActionResult Login(User u)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                //Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + email + user.Password);
                string pass = Function.MD5Sifrele("06by5561,*!1.2.3.4.5" + u.Email + u.Password);
                var user = ctx.Set<User>().FirstOrDefault(d => d.Email == u.Email && d.Password == pass);

                if (user != null)
                {
                    return Ok(new { Id = user.Id,Email=user.Email, Admin = user.IsAdmin,IsCeptenOnay=user.IsActive });
                }
                else
                {
                    return Ok(new { Id = "-1" });
                }

            }
        }

        [HttpPost]

        public void PostTel(User user)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {


                var _user = ctx.Set<User>().FirstOrDefault(d => d.Id == user.Id);

                if (_user != null)
                {
                    _user.EvTel = user.EvTel;
                    _user.Cep = user.Cep;
                    _user.IsTel = user.IsTel;
                    _user.IsTel2 = user.IsTel2;






                    ctx.SaveChanges();

                }


            }



        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool MesajGonder(string tel, User user, ClassFieldDbContext ctx, string message)
        {



            List<string> telList = new List<string>();
            telList.Add(tel);


            smsData MesajPaneli = new smsData();

            MesajPaneli.user = new UserInfo("5394432937", "9qwljl");
            MesajPaneli.msgBaslik = "otowoop.com";
            MesajPaneli.msgData.Add(new msgdata(telList, message));   // Numaralar başında "0" olmadan yazılacaktır
            MesajPaneli.tr = true;

            ReturnValue ReturnData = MesajPaneli.DoPost("http://api.mesajpaneli.com/json_api/", true, true);

            if (ReturnData.status)
            {


                return true;
            }
            else
            {
                return false;
            }


        }



    }
}
