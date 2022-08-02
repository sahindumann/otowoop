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

namespace EClassField.Image.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }



        public ActionResult GetImageProfile(string image, bool isVitrin)
        {
            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/profilemages/" + image;
            Stream rtn = null;
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(aURL);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
            rtn = aResponse.GetResponseStream();

            ISupportedImageFormat format = new JpegFormat { Quality = 100 };
            Size size = new Size(200, 200);

            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory.Load(rtn)
                                .Resize(new ResizeLayer(resizeMode: ResizeMode.Max, size: size))
                                .Format(format)
                                .BackgroundColor(Color.White)

                                .Save(outStream);



                    Bitmap bmp = new Bitmap(outStream);


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

        public ActionResult GetImage(string image, int width = 480, int height = 360, int quality = 50, bool noimage = false, string text = "otowoop.com", bool watermark = true, string aurl = "", bool thumb = false,bool full=false)
        {

            

            string aURL = "https://s3-us-west-2.amazonaws.com/otonomide/ilanimages/" + image;
            //string aURL = "http://d3mq5e3ap3pjg3.cloudfront.net/ilanimages/" + image;
            if (!String.IsNullOrEmpty(aurl))
            {
                aURL = aurl.Replace("http://image6.otowoop.com/Image/GetImage/?aurl=","http://www.otowoop.com/content/imagespreview/");
               
            }

            Stream rtn = null;
            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(aURL);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();
            rtn = aResponse.GetResponseStream();

            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = quality };


            Size size = new Size(width, height);

    
            Bitmap bmmp = new Bitmap(rtn);

            if (full)
                size = new Size(bmmp.Width, bmmp.Height);


            if (size.Height > size.Width)
            {
                

                size = new Size(size.Height, size.Width);
             

            }
            else
            {

                size = new Size(bmmp.Width, bmmp.Height);
          

            }

            if (thumb)
            {
                if (bmmp.Width > bmmp.Height)
                    size = new Size(width, height);
                else
                    size = new Size(height, width);

            }


        
                Graphics gr = Graphics.FromImage(bmmp);
                Bitmap logo = new Bitmap(Server.MapPath("~/Content/imagelogo.png"));
                Rectangle rect = new Rectangle(bmmp.Width - (logo.Width-95), bmmp.Height - (logo.Height-95), 135, 135);
                gr.FillRectangle(new SolidBrush(Color.Black), rect);

                gr.DrawImage(logo, rect);

                gr.Dispose();
            


            using (MemoryStream outStream = new MemoryStream())
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {

                    return CreateThumbnail(bmmp, width, height, 100, null, null, null);

                    // Load, resize, set the format and quality and save an image.
                    if (thumb)
                    {
                        //imageFactory.Load(bmmp)
                        //            .Resize(new ResizeLayer(resizeMode: ResizeMode.Max, size: size))
                        //            .Format(format)
                        //            .BackgroundColor(Color.Transparent)
                        //            //.Crop(new ImageProcessor.Imaging.CropLayer(0, 0, 0, 0, ImageProcessor.Imaging.CropMode.Percentage))
                        //            .Save(outStream);

                        noimage = true;


                        return CreateThumbnail(bmmp, size.Width, size.Height,10, null, null, null);
                    }


                    if (!noimage)
                    {
                        Bitmap bmp = bmmp;
                        if (size.Width >= 110 || size.Height>=110)
                        {
                            if (watermark)
                                bmp = Yaziyaz(bmp, size.Width, size.Height, "", text);
                        }
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, ImageFormat.Jpeg);

                        return File(ms.GetBuffer(), "image/jpeg");
                    }
                    else
                    {
                        return File(outStream.GetBuffer(), "image/jpeg");
                    }
                    //bmp.Save(ms, ImageFormat.Jpeg);

                    //bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
                // Do something with the stream.


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


        public ActionResult CreateThumbnail(System.Drawing.Image image, int width, int height, int quality, string path = "", HttpServerUtilityBase ctx = null, string id = "")
        {


            float scaleHeight = (float)width / (float)image.Height;
            float scaleWidth = (float)height / (float)image.Width;
            float scale = Math.Min(scaleHeight, scaleWidth);

            Bitmap bmp2 = null;
            // the resized result bitmap
            using (Bitmap result = new Bitmap((int)(image.Width * scale), (int)(image.Height * scale)))
            {
                // get the graphics and draw the passed image to the result bitmap
                using (Graphics grphs = Graphics.FromImage(result))
                {
                    grphs.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    grphs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grphs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    grphs.DrawImage(image, 0, 0, (int)(image.Width * scale), (int)(image.Height * scale));
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



                //result.Dispose();


                string imagestr = id;
                MemoryStream ms = new MemoryStream();
                result.Save(ms, ImageFormat.Jpeg);

                return File(ms.GetBuffer(), "image/jpeg");
            }




        }
    }
}