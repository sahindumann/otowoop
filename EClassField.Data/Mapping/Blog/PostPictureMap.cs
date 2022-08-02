using EClassField.Core.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Blog
{
 public   class PostPictureMap:EntityTypeConfiguration<PostPicture>
    {
        public PostPictureMap()
        {
            this.HasKey(d => d.Id);

            this.HasRequired(d => d.Post).WithMany(d => d.PostPictures).HasForeignKey(d => d.PostId);
            this.HasRequired(d => d.Picture).WithMany().HasForeignKey(d => d.PictureId);
            this.Map(d => d.ToTable("Post_Picture_Mapping"));
        }
    }
}
