using EClassField.Core;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.OneSignal;
using EClassField.Data;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class IlanController : Controller
    {
        // GET: Areas/Ilan
        public ActionResult Index(int pageID = 1, int userID = -1, bool isAktif = false, string CreationTime = "asc")
        {
            ViewBag.Aktif = isAktif;
            ViewBag.CreationTime = CreationTime;
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var products = ctx.Set<Product>().Where(d => userID != -1 ? d.User.Id == userID : true && d.IsActive == isAktif);
                PageModel pagemodel = new PageModel();
                pagemodel.TotalCount = products.Count();
                pagemodel.PageSize = 20;
                pagemodel.PageIndex = pageID;

                ViewBag.PageModel = pagemodel;
                if (products.Any())
                {



                    if (CreationTime == "asc")
                        products = products.OrderBy(d => d.CreationTime).Skip((pageID - 1) * 8).Take(8);
                    else
                        products = products.OrderByDescending(d => d.CreationTime).Skip((pageID - 1) * 8).Take(8);

                    var aa = products.AsEnumerable().Select(d => new ProductModelView

                    {

                        Id = d.Id,
                        //Categories = d.ProductCategories.Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList(),
                        //Attribute = d.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon }).ToList(),
                        City =Function.FullLoc( d.City,null,null,null),
                        Date = d.CreationTime.ToShortDateString() + "",
                        FullLoc = Function.FullLoc(d.City,d.Town,d.Area,d.Neighborhood),
                        Image = d.ProductPictures.Any()? d.ProductPictures.LastOrDefault().Picture.FileName:null,
                        Price = d.Price + "",
                        Town = "",
                        Title = d.Title,
                        Description = d.Description ?? "Acıklama Bulunmuyor"
                    });


                    return View(aa.ToList());
                }
                else
                {
                    return View(new List<ProductModelView>());
                }
            }



        }


        public JsonResult IlanAktif(string securtiy = "111111", int productID = -1, bool aktif = false, string messsage = "")
        {



            if (securtiy == "111111")
            {
                using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                {

                    var product = ctx.Set<Product>().FirstOrDefault(d => d.Id == productID);
                    if (product != null)
                    {
                        product.IsActive = aktif;
                        ctx.SaveChanges();

                        var player = ctx.Set<PlayerUser>().FirstOrDefault(d => d.UserID == product.User.Id);


                        if (player != null)
                        {
                            if (player.PlayerID != 0)
                            {
                                var plyr = ctx.Set<Player>().FirstOrDefault(d => d.Id == player.PlayerID);
                                if (plyr != null)
                                    OneSignalAPI.SendMessage(messsage, new List<string>() { plyr.PlayerID });
                            }

                        }
                        return Json("T", JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        return Json("F", JsonRequestBehavior.AllowGet);
                    }




                }
            }

            return Json("G", JsonRequestBehavior.AllowGet);
        }


        public JsonResult SendMessage(int productID = -1)
        {
            //OneSignalAPI.SendMessage("İlanınız Onaylanamdı", "All" + "");




            return Json("G", JsonRequestBehavior.AllowGet);
        }
    }
}