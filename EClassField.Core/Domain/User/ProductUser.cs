using EClassField.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.User
{
    public class ProductUser:BaseEntity<int>
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public virtual  Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
