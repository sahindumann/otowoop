using EClassField.Core.Domain.Comment;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Comment
{
    class CommentProductMap:EntityTypeConfiguration<ProductComment>
    {
        public CommentProductMap()
        {
            this.HasKey(D => D.Id);
      
            this.HasRequired(d => d.Product).WithMany().HasForeignKey(d => d.ProductId);
            this.HasRequired(d => d.Comment).WithMany().HasForeignKey(d => d.CommentID).WillCascadeOnDelete(true);
            this.HasRequired(d => d.User).WithMany().HasForeignKey(d => d.UserID).WillCascadeOnDelete(true);
        }
    }
}
