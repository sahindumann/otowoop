using EClassField.API.Models;
using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class UploadikiController : ApiController
    {
        [HttpPost]
        public IHttpActionResult UploadImage(Input id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                int productID = -1;
                bool vitrin = false;
                if (Function.getValue(id.Value, "ProductID") != "")
                {
                    productID = Convert.ToInt32(Function.getValue(id.Value, "ProductID"));

                }


                //string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
                //var bytes = Convert.FromBase64String(base64);
                //using (var imageFile = new FileStream(Server.MapPath("~/Content/productImages/" + guid + ".jpg"), FileMode.Create))
                //{
                //    imageFile.Write(bytes, 0, bytes.Length);
                //    imageFile.Flush();
                //}

                var imagess = ctx.Set<ProductPicture>().Where(d => d.ProductId == productID).ToList();


                ctx.Set<ProductPicture>().RemoveRange(imagess);
                ctx.SaveChanges();

                string[] imgs = Function.getValue(id.Value, "Images").Split(',').Where(d => d != "").ToArray();
                List<ProductPicture> pictures = new List<ProductPicture>();
                foreach (var item2 in imagess)
                {
                    if (!imgs.Contains(item2.Picture.FileName))
                    {
                        pictures.Add(item2);
                    }

                }
                foreach (var item in imgs)
                {
                 

                    ctx.Set<ProductPicture>().Add(new ProductPicture
                    {

                        ProductId = productID,
                        Picture = new Core.Domain.Media.Picture { FileName = item , MimeType = "image/jpeg", IsVitrin = vitrin }
                    });



                }
           
                ctx.SaveChanges();




                return Ok(new { Ok = "" });
            }



        }
    }
}
