using ClassFieldWeb_.Models;
using EClassField.Core;
using EClassField.Core.Domain.Blog;
using EClassField.Core.Domain.Locazition;
using EClassField.Core.Domain.Tags;
using EClassField.Data;
using EClassField.Services.Blog;
using EClassField.Services.Catalog;
using EClassField.Services.Languages;
using EClassFieldWeb_.Models;
using EClassFieldWeb_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class PostController : Controller
    {

        PostTagService posttagservice = new PostTagService();
        PostLangugeService postlanguageservices = new PostLangugeService();
        EClassField.Services.Catalog.LanguageService languageservice = new EClassField.Services.Catalog.LanguageService();
        PostService postservice = new PostService();
        // GET: Areas/Post
        public ActionResult Index(int postID = 0)
        {
            Post p = new Post();
            if (postID > 0)
            {
                p = postservice.GetById(d => d.Id == postID);
            }
            else
            {
                List<PostLanguage> postlanguages = new List<PostLanguage>();
                foreach (var item in languageservice.GetTable().ToList())
                {
                    postlanguages.Add(new PostLanguage { Language = item });
                }

                p = new Post { PostLanguages = postlanguages };
                p.PostTags = new List<PostTag>();
                p.PostPictures = new List<PostPicture>();
                

            }

            return View(p);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Index(FormCollection col)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
                try
                {

                    string title = col["Title_tr"];
                    string desc = col["Description_tr"];

                    Language lan = languageservice.GetById(d => d.SeoName == "tr");

                    Post p = new Post();

                    if (col["Id"] != null)
                    {
                        int postID = Convert.ToInt32(col["Id"].ToString());
                        if (postID != 0)
                        {
                            p = ctx.Set<Post>().FirstOrDefault(d => d.Id == postID);


                            ctx.Set<CategoryPost>().RemoveRange(p.PostCategories);
                            ctx.Set<PostPicture>().RemoveRange(p.PostPictures);
                            ctx.Set<PostTag>().RemoveRange(p.PostTags);
                            ctx.Set<PostLanguage>().RemoveRange(p.PostLanguages);



                            //p.PostCategories.Clear();
                            //p.PostPictures.Clear();
                            //p.PostTags.Clear();
                            //p.PostLanguages.Clear();
                            ctx.SaveChanges();

                        }
                    }
                   

                        p.Title = title;
                        p.Description = desc;
                        p.CreationTime = Convert.ToDateTime(col["date"] == "" ? DateTime.Now.ToString() : col["date"]);
                        p.UpdateTime = DateTime.Now;
                        p.PostLanguages.Add(new PostLanguage { LanguageId = lan.Id, Post = p });

                        int[] categoryIds = col["catIds"].ToString().Split(',').Select(d => Convert.ToInt32(d)).ToArray();
                        string[] tags = col["catIds"].ToString().Split(',').Select(d => d.ToString()).ToArray();
                        string[] images = col["images"].ToString().Split(',').Select(d => d.ToString()).ToArray();

                        if (p.Id <= 0)
                        {

                            ctx.Set<Post>().Add(p);
                        }
                        else
                        {
                            ctx.SaveChanges();
                        }
                        foreach (var item in categoryIds)
                        {
                            p.PostCategories.Add(new EClassField.Core.Domain.Blog.CategoryPost { CategoryId = item, PostId = p.Id });
                        }
                        foreach (var item in tags)
                        {
                            if (!p.PostTags.Any(d => d.Tag.Name == item))
                            {

                                p.PostTags.Add(new PostTag { PostId = p.Id, Tag = new Tag { Name = item } });

                            }



                        }
                        foreach (var item in images)
                        {

                            if (!p.PostPictures.Any(d => d.Picture.FileName == item))
                            {
                                p.PostPictures.Add(new PostPicture { PostId = p.Id, Picture = new EClassField.Core.Domain.Media.Picture { FileName = item } });
                            }

                        }
                        ctx.SaveChanges();
                    
                    return Json("T", JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json("F", JsonRequestBehavior.AllowGet);

                }
            }
        }




        public ActionResult List(int pageID = 1)

        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {
            
                var posts = ctx.Set<Post>();

                PageModel pagemodel = new PageModel();
                pagemodel.TotalCount = posts.Count();
                pagemodel.PageSize = 20;
                pagemodel.PageIndex = pageID;

                ViewBag.PageModel = pagemodel;
                var res=posts.OrderByDescending(d=>d.CreationTime).Skip(((pageID - 1) * 20)).Take(20).ToList(). Select(d => new PostViewModel
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
                    Tags = d.PostTags.Select(t => new Input { Value = t.Tag.Name }).ToList(),
                    Categories = d.PostCategories.Select(p => new Input { Text = p.Category.Name, Value = p.Category.Id + "" }).ToList()

                }).ToList();


                return View(res);
            }

        }

        public ActionResult Delete(int postID=0)
        {
            Post p = postservice.GetById(d => d.Id == postID);

            postservice.Delete(p);

            return RedirectToAction("List", new { pageID = 1 });
        }

        public ActionResult Active(int postID = 0)
        {
            Post p = postservice.GetById(d => d.Id == postID);

            p.IsActive = p.IsActive ? false : true;
            postservice.Update(p);

            return RedirectToAction("List", new { pageID = 1 });
        }
    }
}