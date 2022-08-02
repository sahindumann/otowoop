using EClassField.Core.Domain.Directory;
using EClassField.Data;
using EClassFieldWeb_.Models;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class BaseController : Controller
    {

        public JsonResult GetCategories(string text, bool isnumber = false)
        {
            JsonResult result = null;

            var user = General.User;
            

            if (text.Length >= 3)
            {

                if (user!=null &&user.Email=="shnduman@gmail.com")
                {
                    text = Function.GetStringFormatTextKategoriUrl(text);
                    result = new JsonResult()
                    {

                        Data = Cache.Categories.Where(d => Function.GetStringFormatTextKategoriUrl(d.FullPath).Contains(text)).Select(d => new { text = d.FullPath, id = isnumber ? d.Id + "" : Function.GetStringFormatText(d.FullPath) }).ToList(),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    text = Function.GetStringFormatTextKategoriUrl(text);
                    result = new JsonResult()
                    {

                        Data = Cache.Categories.Where(d =>  d.IsIlan&& Function.GetStringFormatTextKategoriUrl(d.FullPath).Contains(text)).Select(d => new { text = d.FullPath, id = isnumber ? d.Id + "" : Function.GetStringFormatText(d.FullPath) }).ToList(),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }

            return result;


        }


        public JsonResult GetCategoriesServis(string text, bool isnumber = false)
        {
            JsonResult result = null;


            if (text.Length >= 3)
            {

                text = Function.GetStringFormatTextKategoriUrl(text);
                result = new JsonResult()
                {

                    Data = Cache.Categories.Where(d => Function.GetStringFormatTextKategoriUrl(d.FullPath).Contains(text)).Select(d => new { text = d.Name, id = isnumber ? d.Id + "" : d.Id + "" }).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }

            return result;


        }



        public ActionResult GetImage(string image, int width = 480, int height = 360)
        {
            string fullPath = Server.MapPath("~/Content/Site/ProductImages/" + image);
            if (System.IO.File.Exists(fullPath))
            {


                // Format is automatically detected though can be changed.
                ISupportedImageFormat format = new JpegFormat { Quality = 70 };
                Size size = new Size(width, height);

                byte[] photoBytes = System.IO.File.ReadAllBytes(fullPath);


                using (MemoryStream inStream = new MemoryStream(photoBytes))
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            // Load, resize, set the format and quality and save an image.
                            imageFactory.Load(inStream)
                                        .Resize(size)
                                        .Format(format)
                                        .BackgroundColor(Color.Black)
                                        .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
                                        .Save(outStream);



                            Bitmap bmp = new Bitmap(outStream);

                            Yaziyaz(bmp, width, height, "", "otomarket.com");


                            MemoryStream ms = new MemoryStream();
                            bmp.Save(ms, ImageFormat.Jpeg);
                            return File(ms.GetBuffer(), "image/jpeg");
                            //bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
                        }
                        // Do something with the stream.
                    }
                }
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
        public JsonResult GetLocation(int ID, int typeID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {


                switch (typeID)
                {
                    case 4:
                        return Json(ctx.Set<Neighborhood>().Where(d => d.TownId == ID).Select(d => new { Name = d.Name, Value = d.Id }).OrderBy(d=>d.Name).ToList(), JsonRequestBehavior.AllowGet);
                    case 8:
                        return Json(ctx.Set<Area>().Where(d => d.TownId == ID).Select(d => new { Name = d.Name, Value = d.Id }).ToList(), JsonRequestBehavior.AllowGet);
                    case 3:
                        return Json(ctx.Set<Town>().Where(d => d.CityId == ID).Select(d => new { Name = d.Name, Value = d.TownId }).OrderBy(d => d.Name).ToList(), JsonRequestBehavior.AllowGet);
                    case 2:
                        return Json(ctx.Set<City>().Select(d => new { Name = d.Name, Value = d.Id }).OrderBy(d=>d.Name).ToList(), JsonRequestBehavior.AllowGet);
                    default:

                        return Json(ctx.Set<City>().Select(d => new { Name = d.Name, Value = d.CityId }).OrderBy(x=>x.Name).ToList().OrderBy(d=>d.Name).ToList(), JsonRequestBehavior.AllowGet);

                }

            }

        }


        public JsonResult GetLocationType(Input model)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {   var ids = model.Value.Split(',').Where(d => d != "").Select(d => Convert.ToInt32(d));
                if (model.Text == "City")
                {



                    var result = ctx.Set<Town>().Where(d => ids.Contains(d.CityId)).Select(d => new { Value = d.TownId, Text = d.Name }).OrderBy(x=>x.Text).ToList();

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (model.Text == "Town")
                {

                    var result = ctx.Set<Neighborhood>().Where(d => ids.Contains(d.TownId)).Select(d => new { Value = d.Id, Text = d.Name }).OrderBy(x => x.Text).ToList();

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return null;


            }

        }
    }

}
