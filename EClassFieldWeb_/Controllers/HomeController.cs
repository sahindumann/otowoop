using EClassField.Core;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity.Spatial;
using EClassField.Core.Domain.Directory;
using EClassField.Core.Domain.Blog;

namespace EClassFieldWeb_.Controllers
{
    public class HomeController : BaseController
    {





        public string OSman(string lon = "", string lat = "")
        {
            var a = DbGeography.PointFromText($"POINT({lon} {lat})", 4326).Buffer(2000);

            return a.WellKnownValue.WellKnownText.ToString();
        }

        public void ViewBagHazirla()
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var userr = General.User;
                if (userr != null)
                {
                    var user = ctx.Set<EClassField.Core.Domain.User.User>().FirstOrDefault(d => d.Id == userr.Id);

                    if (user != null)
                    {


                        var image = user.UserImages.ToList().Find(d => d.Picture.IsVitrin);
                        if (image != null)
                        {
                            ViewBag.ProfileImage = image != null ? "http://image6.otomarket.com/Image/GetImageProfile/?image=" + image.Picture.FileName + "&isVitrin=true" : "https://s3-us-west-2.amazonaws.com/otonomide/profilemages/placeholder_logo.jpg";
                        }

                    }
                }
            }
        }

        // GET: Home
        public ActionResult Index()
        {

            ViewBag.Title = "Sahibinden Satılık, Kiralık, Otomobil, Yedek Parça Ürünleri";
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var parentcategories = new int[] { 2 };

                var ids = Cache.Categories.Select(d => d.Name).Distinct();

                var categories = Cache.Categories.Where(d => parentcategories.Contains(d.ParentCategoryId)).Select(d => new Input { Text = d.Name, Value = d.Id + "" }).ToList();

                var products = ctx.Set<Product>().Where(d => d.IsActive).OrderByDescending(d => d.CreationTime).Take(32).ToList().Select(d => new ProductModelView
                {


                    Id = d.Id,
                    Categories = d.ProductCategories != null ? d.ProductCategories.Where(x => x.Category != null).Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList() : new List<Input>(),
                    Attribute = d.ProductAttributes.Where(x=>x.SubAttribute!=null).Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value, Icon = a.Attribute.Icon }).ToList(),
                    City = d.City != null ? d.City.Name : "",
                    Date = d.CreationTime.ToShortDateString() + "",
                    FullLoc = Function.FullLoc(d.City, d.Town, d.Area, d.Neighborhood),
                    Image = d.ProductPictures.Where(x=>x.Picture!=null).FirstOrDefault() != null ? d.ProductPictures.FirstOrDefault().Picture.FileName : "",
                    Price = d.Price + "",
                    Town = d.Town == null ? "" : d.Town.Name,
                    Title = d.Title




                });

                ViewBag.Products = products.ToList();
                ViewBag.Categories = categories;
                ViewBagHazirla();
            }

            return View();
        }


        public ActionResult Kategori(string id, string attrs, string orderby = "asc", string ordertype = "CreationTime", int pageID = 1, int pageSize = 20, int username = -1, string fullorder = "", int[] city = null, int[] town = null, int[] neighborhood = null)
        {

            ViewBag.fullorder = fullorder;
            id = Function.GetStringFormatTextSearchCategory(id);
            Category cat = Cache.Categories.Find(d => Function.GetStringFormatTextSearchCategory(d.FullPath).Contains(id));
            if (!String.IsNullOrEmpty(Request.QueryString["search"]))
            {
                cat = Cache.Categories.Find(d => Function.GetStringFormatTextSearchCategory(d.FullPath).Contains(id));

            }


            ViewBag.BreadCrumbs = new List<Input>();
            if (cat != null)
            {
                var katyol = Function.GetCategoryPath(cat.Id);

                string text = "";
                string selectedkat = "";
                string title = "";


                if (cat.IsBlog)
                {





                    var breadcrumbs = katyol.Select(d => new Input { Text = d.Name, Value = d.Id + "" }).ToList();

                    var cats = katyol.Select(d => d.Id).ToList();
                    ViewBag.BreadCrumbs = breadcrumbs;
                    using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                    {

                        var posts = ctx.Set<Post>().Where(x => x.IsActive && x.PostCategories.Any(y=>y.CategoryId==cat.Id)).Select(d => new PostViewModel
                        {

                            Id = d.Id,
                            Title = d.Title,
                            CreationTime = d.CreationTime,
                            Deleted = d.Deleted,
                            Description = d.Description,
                            MetaDescription = d.MetaDescription,
                            IsActive = d.IsActive,
                            MetaKeywords = d.MetaKeywords,
                            MetaTitle = d.MetaTitle,
                            Pictures = d.PostPictures.Select(p => new Input { Value = p.Picture.FileName }).ToList(),
                            Tags = d.PostTags.Select(t => new Input { Value = t.Tag.Name }).ToList()

                        }).OrderBy(x => x.CreationTime).ToList();


                    ViewBag.posts = posts;

                        ViewBag.catstr = Function.GetStringFormatText(Function.getCategoriesPathString(cat.Id));
                        ViewBag.SubCategories = Cache.Categories.FindAll(d => d.ParentCategoryId == cat.Id);

                        ViewBag.IsSearchFilter = false;
                        ViewBag.BreadCrumbs = breadcrumbs;

                        ViewBagHazirla();


                        return View("~/Views/Partial/_blogList.cshtml",posts);

                    }


                }

                else if (cat.IsIlan)
                {

                    if (katyol.Count > 2)
                    {
                        selectedkat = katyol[2].Name;
                        text = katyol[2].Name + " Fiyat Listesi, sıfır satılık otomobil fiyatları ve Sahibinden " + selectedkat + " Araba Modelleri ile Türkiye'nin en büyük oto pazarı otomarket.com";

                        if (katyol.Select(d => d.ParentCategoryId).Contains(2))
                        {
                            title = selectedkat + " Fiyat Listesi ve Sahibinden Araba Modelleri | otomarket.com";
                        }

                    }
                    else
                    {
                        text = cat.Name + " Fiyat Listesi, sıfır satılık otomobil fiyatları ve Sahibinden Tüm Araba Modelleri ile Türkiye'nin en büyük oto pazarı otomarket.com";
                        selectedkat = cat.Name;

                        title = "2.El Arabalar ve Satılık Sıfır Km Otomobil Fiyatları otomarket.comda";





                    }

                    ViewBag.Title = title;
                    ViewBag.Desc = text;
                    ViewBag.Keywords = selectedkat + " Fiyat Listesi, ve, Sahibinden, " + selectedkat + ", Araba, Modelleri, |, otomarket.com";



                    ViewBag.catstr = Function.GetStringFormatText(Function.getCategoriesPathString(cat.Id));
                    ViewBag.SubCategories = Cache.Categories.FindAll(d => d.ParentCategoryId == cat.Id);

                    var breadcrumbs = katyol.Select(d => new Input { Text = d.Name, Value = d.Id + "" }).ToList();
                    ViewBag.BreadCrumbs = breadcrumbs;

                    var attr = Cache.CatAttribute.OrderByDescending(d => d.CategoryId).FirstOrDefault(d => katyol.Select(k => k.Id).Contains(d.CategoryId));
                    if (attr != null && !cat.IsOtoKuafor)
                    {
                        var attributes = Cache.CatAttribute.FindAll(d => d.CategoryId == attr.CategoryId).Select(d =>
                           new Input
                           {
                               EndText = d.Attribute.AttributeEndText,
                               Text = d.Attribute.Name,
                               SubInputs = Cache.CatSubAttribute.
                           FindAll(s => d.AttributeId == s.AttributeId && s.CategoryId == attr.CategoryId && s.SubAttribute.ValueNumber <= 0 && s.SubAttribute.Value != null).Select(p => new Input { Text = p.SubAttribute.Value, Value = p.SubAttribute.Id + "", EndText = p.M_Attribute.AttributeEndText }).Distinct().ToList()
                           }).ToList();

                        var orderList = EClassField.Core.OrderList.GetOrderTip().FindAll(d => d.KatID.Contains(attr.CategoryId));

                        ViewBag.OrderList = orderList;

                        ViewBag.Attributes = attributes;

                        using (ClassFieldDbContext ctx = new ClassFieldDbContext())
                        {

                            //var ppp = ctx.Set<Product>().ToList();
                            //foreach (var item in ppp)
                            //{

                            //    Neighborhood n = ctx.Set<Neighborhood>().FirstOrDefault(x => x.ILADI == item.IL && x.TownName == item.ILCE && x.Name == item.MAHALLE.Replace("Mh.", ""));
                            //    if (n != null)
                            //    {
                            //        Product pp = ctx.Set<Product>().FirstOrDefault(x => x.Id == item.Id);
                            //        pp.MAHALLEKOD = n.Id;
                            //    }

                            //}
                            //ctx.SaveChanges();



                            var products = ctx.Set<Product>().Where(d => d.IsActive && (d.ProductCategories.Select(p => p.CategoryId).Contains(cat.Id) && !d.ProductCategories.Any(x => x.Category.IsFun)));
                            if (username != -1)
                            {

                                products = products.Where(d => d.User.Id == username);

                                User u = ctx.Set<User>().FirstOrDefault(d => d.Id == username);
                                ViewBag.user = u;

                                ViewBag.userimage = u.UserImages.FirstOrDefault(d => d.Picture.IsVitrin);
                            }





                            foreach (var item in Request.QueryString.AllKeys)
                            {
                                if (String.IsNullOrEmpty(Request.QueryString[item].ToString()))
                                    continue;

                                var attibut = Cache.CatSubAttribute.Find(d => Function.GetStringFormatText(d.M_Attribute.Name) == Function.GetStringFormatText(item));
                                if (attibut != null || item == "price")
                                {
                                    if (item == "price" || (attibut.SubAttribute.ValueNumber > 0 && attibut.SubAttribute.Value == null))
                                    {
                                        decimal[] between = Request.QueryString[item].Split(',').Where(d => d != "").Select(d => Convert.ToDecimal(d)).ToArray();
                                        if (between.Length >= 2)
                                        {
                                            decimal min = between[0];
                                            decimal max = between[1];

                                            if (item == "price")
                                            {
                                                products = products.Where(d => d.Price >= min && d.Price <= max);
                                            }
                                            else
                                            {

                                                var doublemin = Convert.ToDouble(min);
                                                var doublemax = Convert.ToDouble(max);
                                                //products = products.
                                                //    Where(d => d.ProductAttributes.Where(a =>   a.SubAttribute.ValueNumber >= min && a.SubAttribute.ValueNumber <= max).Any());

                                                products = products.Join(ctx.Set<ProductAttributeValueNumber>(),p=>p.Id,pv=>pv.ProductId,(p,pv)=> new {

                                                    Productt=p,
                                                    ValueNumber=pv.Value,
                                                    AttrName=pv.M_Attribute.Name
                                                }).Where(x=> x.ValueNumber>=doublemin && x.ValueNumber<=doublemax).Select(x=>x.Productt);



                                            }
                                        }
                                    }
                                    else
                                    {
                                        int[] attrsids = Request.QueryString[item].Split(',').Where(d => d != "").Select(d => Convert.ToInt32(d)).ToArray();

                                        if (attrsids.Any())
                                        {
                                            products = products.Where(d => d.ProductAttributes.Select(s => s.SubAttributeId).Any(x => attrsids.Contains(x)));
                                        }


                                    }

                                }

                            }

                            if (neighborhood != null)
                            {
                                products = products.Where(d => neighborhood.Contains(d.MAHALLEKOD));
                            }
                            else if (town != null)
                            {
                                products = products.Where(d => town.Contains(d.ILCEKOD));
                            }
                            else if (city != null)
                            {
                                products = products.Where(d => city.Contains(d.ILKOD));
                            }



                            ViewBag.foundCount = products.Count();


                            PageModel pagemodel = new PageModel();
                            pagemodel.TotalCount = ViewBag.foundCount;
                            pagemodel.PageSize = 20;
                            pagemodel.PageIndex = pageID;

                            ViewBag.PageModel = pagemodel;



                            if (!(String.IsNullOrEmpty(fullorder)))
                            {
                                Ordertip tip = OrderList.GetOrderTip().Find(d => d.OrderIndex == Convert.ToInt32(fullorder));


                                PropertyInfo tiporder = new Product().GetType().GetProperty(tip.Orderby);

                                if (tiporder != null)
                                {

                                    if (tip.OrderType == "desc")
                                    {
                                        products = products.OrderBy(tip.Orderby + " descending");
                                    }
                                    else
                                    {
                                        products = products.OrderBy(tip.Orderby);
                                    }
                                }
                                else
                                {
                                    if (tip.OrderType == "desc")
                                    {
                                        products = products.OrderBy(d => d.ProductAttributes.Where(dd => dd.Attribute.Name == tip.Orderby).Select(p => p.SubAttribute.ValueNumber).Max());
                                    }
                                    else
                                    {
                                        products = products.OrderBy(d => d.ProductAttributes.Where(dd => dd.Attribute.Name == tip.Orderby).Select(p => p.SubAttribute.ValueNumber).Max());
                                    }
                                }
                            }
                            else
                            {
                                products = products.OrderByDescending(d => d.CreationTime);
                            }
                            var result = products.Skip((pageID - 1) * 20).Take(20).ToList().Select(d => new ProductModelView
                            {


                                Id = d.Id,
                                Categories = d.ProductCategories != null ? d.ProductCategories.Where(x=>x.Category!=null).Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList() : null,
                                Attribute = d.ProductAttributes != null ? d.ProductAttributes.Where(x=>x.SubAttribute!=null).Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon }).ToList() : null,
                                City = d.City == null ? "" : d.City.Name,
                                Date = d.CreationTime.ToShortDateString() + "",
                                FullLoc = d.IL + " / " + d.ILCE ,
                                Image =d.Image==null?   Function.GetPictureImage(d.ProductPictures.Any() ? d.ProductPictures.FirstOrDefault().Picture.FileName : "",true):Function.GetPictureImage(d.Image,true,true),
                                Price = d.Price + "",
                                Town = d.Town != null ? d.Town.Name : "",
                                Title = d.Title,
                                Description = d.Description ?? "Acıklama, Bulunmuyor",
                                Datte = d.CreationTime,
                                HREF= d.Image == null ? Function.GetPictureImage(d.ProductPictures.Any() ? d.ProductPictures.FirstOrDefault().Picture.FileName : "", true) : Function.GetPictureImage(d.Image, true, true),

                            }).ToList();





                            ViewBagHazirla();

                            ViewBag.Products = result;
                        }

                    }

                    return View("~/Views/Partial/_ilanList.cshtml");
                }
            }


            return View();
        }


        public ActionResult Detail(string detail, int id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                //d.User.IsAdmin?new User {Name=d.UserName,EvTel=d.Tel1,IsTel=d.Tel2 }: d.User
                var product = ctx.Set<Product>().Where(d => d.Id == id && d.IsActive).ToList().Select(d => new ProductModelView
                {


                    Id = d.Id,
                    Categories = d.ProductCategories.Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList(),
                    Attribute = d.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? string.Format("{0:N}", Convert.ToDouble(a.SubAttribute.ValueNumber) + "".Split(',')[0] + " " + a.Attribute.AttributeEndText), Icon = a.Attribute.Icon }).ToList(),
                    City = d.IL,
                    Date = d.CreationTime.ToShortDateString() + "",
                    FullLoc = d.IL + "/" + d.ILCE + "/" + d.MAHALLE,
                    Image = Function.GetPictureImage(d.ProductPictures.Any() ? d.ProductPictures.FirstOrDefault().Picture.FileName : "",false, false),
                    Price = d.Price + " ",
                    Town = d.ILCE,
                    Title = d.Title,
                    Images = d.ProductPictures.Select(p => new Input { Text =Function.GetPictureImage(p.Picture.FileName,true,false), Value = p.Picture.Id + "" }).ToList(),

                    Description = d.Description,
                    User = d.UserName != null ? new EClassField.Core.Domain.User.User { EvTel = d.Tel1, Cep = d.Tel2, Name = d.UserName } : d.User,
         
                    UserImages = d.User.UserImages.Select(p => new Input { Value = p.Picture.FileName }).ToList()



                }).FirstOrDefault();


                if (product != null)
                {
                    ViewBag.Product = product;


                    ViewBag.BreadCrumbs = new List<Input>();
                    var categoryId = product.Categories.OrderByDescending(x => Convert.ToInt32(x.Value)).FirstOrDefault().Value;
                    var katyol = Function.GetCategoryPath(Convert.ToInt32(categoryId));

                    var attributes = GetValues(ctx.Set<ProductAttribute>().Where(x => x.ProductId == product.Id).ToList(), ctx.Set<ProductAttributeValue>().Where(x => x.ProductId == product.Id).ToList(), ctx.Set<ProductAttributeValueNumber>().Where(x => x.ProductId == product.Id).ToList());
                    ViewBag.attributes = attributes;
                    var breadcrumbs = katyol.Select(d => new Input { Text = d.Name, Value = d.Id + "" }).ToList();
                    ViewBag.BreadCrumbs = breadcrumbs;

                    ViewBag.Title = katyol.Select(d => d.Name).Aggregate((a, b) => a + " / " + b) + Function.GetStringFormatText(product.Title) + " - " + product.Id;

                    ViewBag.Desc = ViewBag.Title;
                }

            }
            ViewBagHazirla();
            return View();
        }


        public dynamic GetValues(List<ProductAttribute> attrids, List<ProductAttributeValue> texts, List<ProductAttributeValueNumber> numberrange)
        {

            List<LabelInput> x = new List<LabelInput>();

            attrids.ForEach(d =>
            {

                x.Add(new LabelInput
                {


                    Name = d.Attribute.Name,
                    Value = d.SubAttribute.Value
                });
            });

            texts.ForEach(d =>
            {

                x.Add(new LabelInput
                {

                    Name = d.M_Attribute.Name,
                    Value = d.Value
                });
            });
            numberrange.ForEach(d =>
            {

                x.Add(new LabelInput
                {

                    Name = d.M_Attribute.Name,
                    Value = Convert.ToInt64(string.Format("{0:N}", d.Value).Split(',')[0].Replace(".", "")) + ""
                });
            });




            return x;
        }

        [HttpPost]
        public string Upload(string base64, string productID, bool vitrin = true)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                int producttID = Convert.ToInt32(productID);

                string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                var bytes = Convert.FromBase64String(base64);
                using (var imageFile = new FileStream(Server.MapPath("~/Content/Site/ProductImages/" + guid + ".jpg"), FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }


                ctx.Set<ProductPicture>().Add(new ProductPicture
                {

                    ProductId = producttID,
                    Picture = new EClassField.Core.Domain.Media.Picture { FileName = guid + ".jpg", MimeType = "image/jpeg", IsVitrin = vitrin }
                });
                ctx.SaveChanges();




            }

            return "ok";

        }


        public ActionResult YardimDestek()
        {
            ViewBag.Title = "otomarket.com Yardım ve Destek Merkezi";
            return View();
        }


        public ActionResult Hakkimizda()
        {
            ViewBag.Title = "otomarket.com Hakkımızda";
            return View();
        }


        public ActionResult iletisim()
        {
            ViewBag.Title = "Bize Ulaşın";
            return View();
        }

        public ActionResult NasilParaKazanirim()
        {
            ViewBag.Title = "Nasıl Para Kazanılır";
            return View();
        }
    }

}