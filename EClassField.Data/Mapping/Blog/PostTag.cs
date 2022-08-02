using EClassField.Core.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Blog
{
    public class PostTagMap:EntityTypeConfiguration<PostTag>
    {
        public PostTagMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Post).WithMany(d => d.PostTags).HasForeignKey(d => d.PostId);
            this.HasRequired(d => d.Tag).WithMany().HasForeignKey(d => d.TagId);
            this.Map(d => d.ToTable("Post_Tag_Mapping"));
        }
    }
}
