using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassField.API.Models
{
    public class Cache
    {
        public static List<Category> Categories { get; set; }
        public static List<CategoryAttribute> CatAttribute { get; set; }

        public static List<CategorySubAttribute> CatSubAttribute { get; set; }

        public static List<int> SelectedCategories = new List<int>{2,20485};
        public static List<Category> IsDoluCategory = new List<Category>();
        public static void Load(ClassFieldDbContext ctx)
        {
            
      

            Categories = ctx.Set<Category>().Where(d =>d.IsOtoKuafor ||d.IsIlan).ToList();
            //var ids = Categories.Select(x => x.Id).ToList();
            //var iscats = ctx.Set<ProductCategory>().Where(x => ids.Contains(x.CategoryId)).Select(x=>x.CategoryId).ToList();
            //IsDoluCategory = Categories.FindAll(x => iscats.Contains(x.Id));
            CatAttribute = ctx.Set<CategoryAttribute>().ToList();
            CatSubAttribute = ctx.Set<CategorySubAttribute>().ToList();

        }
    }
}