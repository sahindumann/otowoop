using EClassField.Core.Domain.Blog;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class PostIndexController : Controller
    {
        // GET: PostIndex
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPostDetail(int id = 0)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var post = ctx.Set<Post>().Select(d => new {

                    Title = d.Title,
                    CreationTime = d.CreationTime,
                    Desc = d.Description,
                    Id = d.Id,
                    Image = d.PostPictures.Select(p => p.Picture.FileName).FirstOrDefault(),


                }).FirstOrDefault(d => d.Id == id);


                ViewBag.post = post;
                return View();
            }
        }
    }
}