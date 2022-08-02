using EClassField.Core.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class CategoryPostMap
    {
        public CategoryPostMap()
        {
            //this.HasKey(d => d.Id);
            //this.HasRequired(d => d.Category).WithMany(d => d.CategoryPost).HasForeignKey(d=>d.CategoryId);
            //this.HasRequired(d => d.Post).WithMany().HasForeignKey(d => d.PostId);

        }
    }
}
