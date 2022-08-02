using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
    public class ProductAttributeMap:EntityTypeConfiguration<ProductAttribute>
    {
        public ProductAttributeMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.HasRequired(d => d.Attribute).WithMany().HasForeignKey(d => d.AttributeId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.SubAttribute).WithMany().HasForeignKey(d => d.SubAttributeId).WillCascadeOnDelete(true);
            this.Map(d => d.ToTable("ProductAttributeMapping"));
        }
    }
}
