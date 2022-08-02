using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class ProductDescriptionMap: EntityTypeConfiguration<ProductDescription>
    {
        public ProductDescriptionMap()
        {
            HasKey(x => x.Id);
            ToTable("ProductDescription");
        }

    }
}
