using EClassField.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
    public class ProductPicture : BaseEntity<int>
    {
        public int PictureId { get; set; }
        public int ProductId { get; set; }

        public virtual Picture Picture { get; set; }
        public virtual Product Product { get; set; }
    }
}
