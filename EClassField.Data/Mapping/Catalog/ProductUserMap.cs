using EClassField.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
   public class ProductUserMap:EntityTypeConfiguration<ProductUser>
    {
        public ProductUserMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.User).WithMany(d => d.UserProducts).HasForeignKey(d => d.UserId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
           
        }
    }
}
