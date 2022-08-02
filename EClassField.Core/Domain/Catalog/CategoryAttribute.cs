using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EClassField.Core.Domain.Attribute;
namespace EClassField.Core.Domain.Catalog
{
    public class CategoryAttribute:BaseEntity<int>
    {
        public int AttributeId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual M_Attribute Attribute { get; set; }
    }
}
