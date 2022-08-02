using EClassField.Core.Domain.Attribute;
using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using EClassFieldWeb_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EClassFieldWeb_.Areas.Areas.Controllers
{
    [AdminFilter]
    public class MAttributeController : Controller
    {
        // GET: Areas/MAttribute
        public ActionResult Index(int categoryID = 0, int attributeID = -1)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (attributeID != -1)
                {
                    var attr = ctx.Set<M_Attribute>().Where(d => d.Id == attributeID).FirstOrDefault();
                    if (attr != null)
                        ViewBag.attr = attr;

                }
                var catattribute = ctx.Set<CategoryAttribute>().Where(d => d.CategoryId == categoryID);

                ViewBag.categoryID = categoryID;
                ViewBag.attributes = catattribute.Select(d => new Input { Text = d.Attribute.Name, Value = d.AttributeId + "", Icon = d.Category.Name }).ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(M_Attribute catattr, int categoryID = -1)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (catattr.Id > 0)
                {
                    var attr = ctx.Set<M_Attribute>().FirstOrDefault(d => d.Id == catattr.Id);

                    if (attr != null)
                    {
                        attr.Name = catattr.Name;
                        attr.AttributeType =(AttributeType) catattr.AttributeType;
                        attr.Icon = catattr.Icon;
               
                        ctx.SaveChanges();

                        return RedirectToAction("/Index", new { categoryID = categoryID });
                    }
                }

                else
                {
                    M_Attribute mattr = new M_Attribute();
                    mattr.Name = catattr.Name;
                    mattr.Icon = catattr.Icon;
                    mattr.AttributeType = catattr.AttributeType;

                    ctx.Set<M_Attribute>().Add(mattr);
                    ctx.SaveChanges();

                    CategoryAttribute cattr = new CategoryAttribute();
                    cattr.Attribute = mattr;
                    cattr.CategoryId = categoryID;


                    ctx.Set<CategoryAttribute>().Add(cattr);
                    ctx.SaveChanges();
                }

            }

            return RedirectToAction("/Index", new { categoryID = categoryID });
        }

        public ActionResult Delete(int AttributeID, int categoryID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var attribute = ctx.Set<CategoryAttribute>().FirstOrDefault(d => d.AttributeId == AttributeID &&d.CategoryId==categoryID);

                if (attribute != null)
                {

                    ctx.Set<M_Attribute>().Remove(attribute.Attribute);
           
                 
                    ctx.SaveChanges();


                }
            }
            return RedirectToAction("/Index", new { categoryID = categoryID });
        }



        public ActionResult SubIndex(int attributeID = 0, int categoryID = -1, int subAttributeId = -1)
        {

            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (subAttributeId != -1)
                {
                    var subattr = ctx.Set<SubAttribute>().FirstOrDefault(d => d.Id == subAttributeId);
                    if (subattr != null)
                        ViewBag.attr = subattr;
                }

                var catattribute = ctx.Set<CategorySubAttribute>().Where(d => d.AttributeId == attributeID && d.CategoryId == categoryID);
                ViewBag.categoryID = categoryID;
                ViewBag.attributeID = attributeID;
                ViewBag.attributes = catattribute.Select(d => new Input { Text = d.SubAttribute.Value ?? d.SubAttribute.ValueNumber + "", Value = d.SubAttributeId + "", Icon = d.Category.Name + " / " + d.M_Attribute.Name }).ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult SubIndex(SubAttribute catattr, int categoryID = -1, int attributeID = -1)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                if (catattr.Id > 0)
                {
                    var attr = ctx.Set<SubAttribute>().FirstOrDefault(d => d.Id == catattr.Id);

                    if (attr != null)
                    {
                        attr.Value = catattr.Value;

                        ctx.SaveChanges();

                        return RedirectToAction("/SubIndex", new { categoryID = categoryID, attributeID = attributeID });
                    }
                }
                else
                {
                    SubAttribute subattr = new SubAttribute();
                    subattr.Value = catattr.Value;
                    ctx.Set<SubAttribute>().Add(subattr);
                    ctx.SaveChanges();

                    CategorySubAttribute catsubattr = new CategorySubAttribute();
                    catsubattr.SubAttributeId = subattr.Id;
                    catsubattr.AttributeId = attributeID;
                    catsubattr.CategoryId = categoryID;
                    ctx.Set<CategorySubAttribute>().Add(catsubattr);
                    ctx.SaveChanges();



                }

            }

            return RedirectToAction("/SubIndex", new { categoryID = categoryID, attributeID = attributeID });
        }

        public ActionResult SubDelete(int SubAttributeID,int AttributeID, int categoryID)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                var attribute = ctx.Set<CategorySubAttribute>().FirstOrDefault(d => d.SubAttributeId == SubAttributeID);

                if (attribute != null)
                {
                    ctx.Set<CategorySubAttribute>().Remove(attribute);
                    ctx.SaveChanges();


                }
            }
            return RedirectToAction("/SubIndex", new { categoryID = categoryID, attributeID=AttributeID });
        }
    }
}