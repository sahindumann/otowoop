using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class CategorySubAttributeMap:EntityTypeConfiguration<CategorySubAttribute>
    {

        public CategorySubAttributeMap()
        {
          
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.M_Attribute).WithMany().HasForeignKey(d => d.AttributeId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.SubAttribute).WithMany().HasForeignKey(d => d.SubAttributeId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Category).WithMany(d => d.Sub_SubAttributes).HasForeignKey(d => d.CategoryId);

            this.Map(d => d.ToTable("Category_Attribute_SubAttribute_Mapping"));
            
        }
    }
}
