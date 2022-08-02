using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Attribute
{
    public class SubAttribute : BaseEntity<int>
    {
        public string Value { get; set; }
        public decimal ValueNumber { get; set; }


    }
}
