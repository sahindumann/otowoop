using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Locazition
{
   public class LocaleString:BaseEntity<int>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual Language Language { get; set; }
    }
}
