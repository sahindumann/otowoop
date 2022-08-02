
using EClassField.Core.Domain.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
    public class ProductAttributeValue : BaseEntity<int>
    {
        public int AttributeId { get; set; }
        public int ProductId { get; set; }
        public string Value { get; set; }

        public virtual M_Attribute M_Attribute { get; set; }
        public virtual Product Product { get; set; }

    }

    public class ProductAttributeValueNumber : BaseEntity<int>
    {
        public int AttributeId { get; set; }
        public int ProductId { get; set; }
        public double Value { get; set; }

        public virtual M_Attribute M_Attribute { get; set; }
        public virtual Product Product { get; set; }

    }
}
