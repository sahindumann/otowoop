using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassField.API.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public string Upload(string base64, string productID,bool vitrin=true)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                int producttID = Convert.ToInt32(productID);
   
                string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                var bytes = Convert.FromBase64String(base64);
                using (var imageFile = new FileStream(Server.MapPath("~/Content/productImages/" + guid + ".jpg"), FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                

                ctx.Set<ProductPicture>().Add(new ProductPicture
                {

                    ProductId = producttID,
                    Picture = new Core.Domain.Media.Picture { FileName = guid + ".jpg", MimeType = "image/jpeg",IsVitrin=vitrin }
                });
                ctx.SaveChanges(); 

                   
               

            }

            return "ok";
          
        }
    }
}