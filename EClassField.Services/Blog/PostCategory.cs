using EClassField.Core;
using EClassField.Core.Domain.Blog;
using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Services.Blog
{
    public class CategoryPostt : BaseEntity<int>
    {
        public int CategoryId { get; set; }
        public int PostId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Post Post { get; set; }
    }
}
