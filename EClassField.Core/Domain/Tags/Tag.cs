using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Tags
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
       
    }
}
