using EClassField.Core;
using EClassField.Core.Domain.Attribute;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Directory;
using EClassField.Core.Domain.OneSignal;
using EClassField.Core.Domain.Rating;
using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace EClassFieldWeb_.Controllers
{
    public class IlanVerController : Controller
    {
        // GET: IlanVer
        public ActionResult Index(int ilanID = -1, int catID = 16)
        {

            var userr = General.User;
            ViewBag.IsAdmin = userr.IsAdmin;
            if (userr == null)

                return Redirect("/Login");

            if (userr.IsActive == false)
                return Redirect("/SignUp/CeptenOnay/?userID=" + userr.Id);


            string[] emails = { "otomarketdestek@gmail.com", "cdeveloperr@gmail.com", "shnduman@gmail.com" };

            if (emails.Contains(userr.Email))
            {
                ViewBag.isface = true;
            }
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (ilanID != -1)
                {
                    var ilan = ctx.Set<Product>().Where(d => d.Id == ilanID && (d.User.Email == (userr.Email) || (emails.Contains(userr.Email)))).FirstOrDefault();
                    if (ilanID != -1)
                    {


                        var ilanres = new EClassFieldWeb_.Models.ViewModel.ProductModelView
                        {



                            Id = ilan.Id,
                            Categories = ilan.ProductCategories.Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList(),
                            Attribute = ilan.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Id + "", Icon = a.Attribute.Icon, EndText = a.Attribute.AttributeEndText }).ToList(),
                            City = Function.FullLoc(ilan.City, null, null, null),
                            Date = ilan.CreationTime.ToShortDateString() + "",
                            FullLoc = Function.FullLoc(ilan.City, ilan.Town, ilan.Area, ilan.Neighborhood),
                            Image = ilan.ProductPictures.Any() ? ilan.ProductPictures.LastOrDefault().Picture.FileName : null,
                            Price = ilan.Price + "",
                            Town = Function.FullLoc(null, ilan.Town, null, null),
                            Title = ilan.Title,
                            Images = ilan.ProductPictures.Select(i => new Input { Value = i.Picture.FileName }).ToList(),
                            ValueAttribute = ilan.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name + "", Value = a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon, EndText = a.Attribute.AttributeEndText }).ToList(),
                            CityId = ilan.City != null ? ilan.City.Id : -1,
                            TownId = ilan.Town != null ? ilan.Town.Id : -1,
                            AreaId = ilan.Area != null ? ilan.Area.Id : -1,
                            NeigboardId = ilan.Neighborhood != null ? ilan.Neighborhood.Id : -1,
                            Latude = ilan.Latitude,
                            Longtude = ilan.Longitude,
                            Description = ilan.Description,
                            ILKOD = ilan.ILKOD,
                            ILCEKOD = ilan.ILCEKOD,
                            MAHALLEKOD = ilan.MAHALLEKOD,
                            User = new User { Name = ilan.UserName, EvTel = ilan.Tel1, IsTel = ilan.Tel2 },
                            Link = ilan.Link


                        };

                        if (ilanres != null)
                        {
                            ViewBag.ilanID = ilanres.Id;
                            ViewBag.ilan = ilanres;
                        }
                        var attributes = ilan.ProductAttributes.Select(d => new AttributeModel { AttributeId = d.AttributeId, SubAttributeId = d.SubAttributeId, Name = d.Attribute.Name }).ToList();

                        var category = ilan.ProductCategories.LastOrDefault(d => d.Product.ProductAttributes.Any());

                        ViewBag.SubAttributeIDS = ilanres.Attribute.Select(d => d.Value).ToList();

                        var attrsproducts = ilan.ProductAttributes.Select(x => new AttributeModel { AttributeId = x.AttributeId, SubAttributeId = x.SubAttributeId, Text = x.SubAttribute.Value, Name = x.Attribute.Name }).ToList();

                        ViewBag.attrList = attrsproducts;


                        var cats = Function.GetCategoryPath(Convert.ToInt32(ilanres.Categories.LastOrDefault().Value));

                        ViewBag.Category = new Input { Text = Function.getCategoriesPathString(Convert.ToInt32(ilanres.Categories.LastOrDefault().Value)), Value = cats[cats.Count - 1].Id + "" };
                        var cat = cats.LastOrDefault(d => d.SubAttributes.Any());
                        if (cat != null)
                        {
                            var attrs = Cache.CatAttribute.Where(d => d.CategoryId == cat.Id).
                                Select(d => new Input
                                {
                                    Value = AttributeValueText(d.AttributeId, ilanID, ctx),
                                    AttributeId = d.AttributeId,

                                    EndText = d.Attribute.AttributeEndText,
                                    Text = d.Attribute.Name,
                                    Type = d.Attribute.AttributeType,
                                    SubInputs = Cache.CatSubAttribute.Where(s => s.M_Attribute.AttributeType == AttributeType.DropDown && s.CategoryId == cat.Id && d.AttributeId == s.AttributeId)
                                .Select(c => new Input { Text = c.SubAttribute.Value, Value = c.SubAttributeId + "", EndText = d.Attribute.AttributeEndText, Type = c.M_Attribute.AttributeType }).ToList()
                                });

                            ViewBag.Attributes = attrs.ToList();

                            ViewBag.catID = cat.Id;
                            ViewBag.ilan = ilanres;
                        }
                    }
                    else if (catID != -1)
                    {

                        var cats = Function.GetCategoryPath(catID);
                        var cat = cats.LastOrDefault(d => d.SubAttributes.Any());
                        var attrs = Cache.CatAttribute.Where(d => d.CategoryId == cat.Id).
                            Select(d => new Input { Text = d.Attribute.Name, SubInputs = Cache.CatSubAttribute.Where(s => s.M_Attribute.AttributeType == AttributeType.DropDown && s.CategoryId == cat.Id && d.AttributeId == s.AttributeId).Select(c => new Input { Text = c.SubAttribute.Value, Value = c.SubAttributeId + "" }).ToList() }).ToList();

                        ViewBag.Attributes = attrs;
                    }

                }
                return View();
            }

        }

        public string AttributeValueText(int attributeId, int productId, ClassFieldDbContext ctx)
        {
            var attr = ctx.Set<ProductAttributeValue>().FirstOrDefault(x => x.AttributeId == attributeId && x.ProductId == productId);

            var attrvalue = ctx.Set<ProductAttributeValueNumber>().FirstOrDefault(x => x.AttributeId == attributeId && x.ProductId == productId);
            if (attr != null)
            {
                return attr.Value;
            }
            if (attrvalue != null)

                return attrvalue.Value + "";

            return "";


        }

        public PartialViewResult GetAttributePartial(int categoryID, bool isFill = false, int productID = -1)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {


                var cats = Function.GetCategoryPath(categoryID);
                var cat = cats.LastOrDefault(d => d.SubAttributes.Any());








                //var productList = ctx.Set<Product>().Where(d => d.ProductCategories.FirstOrDefault(c => c.CategoryId == categoryID) != null);

                //if (General.User.Email != "shnduman@gmail.com")
                //{

                //    if (productList != null && productList.Any())
                //    {
                //        if (!isFill)
                //        {


                //            if (cat != null)
                //            {
                //                var attrs = Cache.CatAttribute.Where(d => d.CategoryId == cat.Id).
                //                    Select(d =>


                //                    new Input
                //                    {
                //                        AttributeId = d.AttributeId,
                //                        Type = d.Attribute.AttributeType,
                //                        EndText = d.Attribute.AttributeEndText,
                //                        Text = d.Attribute.Name,
                //                        SubAttributeId = Cache.CatSubAttribute.FirstOrDefault(s => s.AttributeId == d.AttributeId) != null ? Cache.CatSubAttribute.FirstOrDefault(s => s.AttributeId == d.AttributeId).SubAttributeId : 0,
                //                        SubInputs = Cache.CatSubAttribute.Where(s => s.CategoryId == cat.Id && d.AttributeId == s.AttributeId && d.Attribute.AttributeType == AttributeType.DropDown).Select(c => new Input
                //                        {
                //                            Text = c.SubAttribute.Value,
                //                            Value = c.SubAttributeId + "",
                //                            EndText = c.M_Attribute.AttributeEndText
                //                        }).ToList()
                //                    });
                //                ViewBag.attrs = attrs;


                //            }


                //            return PartialView("~/Views/Partial/_GetDoluDetayList.cshtml", productList.Take(5).ToList().Select(d => new ProductModelView
                //            {


                //                Id = d.Id,
                //                Attribute = d.ProductAttributes != null ? d.ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon, EndText = a.Attribute.AttributeEndText }).ToList() : null,


                //            }).ToList());

                //        }
                //    }


                //    ViewBag.productList = !productList.Any() ? null : productList.FirstOrDefault(d => d.Id == productID).ProductAttributes.Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.Value, Icon = a.Attribute.Icon, EndText = a.Attribute.AttributeEndText }).ToList();

                //}






                if (cat != null)
                {
                    var attrs = Cache.CatAttribute.Where(d => d.CategoryId == cat.Id).
                        Select(d =>


                        new Input
                        {
                            AttributeId = d.AttributeId,
                            Type = d.Attribute.AttributeType,
                            EndText = d.Attribute.AttributeEndText,
                            Text = d.Attribute.Name,
                            SubAttributeId = Cache.CatSubAttribute.FirstOrDefault(s => s.AttributeId == d.AttributeId) != null ? Cache.CatSubAttribute.FirstOrDefault(s => s.AttributeId == d.AttributeId).SubAttributeId : 0,
                            SubInputs = Cache.CatSubAttribute.Where(s => s.CategoryId == cat.Id && d.AttributeId == s.AttributeId && d.Attribute.AttributeType == AttributeType.DropDown).Select(c => new Input
                            {
                                Text = c.SubAttribute.Value,
                                Value = c.SubAttributeId + "",
                                EndText = c.M_Attribute.AttributeEndText
                            }).ToList()
                        });


                    return PartialView("~/Views/Partial/_AttributePartial.cshtml", attrs.ToList());

                }


            }
            return null;

        }

        public JsonResult DeleteProduct(int productID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var item = ctx.Set<Product>().FirstOrDefault(x => x.Id == productID);

                bool isian = false;
                var cat = item.ProductCategories.FirstOrDefault(x => x.Category.IsIlan);
                List<string> images = item.ProductPictures.Select(x => x.Picture.FileName).ToList();
                if (cat != null)
                {
                    var productCategory = ctx.Set<ProductCategory>().Where(x => x.ProductId == item.Id);
                    var productattribute = ctx.Set<ProductAttribute>().Where(x => x.ProductId == item.Id);
                    var productattributeValue = ctx.Set<ProductAttributeValue>().Where(x => x.ProductId == item.Id);
                    var productattributeValueNumber = ctx.Set<ProductAttributeValueNumber>().Where(x => x.ProductId == item.Id);
                    var productFavori = ctx.Set<ProductFavori>().Where(x => x.ProductId == item.Id);
                    var productPicture = ctx.Set<ProductPicture>().Where(x => x.ProductId == item.Id);


                    ctx.Set<ProductCategory>().RemoveRange(productCategory);
                    ctx.Set<ProductAttribute>().RemoveRange(productattribute);
                    ctx.Set<ProductAttributeValue>().RemoveRange(productattributeValue);
                    ctx.Set<ProductAttributeValueNumber>().RemoveRange(productattributeValueNumber);
                    ctx.Set<ProductFavori>().RemoveRange(productFavori);
                    ctx.Set<ProductPicture>().RemoveRange(productPicture);



                    foreach (var item2 in images)
                    {

                        AmazonS3.DeleteFile("otonomide", item2);
                    }



                }


                ctx.Set<Product>().Remove(item);
                ctx.SaveChanges();


                return null;
            }



        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection col, List<AttributeModel> attributes, int catID = -1, int ilanID = -1, Location loc = null, decimal price = 0, string UserName = null, string Tel1 = null, string Tel2 = null, string Link = null)
        {
            string[] values = Request.Form[1].Split('&');

            int userID = General.User.Id;

            Product product = null;
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {









                //foreach (var item in values)
                //{
                //    string name = Function.GetStringFormatText(item.Split('=')[0]);
                //    var attr = Cache.CatAttribute.FirstOrDefault(d => Function.GetStringFormatText(d.Attribute.Name) == name);

                //    string text = item.Split('=')[1];

                //    string item2 = item.Replace("%2C", ",").Split(',')[0];
                //    if (attr != null)
                //    {
                //        int valueID = 0;
                //        try
                //        {
                //            valueID = Convert.ToInt32(item2.Split('=')[1]);
                //            if (valueID != -1)
                //            {
                //                int subAttrValue = -1;
                //                CategorySubAttribute subattr = null;
                //                if (attr.Attribute.AttributeType == AttributeType.DropDown)
                //                {
                //                    subattr = ctx.Set<CategorySubAttribute>().FirstOrDefault(d => d.SubAttributeId == valueID);
                //                    if (subattr == null)
                //                    {
                //                        var catsubattr = new CategorySubAttribute { AttributeId = attr.AttributeId, SubAttribute = new SubAttribute { Value = null, ValueNumber = valueID }, CategoryId = attr.CategoryId };
                //                        ctx.Set<CategorySubAttribute>().Add(catsubattr);
                //                        ctx.SaveChanges();
                //                    }

                //                }
                //                else
                //                {
                //                    subattr = ctx.Set<CategorySubAttribute>().FirstOrDefault(d => d.SubAttribute.ValueNumber == valueID);


                //                    if (subattr == null)
                //                    {

                //                        var catsubattr = new CategorySubAttribute { AttributeId = attr.AttributeId, SubAttribute = new SubAttribute { Value = null, ValueNumber = valueID }, CategoryId = attr.CategoryId };
                //                        ctx.Set<CategorySubAttribute>().Add(catsubattr);
                //                        ctx.SaveChanges();
                //                    }

                //                }
                //            }
                //        }
                //        catch {

                //            var catsubattr = new CategorySubAttribute { AttributeId = attr.AttributeId, SubAttribute = new SubAttribute { Value = text, ValueNumber = -1}, CategoryId = attr.CategoryId };
                //            ctx.Set<CategorySubAttribute>().Add(catsubattr);
                //            ctx.SaveChanges();
                //        }

                //    }

                //}



                int ID = ilanID;
                if (ID > -1)
                {
                    product = ctx.Set<Product>().FirstOrDefault(d => d.Id == ID);
                    ctx.Set<ProductCategory>().RemoveRange(product.ProductCategories);
                    ctx.Set<ProductAttribute>().RemoveRange(product.ProductAttributes);
                    ctx.Set<ProductPicture>().RemoveRange(product.ProductPictures);

                }
                else
                {
                    product = new Product();

                    product.ProductAttributes = new List<ProductAttribute>();
                    product.ProductCategories = new List<ProductCategory>();
                    product.ProductPictures = new List<ProductPicture>();


                }
                //var categories = Function.getKategoriYol(catID, "IsIlan", false).FirstOrDefault(x => x.SubAttributes.Any());

                if (attributes != null && attributes.Any())
                {



                    foreach (var item in attributes)
                    {
                        if (item.SubAttributeId <= -1 || item.AttributeId <= -1)
                            continue;


                        var attribute = ctx.Set<M_Attribute>().FirstOrDefault(d => d.Id == item.AttributeId);
                        if (attribute.AttributeType == AttributeType.Text || attribute.AttributeType == AttributeType.Range)
                        {

                            double d = 0;
                            Double.TryParse(item.Text, out d);

                            if (d <= 0)
                            {
                                ProductAttributeValue proattrvalue2 = ctx.Set<ProductAttributeValue>().FirstOrDefault(da => da.ProductId == product.Id && da.AttributeId == item.AttributeId);
                                if (proattrvalue2 != null)
                                {
                                    proattrvalue2.Value = item.Text;
                                }
                                else
                                {

                                    ProductAttributeValue proattrvalue = new ProductAttributeValue { AttributeId = item.AttributeId, ProductId = product.Id, Value = item.Text };
                                    ctx.Set<ProductAttributeValue>().Add(proattrvalue);


                                }
                            }
                            else
                            {
                                ProductAttributeValueNumber proattrvalue2 = ctx.Set<ProductAttributeValueNumber>().FirstOrDefault(dd => dd.ProductId == product.Id && dd.AttributeId == item.AttributeId);
                                if (proattrvalue2 != null)
                                {
                                    proattrvalue2.Value = Convert.ToDouble(item.Text);
                                }
                                else
                                {

                                    ProductAttributeValueNumber proattrvalue = new ProductAttributeValueNumber { AttributeId = item.AttributeId, ProductId = product.Id, Value = Convert.ToDouble(item.Text) };
                                    ctx.Set<ProductAttributeValueNumber>().Add(proattrvalue);


                                }
                            }

                        }
                        else if (attribute.AttributeType == AttributeType.DropDown)
                        {

                            SubAttribute subattr = ctx.Set<SubAttribute>().FirstOrDefault(d => d.Id == item.SubAttributeId);

                            product.ProductAttributes.Add(new ProductAttribute { AttributeId = attribute.Id, SubAttributeId = subattr.Id, ProductId = product.Id });
                        }



                    }


                }


                product.VideoLink = col["VideoLink"];
                product.UserName = UserName;
                product.Tel1 = Tel1;
                product.Tel2 = Tel2;
                product.Link = Link;
                product.Title = col["Title"];
                product.CreationTime = DateTime.Now;
                product.UpdateTme = DateTime.Now;
                product.Price = price;
                product.Description = col["Desc"];
                product.FaceLink = col["FaceLink"];
                int cityID = loc.CityID;
                int townID = loc.TownID;
                int areaID = loc.AreaID;
                int neigboardID = loc.NeigborhoodID;
                product.City = ctx.Set<City>().FirstOrDefault(d => d.CityId == cityID);
                product.Town = ctx.Set<Town>().FirstOrDefault(d => d.TownId == townID);

                product.Neighborhood = ctx.Set<Neighborhood>().FirstOrDefault(d => d.Id == neigboardID);

                product.ILKOD = cityID;
                product.ILCEKOD = townID;
                product.MAHALLEKOD = neigboardID;
                //product.Latitude = col["Latidue"].ToString();
                //product.Longitude = col["Longtude"].ToString();
                if (product.City != null)
                {
                    product.IL = product.City.Name;
                }
                if (product.Town != null)
                {
                    product.ILCE = product.Town.Name;
                }
                if (product.Neighborhood != null)
                {
                    product.MAHALLE = product.Neighborhood.Name;
                }
                var cats = Function.getKategoriYol(catID, "IsIlan", false);
                foreach (var item in cats)
                {
                    product.ProductCategories.Add(new ProductCategory { CategoryId = item.Id, ProductId = product.Id });
                }

                string[] files = col["Files"].Split(';');


                foreach (var item in files)
                {
                    if (item.Length >= 10)
                    {

                        product.ProductPictures.Add(new ProductPicture { Picture = new EClassField.Core.Domain.Media.Picture { FileName = item, MimeType = "image/jpeg" }, ProductId = product.Id });
                    }
                }


                if (product.Id <= 0)
                {

                    product.User = ctx.Set<User>().FirstOrDefault(d => d.Id == userID);
                    ctx.Set<ProductUser>().Add(new ProductUser { ProductId = product.Id, UserId = userID });

                    ctx.Set<Product>().Add(product);
                }

                if (cats.Any(x => x.IsFun))
                {
                    product.IsActive = true;
                }
                else
                {
                    product.IsActive = false;
                }
                product.IsPending = true;

                if (userID == 2040)
                {
                    product.IsOrnek = true;
                }


                try
                {
                    string address = product.IL + " " + product.ILCE + " " + product.MAHALLE;
                    string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

                    WebRequest request = WebRequest.Create(requestUri);
                    WebResponse response = request.GetResponse();
                    XDocument xdoc = XDocument.Load(response.GetResponseStream());

                    XElement result = xdoc.Element("GeocodeResponse").Element("result");
                    XElement locationElement = result.Element("geometry").Element("location");
                    XElement latt = locationElement.Element("lat");
                    XElement lngg = locationElement.Element("lng");

                    string name = $"POINT({latt.Value + " " + lngg.Value})";

                    //product.Location = DbGeography.FromText(name);
                }
                catch (Exception ex)
                {


                }


                ctx.SaveChanges();






                var player = ctx.Set<Player>().FirstOrDefault(d => d.Id == 9);
                if (player != null)
                {
                    OneSignalAPI.SendMessage("Yeni Bir İlan Eklendi", new List<string>() { player.PlayerID });
                }

                KazancAralik aralik = KazancList.GetKazancAralik().FirstOrDefault(d => d.KazancTipi == KazancType.IlanVerme);
                double many = Function.RandomNumberBetween((double)aralik.Min, (double)aralik.Max);

                ctx.Set<Kazanc>().Add(new Kazanc { ProductId = product.Id, IsActive = false, CreationTime = DateTime.Now, KazancMiktar = (decimal)many, KazancTipi = KazancType.IlanVerme, UserID = userID });

                ctx.SaveChanges();
            }




            return View();
        }

        public static string DoFormatDecimal(decimal myNumber)
        {
            try
            {
                if (myNumber >= 1000 && myNumber <= 9999)
                {
                    return string.Format("{0:N}", myNumber).Split(',')[0].Replace(".", "");
                }

                return string.Format("{0:N}", myNumber).Split(',')[0];

            }
            catch (Exception)
            {

                return "Belirtilmemiş";
            }

        }

        public string GetValueFormCollection(string title, string[] values)
        {
            var str = values.FirstOrDefault(d => d.Split('=')[0] == title);
            return str != null ? str.Split('=')[1] ?? "-1" : "-1";
        }


        [HttpPost]
        public string ResimUpload()
        {

            string images = "";
            int userID = 2040;
            foreach (string item in Request.Files)
            {

                HttpPostedFileBase file = Request.Files[item];
                var guid = Guid.NewGuid().ToString().Substring(0, 10).Replace("-", "") + userID;
                //AmazonS3.UploadFile(file.InputStream, guid);
                images += guid + ".jpg,";
                string image = guid + ".jpg";



                image = file.FileName;
               
                
                //file.SaveAs(Server.MapPath("~/Content/Site/ProductImages/" + guid + ".jpg"));

                MemoryStream ms = GetImage(file.InputStream, 1600, 1200, 100);


                // AmazonS3.UploadFile(ms, image);


                file.SaveAs(Server.MapPath("~/Content/imagespreview/" + image));


            }
            if (images != "")
                images = images.Substring(0, images.Length - 1);



            return images;
        }

        public MemoryStream GetImage(Stream rtn, int width = 480, int height = 360, int quality = 50, bool isface = false)
        {




            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = quality };
            Size size = new Size(width, height);

            Bitmap bmp2 = new Bitmap(rtn);
            if (bmp2.Height > bmp2.Width)
            {
                size = new Size(height, width);
            }

            MemoryStream ms2 = new MemoryStream();
            bmp2.Save(ms2, ImageFormat.Jpeg);

            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    if (isface)
                    {
                        ISupportedImageFormat formatt = new JpegFormat { Quality = 100 };
                        imageFactory.Load(bmp2)

               .Format(formatt)
               .BackgroundColor(Color.White)

               .Save(outStream);
                    }
                    else
                    {
                        imageFactory.Load(bmp2)
               .Resize(size)
               .Format(format)
               .BackgroundColor(Color.White)
               .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
               .Save(outStream);
                    }
                    // Load, resize, set the format and quality and save an image.




                    Bitmap bmp = new Bitmap(outStream);



                    //bmp = Yaziyaz(bmp, width, height, "", "otomarket.com");

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);

                    return ms;
                }
                // Do something with the stream.


            }


        }
        public ActionResult GetImage(string image, int width = 480, int height = 360, int quality = 50)
        {

            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/ilanimagespreview/" + image;
            Stream rtn = null;
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(aURL);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
            rtn = aResponse.GetResponseStream();

            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = quality };
            Size size = new Size(width, height);





            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory.Load(rtn)
                                .Resize(new ResizeLayer(resizeMode: ResizeMode.Crop, size: size))
                                .Format(format)
                                .BackgroundColor(Color.White)
                                .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
                                .Save(outStream);



                    Bitmap bmp = new Bitmap(outStream);

                    bmp = Yaziyaz(bmp, width, height, "", "otomarket.com");

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);
                    //bmp.Save(ms, ImageFormat.Jpeg);
                    return File(ms.GetBuffer(), "image/jpeg");
                    //bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
                // Do something with t}he stream.
            }




            return View();
        }
        public Bitmap Yaziyaz(Bitmap resim, int genislik, int yukseklik, string yol, string watermark)
        {

            //resmin boyutu bizim vermiş olduğumuz genişlik veya yükseklikten büyükse boyutlandırma yapıyoruz.
            if (resim.Width > genislik || resim.Height > yukseklik)
            {
                Size ebatlar = new Size(resim.Width, resim.Height);
                //resmin genişlik ve yükseklik oranını alıyoruz.
                double oran = ((double)resim.Width / (double)resim.Height);
                if (resim.Width > genislik && genislik > 0)
                {//burada genişlik parametresi 0 olarak verilmişse boyutlandırma yapılmayacak. Yani resim orijinal genişliğinde kalacak.
                    ebatlar.Width = genislik;
                    ebatlar.Height = (int)((double)genislik / oran);
                }
                if (ebatlar.Height > yukseklik && yukseklik > 0)
                {//burada yükseklik parametresi 0 olarak verilmişse boyutlandırma yapılmayacak. Yani resim orijinal yükseklikte kalacak.
                    ebatlar.Height = yukseklik;
                    ebatlar.Width = (int)((double)yukseklik * oran);
                }
                resim = new Bitmap(resim, ebatlar);
            }

            //resmin üzerine yazı yazmak istemeyebiliriz o yüzden “watermark” parametresine boş string verebiliriz.
            if (!string.IsNullOrEmpty(watermark))
            {
                Graphics graf = Graphics.FromImage(resim);
                //resmin şeffaflık (alpha) değeri ve renk değerleri belirleniyor.
                SolidBrush firca = new SolidBrush(Color.FromArgb(80, 192, 192, 192));

                //resmin köşegen uzunluğu pisagor denklemiyle hesaplanıyor.
                double kosegen = Math.Sqrt(resim.Width * resim.Width + resim.Height * resim.Height);
                Rectangle kutu = new Rectangle();

                //bu 3 satırda ise yazının başlama noktası (x,y koordinatları) ve ayrıca font boyutu ayarlanıyor.
                //bunun için aşağıdaki gibi yaklaşık değerler kullandım 1,3..... 1,6.... gibi siz bu rakamlarla oynama yapabilirsiniz.
                kutu.X = (int)(kosegen / 3);
                float yazi = (float)(kosegen / watermark.Length * 0.6);
                kutu.Y = -(int)(yazi / 1);

                Font fnt = new Font("times new roman", yazi, FontStyle.Bold);//font tipi ve boyutu       
                                                                             //can alıcı noktamız burası
                                                                             //burada köşegen eğimini aşağıdaki formülle hesaplıyoruz.
                float egim = (float)(Math.Atan2(resim.Height, resim.Width) * 180 / System.Math.PI);
                graf.RotateTransform(egim);
                StringFormat sf = new StringFormat();
                // ve nihayet watermarkımızı resim üzerine yazdırıyoruz.
                graf.DrawString(watermark, fnt, firca, kutu, sf);
            }

            return resim;
        }


        public ActionResult GetImageResize(Stream stream, int width = 480, int height = 360, int quality = 50)
        {

            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/ilanimages/" + "image";

            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = quality };
            Size size = new Size(width, height);





            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory.Load(stream)
                                .Resize(new ResizeLayer(resizeMode: ResizeMode.Crop, size: size))
                                .Format(format)
                                .BackgroundColor(Color.White)
                                .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
                                .Save(outStream);



                    Bitmap bmp = new Bitmap(outStream);

                    bmp = Yaziyaz(bmp, width, height, "", "otomarket.com");

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);
                    //bmp.Save(ms, ImageFormat.Jpeg);
                    return File(ms.GetBuffer(), "image/jpeg");
                    //bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
                // Do something with the stream.


            }

            return View();
        }


        public void DeleteImage(string filename)
        {

            string fullPath = Server.MapPath("~/Content/Site/ProductImages/" + filename);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }


        }
    }
}