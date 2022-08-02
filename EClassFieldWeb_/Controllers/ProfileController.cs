using EClassField.Core;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Media;
using EClassField.Core.Domain.Rating;
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
    public class ProfileController : Controller
    {

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        void ViewBagHazirla(ClassFieldDbContext ctx, User user)
        {



            var products = ctx.Set<Product>().Where(d => d.User.Id == user.Id).ToList();

            ViewBag.yayindaolan = products.FindAll(d => d.IsActive).ToList().Count;
            ViewBag.yayindaolmayan = products.FindAll(d => !d.IsActive).ToList().Count;

            var vitrinimage = ctx.Set<UserImage>().Where(d => d.Picture.IsVitrin && d.UserId == user.Id).FirstOrDefault();

            if (vitrinimage != null)
            {
                ViewBag.userImage = "http://image6.otomarket.com/Image/GetImageProfile/?image=" + vitrinimage.Picture.FileName + "&isVitrin=true";
            }
            else
            {
                ViewBag.userImage = "https://s3-us-west-2.amazonaws.com/otonomide/profilemages/placeholder_logo.jpg";
            }

            ViewBag.user = user;

        }
        public ActionResult GetDetail()
        {
            {

                using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                {


                    var user = General.User;


                    ViewBagHazirla(ctx, user);
                }



                return View();
            }
        }

        public ActionResult GetIlan(int type = 0)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var user = General.User;
                int userID = user.Id;
                ViewBag.user = user;

                var products = ctx.Set<Product>().AsQueryable();

                if (type == -1)
                    products = products.Where(d => d.User.Id == userID);
                else if (type == 0)
                    products = products.Where(d => d.User.Id == userID && d.IsActive == false);
                else
                    products = products.Where(d => d.User.Id == userID && d.IsActive == true);



                ViewBag.products = products.ToList().Select(d => new ProductModelView
                {

                    Id = d.Id,
                    Categories = d.ProductCategories != null && d.ProductCategories.Any() ? d.ProductCategories.Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList() : new List<Input>(),
                    Attribute = d.ProductAttributes != null && d.ProductPictures.Any() ? d.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon }).ToList() : new List<Input>(),
                    City = "",
                    Date = d.CreationTime.ToShortDateString() + "",
                    FullLoc = Function.FullLoc(d.City, d.Town, d.Area, d.Neighborhood),
                    Image = d.ProductPictures != null && d.ProductPictures.Any() ? d.ProductPictures.LastOrDefault().Picture.FileName : "",
                    Price = d.Price + "",
                    Town = "",
                    Title = d.Title,
                    Description = d.Description ?? "Acıklama Bulunmuyor"


                }).ToList();
                ViewBagHazirla(ctx, user);

            }


            return View();
        }


        public ActionResult GetFav()
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var user = General.User;
                int userID = user.Id;

                ViewBag.user = user;
                var products = ctx.Set<ProductFavori>().Where(d => d.UserId == userID).ToList();



                ViewBag.products = products.Where(d => d.UserId == userID).Select(d => new ProductModelView
                {

                    Id = d.Id,
                    Categories = d.Product.ProductCategories.Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList(),
                    Attribute = d.Product.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon }).ToList(),
                    City = d.Product.City.Name,
                    Date = d.Product.CreationTime.ToShortDateString() + "",
                    FullLoc = d.Product.City.Name + "/" + d.Product.Town.Name + "/" + d.Product.Area.Name + "/" + d.Product.Neighborhood.Name,
                    Image = d.Product.ProductPictures.LastOrDefault().Picture.FileName,
                    Price = d.Product.Price + "",
                    Town = d.Product.Town.Name,
                    Title = d.Product.Title


                }).ToList();

                ViewBagHazirla(ctx, user);
            }


            return View();
        }


        public string UploadImage(HttpPostedFileWrapper file)
        {
            var user = General.User;

            string url = "";
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 10).Replace("-", "") + user.Id;
                //AmazonS3.UploadFile(file.InputStream, guid);

                string image = guid + ".jpg";

                url = image;
                AmazonS3.UploadFile(file.InputStream, image, "profilemages/");

                var vitrinimage = ctx.Set<UserImage>().Where(d => d.Picture.IsVitrin && d.UserId == user.Id).FirstOrDefault();
                if (vitrinimage != null)
                    ctx.Set<UserImage>().Remove(vitrinimage);

                ctx.Set<UserImage>().Add(new UserImage
                {
                    UserId = user.Id,
                    Picture = new Picture { FileName = image, MimeType = "type", IsVitrin = true }

                });

                ctx.SaveChanges();


            }
            return "http://image6.otomarket.com/Image/GetImageProfile/?image=" + url + "&isVitrin=true";

        }


        public ActionResult ProfileEdit()
        {

            var user = General.User;

            string url = "";
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                ViewBagHazirla(ctx, user);
            }


            return View(user);
        }
        [HttpPost]
        public ActionResult ProfileEdit(User u)
        {

            var user = General.User;

            string url = "";
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var user2 = ctx.Set<EClassField.Core.Domain.User.User>().FirstOrDefault(d => d.Id == user.Id);

                u.Email = user.Email;
                UpdateModel(user2);

                ViewBagHazirla(ctx, user);

                ctx.SaveChanges();
            }


            return View(user);
        }


        public ActionResult GetKazanc()
        {
            var user = General.User;

            string url = "";
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var user2 = ctx.Set<EClassField.Core.Domain.User.User>().FirstOrDefault(d => d.Id == user.Id && d.IsActive);

                var toplamkazanc = ctx.Set<Kazanc>().Where(d => d.UserID == user.Id && d.IsActive);
                if (toplamkazanc.Any())
                {
                    var toplamkazanc_ = toplamkazanc.Sum(d => d.KazancMiktar);

                    ViewBag.kazanc = toplamkazanc;
                }

                else
                {
                    ViewBag.kazanc = 0;
                }
                var result = ctx.Set<Kazanc>().Where(d => d.UserID == user.Id).ToList();

                ViewBagHazirla(ctx, user);

                return View(result);
            }



        }

    }
}