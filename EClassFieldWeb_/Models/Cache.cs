
using EClassField.Core.Domain.Catalog;
using EClassField.Core.Domain.Directory;
using EClassField.Core.Domain.Galerry;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EClassFieldWeb_.Models
{
    public class Cache
    {
        public static List<Category> Categories { get; set; }
        public static List<CategoryAttribute> CatAttribute { get; set; }

        public static List<CategorySubAttribute> CatSubAttribute { get; set; }

        public static List<City> Citites { get; set; }
        public static List<Town> Towns { get; set; }
        public static List<Area> Areas { get; set; }
        public static List<Neighborhood> Neighborhoods { get; set; }

        public static Slider Slider;
        public static void Load(ClassFieldDbContext ctx)
        {
            List<int> ids = new List<int>();
            ids.Add(-1);
            ids.Add(20455);
            ids.Add(20451);
            ids.Add(20446);

            CatAttribute = ctx.Set<CategoryAttribute>().ToList();
            CatSubAttribute = ctx.Set<CategorySubAttribute>().ToList();
            Citites = ctx.Set<City>().ToList();
            Towns = ctx.Set<Town>().ToList();
            Areas = ctx.Set<Area>().ToList();
            Neighborhoods = ctx.Set<Neighborhood>().ToList();

            LoadCategories(ctx);



            Categories = ctx.Set<Category>().Where(d => !d.Deleted).ToList();
            Slider = ctx.Set<Slider>().FirstOrDefault(d => d.IsAktif);
        }

        public static void LoadCategories(ClassFieldDbContext ctx)
        {
            Categories = ctx.Set<Category>().ToList();


            foreach (var item in Categories)
            {

                item.FullPath = Function.getCategoriesPathString(item.Id);

            }

            ctx.SaveChanges();



        }
    }
}