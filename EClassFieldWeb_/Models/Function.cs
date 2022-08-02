using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Directory;
using EClassField.Core.Domain.User;
using EClassField.Data;
using EClassField.Services.Catalog;
using EClassFieldWeb_.Models.ViewModel;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using MesajPaneli.Business;
using MesajPaneli.Models;
using MesajPaneli.Models.JsonPostModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Models
{
    public class Function

    {






        public static double RandomNumberBetween( double minValue, double maxValue)
        {
            Random random = new Random();

            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        public static bool MesajGonder(string tel, User user, ClassFieldDbContext ctx, string message)
        {



            List<string> telList = new List<string>();
            telList.Add(tel);


            smsData MesajPaneli = new smsData();

            MesajPaneli.user = new UserInfo("5394432937", "9qwljl");
            MesajPaneli.msgBaslik = "otomarket.com";
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


        public static List<ProductModelView> GetProducts(int cat)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var products = ctx.Set<Product>().Where(d => d.IsActive && (d.ProductCategories.Select(p => p.CategoryId).Contains(cat) && !d.ProductCategories.Any(x => x.Category.IsFun))).OrderByDescending(f=>f.CreationTime).Take(12).ToList();




                return products.Select(d => new ProductModelView
                {


                    Id = d.Id,
                    Categories = d.ProductCategories != null ? d.ProductCategories.Where(c=>c.Category!=null).Select(c => new Input { Text = c.Category.Name, Value = c.CategoryId + "" }).ToList() : null,
                    Attribute = d.ProductAttributes != null ? d.ProductAttributes.Where(x=>x.SubAttribute!=null).Select(a => new Input { Text = a.Attribute.Name, Value = a.SubAttribute.Value ?? a.SubAttribute.ValueNumber + "", Icon = a.Attribute.Icon }).ToList() : null,
                    City = d.City == null ? "" : d.City.Name,
                    Date = d.CreationTime.ToShortDateString() + "",
                    FullLoc = Function.FullLoc(d.City, d.Town, null, null).Replace("/", ""),
                    Image = d.Image == null ? Function.GetPictureImage(d.ProductPictures.Any() ? d.ProductPictures.FirstOrDefault().Picture.FileName : "", true) : Function.GetPictureImage(d.Image, true, true),
                    Price = d.Price + "",
                    Town = d.Town != null ? d.Town.Name : "",
                    Title = d.Title,
                    Description = d.Description ?? "Acıklama Bulunmuyor",
                    Datte = d.CreationTime

                }).ToList();



            }
        }

        public static string GetPictureImage(string url,bool thumb=false,bool local=false)
        {

            if (!string.IsNullOrEmpty(url))
            {

                //if (url.Contains("i0.shbdn.com"))
                //{
                //    return url;

                //}
                //else if (url.Contains("araba.s3"))
                //{
                //    return url;
                //}
                ////else

                //if (!local)
                //{
                //    if (!thumb)
                //        return "http://Image6.otomarket.com/Image/GetImage/?aurl=" + url + "&width=800&height=600&thumb=true";
                //    else
                //        return "http://Image6.otomarket.com/Image/GetImage/?aurl=" + url + "&width=110&height=72&thumb=true";
                //}
                //else
                //{
                //    return "/Content/imagespreview/" + url;
                //}
                return "/Content/imagespreview/" + url;
                //}

            }
            return "/Content/imagelogo.png";

        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetStringTelephone(string number)
        {
            if (!String.IsNullOrEmpty(number))
            {
                number = number.Replace(System.Environment.NewLine, "").Replace(" ", "");

                return "0 " + String.Format("{0:(###) ###-####}", double.Parse(number));
            }
            else
                return "";


        }
        public static List<Category> getKategoriYol(int ID, string categorytype = "IsIlan", bool typee = true)
        {




            ClassFieldDbContext ctx = new ClassFieldDbContext();
            //&& (bool)d.GetType().GetProperty(categorytype).GetValue(d) == typee
            List<Category> cats = new List<Category>();
            Category cat = Cache.Categories.Find(d => d.Id == ID /*&& d.IsActive == true*/);
            if (cat != null)
            {
                cats.Add(cat);
                while (cat != null)
                {
                    cat = Cache.Categories.Find(d => d.Id == cat.ParentCategoryId);
                    if (cat != null)
                    {
                        cats.Add(cat);
                    }
                }

            }


            cats = cats.OrderBy(d => d.Id).ToList();


            return cats;

        }

        public static string getValue(string fullstr, string name)

        {
            string[] inputs = fullstr.Split(';');
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].Split('=')[0].ToLower() == name.ToLower())
                    return inputs[i].Split('=')[1] + "";

            }

            return "";


        }


        public static string FullLoc(City city, Town town, Area area, Neighborhood neighboard)
        {
            string loc = "";
            if (city != null)
                loc += city.Name;
            if (town != null)
                loc += " / " + town.Name;
            if (area != null)
                loc += " / " + area.Name;
            if (neighboard != null)
                loc += " / " + neighboard.Name;

            return loc;

        }

        public static string getCategoriesPathString(int catID, string delimiter = "/")
        {

            if (catID >= 1)
            {
                var kats = getKategoriYol(catID);
                if (kats.Any())
                {
                    var cats = kats.Select(d => d.Name).Aggregate((a, b) => a + delimiter + b);

                    return cats;
                }
            }
            return "";
        }
        public static List<Category> GetCategoryPath(int catID)
        {
            CategoryService categoryservis = new CategoryService();

            var cats = categoryservis.GetSubCategories(catID);

            return cats;
        }

        public static List<Category> getSubCategoriesBlog(int parentID = 0)

        {
            CategoryService categoryservis = new CategoryService();


            List<Category> cats = new List<Category>();
            cats = categoryservis.GetList(d => d.IsBlog && d.ParentCategoryId == parentID).ToList();

            if (cats != null && cats.Count <= 0)
            {
                var cats2 = categoryservis.GetList(d => d.IsBlog).ToList();
                if (cats2.Count >= 1)
                {
                    int parentIDD = cats2.OrderBy(d => d.ParentCategoryId).FirstOrDefault().Id;

                    var category = categoryservis.GetById(d => d.Id == parentIDD);

                    cats.Add(category);
                }
            }


            return cats;
        }
        public static List<Category> getSubCategoriesJson(int parentID)
        {
            CategoryService categoryservis = new CategoryService();


            List<Category> cats = new List<Category>();
            if (parentID >= 1)
                cats = categoryservis.GetList(d => d.ParentCategoryId == parentID && d.IsBlog).ToList();
            else
                cats = categoryservis.GetList(d => d.IsBlog).ToList();

            return cats.Select(d => new Category { Name = d.Name, Id = d.Id, ParentCategoryId = d.ParentCategoryId }).ToList();
        }




        public static List<SelectListItem> getcombokategoriler()
        {
            CategoryService categoryservis = new CategoryService();
            var cats = categoryservis.GetList(d => d.IsBlog).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            int bosluk = 2;

            foreach (var item in cats)
            {
                bosluk = 2;
                var cats2 = cats.FindAll(d => d.ParentCategoryId == item.Id && d.IsBlog).ToList();
                if (cats2.Count >= 1)
                {
                    if (!items.Select(d => d.Value).Contains(item.Id + ""))
                    {

                        items.Add(new SelectListItem { Value = item.Id + "", Text = item.Name.PadLeft(item.Name.Length) });
                    }

                    getcombokategorilerSub(cats2, ref items, ref bosluk, cats);
                }

                else
                {
                    if (!items.Select(d => d.Value).Contains(item.Id + ""))
                    {

                        items.Add(new SelectListItem { Value = item.Id + "", Text = item.Name });
                    }

                }
            }
            return items;

        }
        public static void getcombokategorilerSub(List<Category> cats, ref List<SelectListItem> items, ref int bosluk, List<Category> catss, bool alt = false)
        {
            CategoryService categoryservis = new CategoryService();

            foreach (var item in cats)
            {
                if (alt == false)
                    bosluk = 2;

                var cats2 = catss.FindAll(d => d.ParentCategoryId == item.Id && d.IsBlog).ToList();
                if (cats2.Count >= 1)
                {

                    bosluk += 5;
                    if (!items.Select(d => d.Value).Contains(item.Id + ""))
                    {

                        items.Add(new SelectListItem { Value = item.Id + "", Text = item.Name.PadLeft(item.Name.Length + bosluk, ' ') });
                    }
                    getcombokategorilerSub(cats2, ref items, ref bosluk, catss, true);
                }

                else
                {

                    if (!items.Select(d => d.Value).Contains(item.Id + ""))
                    {
                        bosluk += 5;
                        if (!items.Select(d => d.Value).Contains(item.Id + ""))
                        {

                            items.Add(new SelectListItem { Value = item.Id + "", Text = item.Name.PadLeft(item.Name.Length + bosluk, ' ') });
                        }
                    }


                }
            }


        }

        public static string GetStringFormatText(string a)
        {
            if (String.IsNullOrEmpty(a))
                a = "";

            string[] charecters = { ",", ".", "ç", "ı", "ö", "ş", "ü", " ", "/", "\\", ">", "<", "ç", "&","+" };
            string[] charecters2 = { "-", "-", "c", "i", "o", "s", "u", "-", "-", "-", "-", "-", "-", "-","plus" };
            int index = 0;
            foreach (var item in charecters)
            {
                a = a.ToLower().Replace(charecters[index], charecters2[index]);


                index++;
            }

            a = a.Replace("--", "-").Replace("---", "-").Replace("----", "-").Replace("\n","");

            return a;

        }

        public static string GetStringFormatTextSearchCategory(string a)
        {



            if (String.IsNullOrEmpty(a))
                a = "";

            a = GetStringFormatText(a);

            string[] charecters = { ",", "-", ".", "ç", "ı", "ö", "ş", "ü", " ", "/", "\\", ">", "<", "ç", "&" };

            int index = 0;
            foreach (var item in charecters)
            {
                a = a.ToLower().Replace(charecters[index], "");


                index++;
            }

            a = a.Replace("--", "-").Replace("---", "-").Replace("----", "-");

            return a;

        }


        public static string GetStringFormatTextKategoriUrl(string a)
        {
            if (String.IsNullOrEmpty(a))
                a = "";

            string[] charecters = { ",", ".", " ", "ı", "ş", "ğ", "ö", "ü", " ", "/", "\\", ">", "<", "ç" };
            a = a.ToLower();
            foreach (var item in charecters)
            {
                a = a.Replace(item, "");
            }



            return a;

        }

        public static string MD5Sifrele(string metin)
        {
            // MD5CryptoServiceProvider nesnenin yeni bir instance'sını oluşturalım.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Girilen veriyi bir byte dizisine dönüştürelim ve hash hesaplamasını yapalım.
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            //byte'ları biriktirmek için yeni bir StringBuilder ve string oluşturalım.
            StringBuilder sb = new StringBuilder();


            //hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım.
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürelim.
            return sb.ToString();
        }


        public Bitmap GetImage(string image, int width = 480, int height = 360, int quality = 50, bool isface = false)
        {

            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/ilanimages/" + image;
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
                    if (isface)
                    {
                        ISupportedImageFormat formatt = new JpegFormat { Quality = 100 };
                        imageFactory.Load(rtn)

               .Format(formatt)
               .BackgroundColor(Color.White)

               .Save(outStream);
                    }
                    else
                    {
                        imageFactory.Load(rtn)
               .Resize(size)
               .Format(format)
               .BackgroundColor(Color.White)
               .Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
               .Save(outStream);
                    }
                    // Load, resize, set the format and quality and save an image.




                    Bitmap bmp = new Bitmap(outStream);

                    bmp = Yaziyaz(bmp, width, height, "", "otomarket.com");

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Jpeg);
                    //bmp.Save(ms, ImageFormat.Jpeg);
                    //return File(ms.GetBuffer(), "image/jpeg");
                    return bmp;
                }
                // Do something with the stream.


            }

            return null;
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

        public  void CreateThumbnail(System.Drawing.Image image, int width, int height, int quality,string path="",HttpServerUtilityBase ctx=null,string id="")
        {
            Bitmap bmp2 = null;
            // the resized result bitmap
            using (Bitmap result = new Bitmap(width, height))
            {
                // get the graphics and draw the passed image to the result bitmap
                using (Graphics grphs = Graphics.FromImage(result))
                {
                    grphs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    grphs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grphs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    grphs.DrawImage(image, 0, 0, result.Width, result.Height);
                }

                // check the quality passed in
                if ((quality < 0) || (quality > 100))
                {
                    string error = string.Format("quality must be 0, 100", quality);
                    throw new ArgumentOutOfRangeException(error);
                }

                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                string lookupKey = "image/jpeg";
                var jpegCodec = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();

                //create a collection of EncoderParameters and set the quality parameter
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;
                //save the image using the codec and the encoder parameter





                string imagestr = id;
                result.Save(ctx.MapPath("~/Content/"+path+"/" + imagestr), ImageFormat.Jpeg);
            }

       


        }
    }
}