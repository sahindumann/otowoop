using EClassField.Core.Domain.Catalog;
using EClassField.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Services.Catalog
{
   public class CategoryService:Repository<Category>
    {

        public List<Category> GetSubCategories(int catID=0)
        {
           
            List<Category> cats = new List<Category>();

            Category cat = GetById(d => d.Id == catID);
            if (cat != null)
            {
                cats.Add(cat);

                while (cat!=null)
                {
                    cat = GetById(d => d.Id == cat.ParentCategoryId);
                    if (cat != null)
                        cats.Add(cat);
                }
            }
            cats = cats.OrderBy(d => d.Id).ToList();

            return cats;
        }
    }
}
