using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
    public class ProductAttribute : BaseEntity<int>
    {
        public int AttributeId { get; set; }
        public int ProductId { get; set; }

        public int SubAttributeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual M_Attribute Attribute { get; set; }
        public virtual SubAttribute SubAttribute { get; set; }
    }
}
