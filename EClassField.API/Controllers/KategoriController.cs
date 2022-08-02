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
    public class KategoriController : ApiController
    {
        public IHttpActionResult Get(int ParentID = 0, bool isilan = true, bool isFun = false)
        {
            using (ClassFieldDbContext ctx = new ClassFieldDbContext())
            {

                Category cat = Cache.Categories.FirstOrDefault(d => d.Id == ParentID);
                var yols = Function.getKategoriYol(ParentID).Select(d => new { Name = d.Name, Value = d.Id }).OrderBy(d => d.Value).ToList();

                dynamic cats = null;
                if (isilan)
                {
                    cats = Cache.Categories.Where(d => d.ParentCategoryId == ParentID && d.IsIlan).ToList().Select(d => new { Name = d.Name, Value = d.Id, CatType = GetCategoryType(d), AllIlan = "false" }).ToList();
                    if (cat != null)
                    {
                        string name = cat.Name;
                        int id = cat.Id;
                        cats.Insert(0, new { Name = "Tüm " + name + " İlanları", Value = id, CatType = GetCategoryType(cat), AllIlan = "true" });
                    }
                }
                else if (isFun)
                {
                    cats = Cache.Categories.Where(d => d.ParentCategoryId == ParentID && d.IsFun).ToList().Select(d => new { Name = d.Name, Value = d.Id, CatType = GetCategoryType(d), AllIlan = "false", Image = d.Picture != null ? d.Picture.FileName : "" }).ToList();
                }
                else
                {
                    cats = Cache.Categories.Where(d => d.ParentCategoryId == ParentID && (d.IsIlan || d.IsBlog) && d.IsActive).ToList().Select(d => new { Name = d.Name, Value = d.Id, CatType = GetCategoryType(d), AllIlan = "false" }).ToList();
                }

                var kats = new { Katyol = yols, Kats = cats };




                return Ok(kats);
            }



        }
        public static string GetCategoryType(Category cat)

        {

            if (cat.IsBlog)
                return "blog";
            else if (cat.IsIlan)
                return "ilan";
            else if (cat.IsForum)
                return "forum";
            else if (cat.IsFun)
                return "fun";



            return "ilan";

        }

    }
}
