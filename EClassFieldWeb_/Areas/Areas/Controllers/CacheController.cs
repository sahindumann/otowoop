using EClassField.Core;
using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    public class CacheController : Controller
    {
        // GET: Areas/Cache
        public void Index()
        {


        }

        public void IlanListPrepera()
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var products = ctx.Set<Product>().Where(x => x.IsActive == false || x.IsPending == false).ToList();
                foreach (var item in products)
                {
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


                }

                ctx.SaveChanges();


            }
        }

        public void ImagePreviewSave(int _width,int _height,string _path)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                _path = "imagespreview";
                string[] array = { "sahibinden", "araba", "otonomi" };
                var products = ctx.Set<Product>().Where(x=>x.Image==null&& x.Link!=null && array.Any(y=>x.Link.Contains(y))).ToList();
                var imagesProducts = ctx.Set<ProductPicture>().ToList();
                var productCategories= ctx.Set<ProductCategory>().ToList();
                foreach (var item in products)
                {
                    bool isian = false;
                    var cat = productCategories.FindAll(x => x.ProductId == item.Id).FirstOrDefault(x => x.Category.IsIlan);
                    List<string> images = imagesProducts.FindAll(x => x.ProductId == item.Id).Select(x => x.Picture.FileName).ToList();
              

                        string image = images.FirstOrDefault();

          

                    if (image != null)
                        {
                            if (image.Contains("http"))
                            {

                            try
                            {
                                WebClient cli = new WebClient();

                                byte[] data = cli.DownloadData(image);

                                MemoryStream ms = new MemoryStream(data);
                                Bitmap bmp = new Bitmap(ms);

                                int width = 0;
                                int height = 0;
                                if (bmp.Width > bmp.Height)
                                {
                                    width = _width;
                                    height = _height;
                                }
                                else
                                {
                                    width = _height;
                                    height = _width;
                                }

                             


                                Graphics gr = Graphics.FromImage(bmp);
                                Bitmap logo = new Bitmap(Server.MapPath("~/Content/imagelogo.png"));
                                Rectangle rect = new Rectangle(bmp.Width - (logo.Width - 95), bmp.Height - (logo.Height - 95), 135, 135);
                                gr.FillRectangle(new SolidBrush(Color.Black), rect);

                                gr.DrawImage(logo, rect);

                                gr.Dispose();


                                item.Image = "image_" + item.Id + ".jpg";
                                new EClassFieldWeb_.Models.Function().CreateThumbnail(bmp, width, height, 100, _path, HttpContext.Server, item.Image);

                            }
                            catch 
                            {
                                item.IsActive = false;
                              
                            }
                             
                              
                    

                            }
                    }
                

                }

                ctx.SaveChanges();     
            }

        }


    }
}