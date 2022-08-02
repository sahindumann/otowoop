using EClassField.Core.Domain.Blog;
using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Controllers
{
    public class BlogController : Controller
    {


        public void ViewBagHazirla()
        {
            
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                var userr = General.User;
                if (userr != null)
                {
                    var user = ctx.Set<EClassField.Core.Domain.User.User>().FirstOrDefault(d => d.Id == userr.Id);

                    if (user != null)
                    {


                        var image = user.UserImages.ToList().Find(d => d.Picture.IsVitrin);
                        if (image != null)
                        {
                            ViewBag.ProfileImage = image != null ? "http://image6.otomarket.com/Image/GetImageProfile/?image=" + image.Picture.FileName + "&isVitrin=true" : "https://s3-us-west-2.amazonaws.com/otonomide/profilemages/placeholder_logo.jpg";
                        }

                    }
                }
            }
        }
        // GET: Blog
        public ActionResult Index(string id, int pageID = 1)
        {
            using (EClassField.Data.ClassFieldDbContext ctx = new EClassField.Data.ClassFieldDbContext())
            {
                id = Function.GetStringFormatTextSearchCategory(id);
                Category cat = Cache.Categories.Find(d => d.IsBlog && Function.GetStringFormatTextSearchCategory(d.FullPath).Contains(id));
                var katyol = Function.GetCategoryPath(cat.Id);

                ViewBag.categories = ctx.Set<Category>().Where(d => d.ParentCategoryId == 20418).Select(p => new Input { Text = p.Name, Value = p.Name }).ToList();

                var breadcrumbs = katyol.Select(d => new Input { Text = d.Name, Value = d.Id + "" }).ToList();

                var cats = katyol.Select(d => d.Id).ToList();
                ViewBag.BreadCrumbs = breadcrumbs;


                var posts = ctx.Set<Post>().Where(d =>d.IsActive&& d.PostCategories.Select(pc => pc.Category.Id).Any(p => p == cat.Id)).OrderByDescending(d => d.CreationTime).Skip((pageID - 1) * 8).Take(8).Select(d => new PostViewModel
                {
                    Id = d.Id,
                    Title = d.Title,
                    CreationTime = d.CreationTime,
                    Deleted = d.Deleted,
                    Description = d.Description,
                    MetaDescription = d.MetaDescription,
                    IsActive = d.IsActive,
                    MetaKeywords = d.MetaKeywords,
                    MetaTitle = d.MetaTitle,
                    Pictures = d.PostPictures.Select(p => new Input { Value = p.Picture.FileName }).ToList(),
                    Tags = d.PostTags.Select(t => new Input { Value = t.Tag.Name }).ToList()



                }).ToList();


                var last
                 = ctx.Set<Post>().Where(d => d.PostCategories.Select(pc => pc.Category.Id).Any(p => cats.Contains(p))).OrderByDescending(d => d.CreationTime).Take(4).Select(d => new PostViewModel
                 {
                     Id = d.Id,
                     Title = d.Title,
                     CreationTime = d.CreationTime,
                     Deleted = d.Deleted,
                     Description = d.Description,
                     MetaDescription = d.MetaDescription,
                     IsActive = d.IsActive,
                     MetaKeywords = d.MetaKeywords,
                     MetaTitle = d.MetaTitle,
                     Pictures = d.PostPictures.Select(p => new Input { Value = p.Picture.FileName }).ToList(),
                     Tags = d.PostTags.Select(t => new Input { Value = t.Tag.Name }).ToList()



                 })


                    .ToList();

                ViewBag.lastposts = last;


                ViewBagHazirla();
                return View(posts);
            }


        }


        public ActionResult Detail(string title, int id)
        {
            using (EClassField.Data.ClassFieldDbContext ctx = new EClassField.Data.ClassFieldDbContext())
            {
               

                var post = ctx.Set<Post>().Where(d => d.Id == id).ToList().Select(
                    d => new PostViewModel
                    {
                        Id=d.Id,
                        Title = d.Title,
                        CreationTime = d.CreationTime,
                        Deleted = d.Deleted,
                        Description = d.Description,
                        MetaDescription = d.MetaDescription,
                        IsActive = d.IsActive,
                        MetaKeywords = d.MetaKeywords,
                        MetaTitle = d.MetaTitle,
                        Pictures = d.PostPictures.Select(p => new Input { Value = p.Picture.FileName }).ToList(),
                        Tags = d.PostTags.Select(t => new Input { Value = t.Tag.Name }).ToList(),
                        Categories=d.PostCategories.Select(p => new Input { Text = p.Category.Name, Value = p.Category.Id+"" }).ToList()

            }

                    ).FirstOrDefault();




                ViewBag.categories = post.Categories;

                var breadcrumbs = post.Categories;

                var cats = post.Categories;
                ViewBag.BreadCrumbs = breadcrumbs;
                ViewBagHazirla();
                return View(post);
            }


        }
    }
}