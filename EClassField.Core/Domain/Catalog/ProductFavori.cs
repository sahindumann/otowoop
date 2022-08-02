using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
    public class ProductFavori : BaseEntity<int>
    {
   
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public virtual Product Product { get; set; }
        public virtual User.User User { get; set; }
      
    }
}
