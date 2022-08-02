using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
   public class CategorySubAttribute:BaseEntity<int>
    {
        public int SubAttributeId { get; set; }
        public int AttributeId { get; set; }
        public int CategoryId { get; set; }

        public virtual SubAttribute SubAttribute { get; set; }
        public virtual M_Attribute M_Attribute { get; set; }
        public virtual Category Category { get; set; }
    }
}
