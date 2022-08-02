using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class ProductPictureMap:EntityTypeConfiguration<ProductPicture>
    {


        public ProductPictureMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Picture).WithMany().HasForeignKey(d => d.PictureId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Product).WithMany(d => d.ProductPictures).HasForeignKey(d => d.ProductId).WillCascadeOnDelete(true);
            this.Map(d => d.ToTable("Product_Picture_Mapping"));

        }
    }
}
