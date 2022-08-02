using EClassField.API.Models;
using EClassField.Core.Domain.Comment;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class CommentController : ApiController
    {

        public IHttpActionResult GetProductComment(int ProductID = 0)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var comments = ctx.Set<ProductComment>().Where(d => d.Product.Id == ProductID).ToList();
                var result = comments.Select(d => new { Title = d.Comment.Title, User = d.User.Name, Image = d.User.UserImages.ToList().FirstOrDefault() != null ? d.User.UserImages.FirstOrDefault().Picture.FileName : null,Zaman=Function.GetTimeDiffrent(d.Comment.CreationTime,DateTime.Now) }).ToList();


                return Ok(result);
            }
        }

        [HttpPost]
        public IHttpActionResult SendProduct(CommentModel id)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                Comment cmt = new Comment();
                cmt.Title = id.Text;
                cmt.CreationTime = DateTime.Now;
                cmt.UpdateTime = DateTime.Now;
                ProductComment cmtp = ctx.Set<ProductComment>().Add(new ProductComment { Comment = cmt, UserID = id.UserID, ProductId = id.ProductID });
                ctx.SaveChanges();

                return Ok("");
            }
        }


        public class CommentModel
        {
            public string Text { get; set; }
            public int UserID { get; set; }
            public int ProductID { get; set; }
        }
    }
}
