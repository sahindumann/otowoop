using EClassField.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Catalog
{
   public class CategoryPicture:BaseEntity<int>
    {
        public int CategoryId { get; set; }
        public int PictureId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
