using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
    public class ProductAttributeValueMap:EntityTypeConfiguration<ProductAttributeValue>
    {
        public ProductAttributeValueMap()
        {

            this.HasKey(d => d.Id);
            this.HasRequired(d => d.M_Attribute).WithMany().HasForeignKey(d => d.AttributeId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.ToTable("ProductAttributeValueMap");
        }
    }


    public class ProductAttributeValueNumberMap : EntityTypeConfiguration<ProductAttributeValueNumber>
    {
        public ProductAttributeValueNumberMap()
        {

            this.HasKey(d => d.Id);
            this.HasRequired(d => d.M_Attribute).WithMany().HasForeignKey(d => d.AttributeId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.ToTable("ProductAttributeValueNumberMap");
        }
    }
}
