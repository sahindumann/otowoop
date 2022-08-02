using EClassField.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Blog
{
   public class PostPicture:BaseEntity<int>
    {
        public int PostId { get; set; }
        public int PictureId { get; set; }
        

        public virtual Post Post { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
