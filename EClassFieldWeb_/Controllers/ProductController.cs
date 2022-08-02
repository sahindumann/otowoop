using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using ImageProcessor;
using ImageProcessor.Imaging;
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
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }



        public void A()
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
             


            }
        }

        [HttpGet]
        public string ActiveIlan(int productID, int userID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                Product product = ctx.Set<Product>().FirstOrDefault(d => d.Id == productID && d.User.IsAdmin);
                if (product != null)
                {

                    if (userID == 1)
                    {
                        product.IsActive = product.IsActive ? false : true;
                    }
                    product.IsPending = product.IsActive ? false : true;


                    if (product.ProductPictures != null &&product.IsActive)
                    {
                        string image = product.ProductPictures.FirstOrDefault().Picture.FileName;


                        Bitmap bmp = GetImage(image, 200, 150, 70, product.FaceLink != null);
                        Bitmap bmp2 = GetImage(image, 480, 360, 100, product.FaceLink != null);


                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, ImageFormat.Jpeg);
                        //if (AmazonS3.GetFile("otonomide", "ilanthumb/" + image) == null)
                        ////{
                        //AmazonS3.UploadFile(ms, image, "ilanthumb/");


                        //AmazonS3.UploadFile(ms, image, "ilanthumb/");

                        bmp.Save(Server.MapPath("~/content/imagethumb/" + image));
                        bmp2.Save(Server.MapPath("~/content/imagespreview/" + image));








                        //}
                    }
                    else
                    {

                    }

                    ctx.SaveChanges();




                    return "1";
                }
                return "-1";


            }


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
                 .Resize(new ResizeLayer(resizeMode: ResizeMode.Pad, size: size))
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
    }
}