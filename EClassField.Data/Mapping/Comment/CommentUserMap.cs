using EClassField.Core.Domain.Comment;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Data.Mapping.Comment
{
    public class CommentUserMap : EntityTypeConfiguration<CommentUser>
    {
        public CommentUserMap()
        {
            this.HasKey(d=>d.Id);
            this.HasRequired(d => d.Comment).WithMany().HasForeignKey(d => d.CommentID);
            this.HasRequired(d => d.User).WithMany().HasForeignKey(d => d.UserID);
        }
    }
}
