using EClassField.Core.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Blog
{
   public class PostCategoryMap:EntityTypeConfiguration<CategoryPost>
    {
        public PostCategoryMap()
        {
            this.HasKey(d => d.Id);

            this.HasRequired(d => d.Post).WithMany(d => d.PostCategories);
            this.HasRequired(d => d.Category).WithMany().HasForeignKey(d => d.CategoryId);
            this.ToTable("PostCategories");

        }
    }
}
