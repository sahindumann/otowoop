using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
  public  class ProductDescription:BaseEntity<int>
    {

        public long ProductId { get; set; }
        public string Description { get; set; }

    }
}
