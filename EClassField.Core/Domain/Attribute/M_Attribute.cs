using EClassField.Core.Domain.Locazition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Attribute
{
    public class M_Attribute : BaseEntity<int>, ILocazition
    {
       

        public string Name { get; set; }
        public AttributeType AttributeType { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string AttributeEndText { get; set; }

    }
}
