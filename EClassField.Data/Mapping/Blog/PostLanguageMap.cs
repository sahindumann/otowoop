using EClassField.Core.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Blog
{
   public class PostLanguageMap:EntityTypeConfiguration<PostLanguage>
    {
        public PostLanguageMap()
        {
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Post).WithMany(d => d.PostLanguages).HasForeignKey(d => d.PostId);
            this.HasRequired(d => d.Language).WithMany().HasForeignKey(d => d.LanguageId);
        }
    }
}
