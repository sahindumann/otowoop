using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Comment
{
    public class ProductComment : BaseEntity<long>
    {
        public int CommentID { get; set; }
    
        public int UserID { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual User.User User { get; set; }

    }
}
