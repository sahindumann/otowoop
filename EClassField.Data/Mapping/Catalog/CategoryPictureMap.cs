using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Catalog
{
    public class CategoryPictureMap: EntityTypeConfiguration<CategoryPicture>
    {
        public CategoryPictureMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Picture).WithMany().HasForeignKey(d => d.PictureId).WillCascadeOnDelete(true);
            this.HasRequired(d => d.Category).WithMany().HasForeignKey(d => d.CategoryId);
            this.ToTable("CategoryPictureMap");
        }
    }
}
