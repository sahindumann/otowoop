using EClassField.Core.Domain.Blog;
using EClassField.Core.Domain.Galerry;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EClassField.API.Controllers
{
    public class PostController : ApiController
    {
        public string GetImageRandomStart()
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                Slider s = ctx.Set<Slider>().FirstOrDefault(d => d.IsAktif);
                return "https://www.otowoop.com/" + s.Image;
            }

        }


        public IHttpActionResult GetPosts(int categoryID, string aranan = "", string OrderTip = "asc", int PageID = 1)
        {
            aranan = aranan.ToLower();
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var post = ctx.Set<Post>().Where(d => d.PostCategories.Select(a => a.CategoryId).Contains(categoryID) && (!(string.IsNullOrEmpty(aranan)) ?


                  d.MetaDescription.ToLower().Contains(aranan) ||
                  d.Title.ToLower().Contains(aranan) ||
                  d.Description.ToLower().Contains(aranan)






                  : true)).OrderBy(d => d.CreationTime).Skip((PageID - 1) * 5).Take(5);


                return Ok(post.Select(d => new
                {
                    Id=d.Id,
                    Title = d.Title,
                    Descrition = d.Description,
                    Categories = d.PostCategories.Select(p => new { Name = p.Category.Name, Id = p.CategoryId }),
                    Resimler = d.PostPictures.Select(p => p.Picture.FileName),
                    Image = d.PostPictures.Select(p => p.Picture.FileName).FirstOrDefault(),
                    Tags = d.PostTags.Select(t => t.Tag.Name)


                }).ToList());

            }
        }
        public IHttpActionResult GetPostDetail(int postID)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var post = ctx.Set<Post>().Select(d => new {

                    Title = d.Title,
                    CreationTime = d.CreationTime,
                    Desc = d.Description,
                    Id=d.Id,
                    Image = d.PostPictures.Select(p => p.Picture.FileName).FirstOrDefault(),


                }).FirstOrDefault(d=>d.Id==postID);

                return Ok(post);
            }
        }
    }





}
