using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
    public class ProductFavMap:EntityTypeConfiguration<ProductFavori>
    {
        public ProductFavMap()
        {
            this.HasKey(d => d.Id);
           
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.HasRequired(d => d.User).WithMany().HasForeignKey(d => d.UserId);
            this.Map(d => d.ToTable("Product_Fav_Mappping"));

        }
    }
}
