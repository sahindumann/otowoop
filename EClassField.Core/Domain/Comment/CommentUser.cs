using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Comment
{
    public class CommentUser : BaseEntity<int>
    {
        public int UserID { get; set; }
        public int CommentID { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User.User User { get; set; }
    }
}
