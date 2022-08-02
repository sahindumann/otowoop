﻿using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Blog
{
    public class CategoryPost:BaseEntity<int>
    {
        public int CategoryId { get; set; }
        public int PostId { get; set; }
      
        public virtual Category Category { get; set; }

        public virtual Post Post { get; set; }

    }
}
