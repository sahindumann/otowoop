using EClassField.Core.Domain.Attribute;
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Media;
using EClassField.Services.Catalog;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class CategoryController : Controller
    {
        CategoryService service = new CategoryService();
        LanguageService languageservice = new LanguageService();
        CategoryAttributeService categoryattrservice = new CategoryAttributeService();
        CategorySubAttributeService categorysubattrservice = new CategorySubAttributeService();


        [HttpGet]

        // GET: Areas/Category
        public ActionResult Index(int pageID = 1, string categoryName = "", int parentID = 0)
        {
            var query = service.GetTable();
            if (!string.IsNullOrEmpty(categoryName))
            {
                categoryName = categoryName.ToLower().Replace(" ", "").Replace("-", "");
                query = query.Where(d => d.Name.Contains(categoryName));
            }
            query = query.Where(d => d.ParentCategoryId == parentID);



            //query = query.OrderBy(d => d.Id).Skip((pageID - 1) * 20).Take(20);

            ViewBag.PageTitle = "Kategoriler";
            ViewBag.pageID = pageID;
            ViewBag.categoryName = categoryName;
            ViewBag.parentID = parentID;
            return View(query);
        }

        public ActionResult CategoryAdd(int catID = 0, int parentID = 0, int AttributeID = 0, int subAttributeID = 0, string secili = "category")
        {
            Category category = new Category();
            if (catID <= 0)
            {
                category = new Category();


            }
            else if (catID >= 1)
            {

                ViewBag.parentID = catID;
                category = service.GetById(d => d.Id == catID);
            }
            if (parentID >= 1)
            {
                category = new Category();
                category.ParentCategoryId = parentID;
                category.IsActive = true;
                ViewBag.parentID = parentID;
            }


            ViewBag.catID = catID;

            if (category.GetType().GetInterface("ILocazition") != null)
                ViewBag.islocalize = true;
            else
                ViewBag.islocalize = false;

            ViewBag.languages = languageservice.GetTable().ToList();

            var cats = Function.GetCategoryPath(category.Id).Select(d => d.Id).ToList();


            var attrs = categoryattrservice.GetTable().Distinct().Where(d => cats.Contains(d.CategoryId));
            category.SubAttributes.Clear();
            category.SubAttributes = attrs.ToList();
            if (AttributeID > 0)
            {
                var attribute = categoryattrservice.GetById(d => d.AttributeId == AttributeID);
                ViewBag.subattributes = categorysubattrservice.GetList(d => d.AttributeId == attribute.AttributeId && cats.Contains(d.CategoryId));
                ViewBag.attribute = attribute.Attribute;
                ViewBag.parentID = attribute.CategoryId;


                ViewBag.attributeID = attribute.Id;
                if (ViewBag.catID == null)
                    ViewBag.catID = attribute.CategoryId;



            }
            if (subAttributeID >= 1)
            {

                ViewBag.subattribute = categorysubattrservice.GetById(d => d.Id == subAttributeID);
            }


            if (secili == "subattribute")
                ViewBag.subattributedetay = "active";
            else if (secili == "attribute")
                ViewBag.attributedetay = "active";
            else
                ViewBag.catdetay = "active";


            //Cache.LoadCategories(new EClassField.Data.ClassFieldDbContext());
            return View(category);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CategoryAdd(Category catt)
        {
            using (EClassField.Data.ClassFieldDbContext ctx = new EClassField.Data.ClassFieldDbContext())
            {
                Category category = new Category();



                var cat = ctx.Set<Category>().FirstOrDefault(d => d.Id == catt.Id) ?? new Category();


                UpdateModel(cat);

                if (cat != null && cat.Id > 0)
                {


                }
                else
                {
                    cat.CreationTime = DateTime.Now;


                }


                ViewBag.parentID = cat.ParentCategoryId;
                ViewBag.catID = cat.Id;
                ViewBag.catdetay = "active";

                var files = Request.Files;

               

                for (int i = 0; i < Request.Files.Count; i++)
                {



                    var item = Request.Files[i];

                    if (item.FileName == "")
                        continue;


                    if (cat.Picture != null && System.IO.File.Exists((Server.MapPath("~/Content/categoryimages/" + cat.Picture.FileName))))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/categoryimages/" + cat.Picture.FileName));
                    }
                    string filename = Guid.NewGuid().ToString().Substring(0, 5) + "_" + item.FileName;

                 
                        if (cat.Picture != null)
                        {
                            cat.Picture.FileName = filename;

                        }
                        else
                        {
                        Picture pic = new Picture();
                        pic.FileName = filename;
                        ctx.Set<Picture>().Add(pic);
                        cat.Picture = pic;
                        
                        


                    }


                    item.SaveAs(Server.MapPath("~/Content/categoryimages/"+filename));




                }
                if (cat.Id == 0)
                {
                    ctx.Set<Category>().Add(cat);
                }
                ctx.SaveChanges();
                return RedirectToAction("CategoryAdd", "Category", new { catID = cat.Id }); 
            }
        }

        [HttpPost]
        public ActionResult Attribute(M_Attribute id, int catID)
        {
            var attr = categoryattrservice.GetById(d => d.Id == id.Id);
            if (attr != null)
            {
                attr.Attribute.Name = id.Name;
                attr.Attribute.AttributeType = id.AttributeType;

                categoryattrservice.Update();
            }
            else
            {
                var attrr = categoryattrservice.GetById(d => d.Attribute.Name == id.Name);
                if (attrr != null)
                {
                    categoryattrservice.Add(new CategoryAttribute { AttributeId = attrr.AttributeId, CategoryId = catID });
                }
                else
                {
                    categoryattrservice.Add(new CategoryAttribute { Attribute = id, CategoryId = catID });
                }


                ViewBag.attrbutedetay = "active";

                return Redirect("/Panel/Category/CategoryAdd/?catID=" + catID + "&secili=attribute");

            }


            return Redirect("/Panel/Category/CategoryAdd/?AttributeID=" + id.Id + "&catID=" + catID + "&secili=attribute");

        }

        [HttpPost]
        public ActionResult SubAttribute(SubAttribute id, int catID, int attributeID)
        {
            var attr = categorysubattrservice.GetById(d => d.SubAttributeId == id.Id);
            if (attr != null)
            {
                attr.SubAttribute.Value = id.Value;
                attr.CategoryId = catID;

                attr.AttributeId = attributeID;
                attr.SubAttributeId = id.Id;


                categorysubattrservice.Update();
            }
            else
            {
                var subattr = categorysubattrservice.GetById(d => d.SubAttribute.Value == id.Value);
                if (subattr != null)
                {
                    categorysubattrservice.Add(new CategorySubAttribute { CategoryId = catID, AttributeId = attributeID, SubAttributeId = subattr.SubAttributeId });
                }
                else
                {
                    categorysubattrservice.Add(new CategorySubAttribute { CategoryId = catID, AttributeId = attributeID, SubAttribute = new EClassField.Core.Domain.Attribute.SubAttribute { Value = id.Value } });
                }

            }


            return Redirect("/Panel/Category/CategoryAdd/?AttributeID=" + attributeID + "&catID=" + catID + "&subAttributeID=" + id.Id + "&secili=subattribute");

        }




        [HttpPost]
        public JsonResult GetCategoriesBlog(int Id, string name)
        {
            Category cat = new Category();
            if (!string.IsNullOrEmpty(name))
            {
                cat = service.Add(new Category { ParentCategoryId = Id, Name = name, CreationTime = DateTime.Now, IsBlog = true });
            }

            return Json(new { Id = cat.Id, name = cat.Name }, JsonRequestBehavior.AllowGet);

        }
    }
}