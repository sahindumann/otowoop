using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class CategoryProductMap : EntityTypeConfiguration<CategoryProduct>
    {
        public CategoryProductMap()
        {
     
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Category).WithMany(d=>d.CategoryProducts).HasForeignKey(d => d.CategoryId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.Map(d => d.ToTable("Category_Products_Map"));
          

        }
    }
}
