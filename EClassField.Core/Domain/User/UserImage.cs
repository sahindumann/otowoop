using EClassField.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.User
{
    public class UserImage:BaseEntity<int>
    {
        public virtual Picture  Picture { get; set; }
        public virtual User User { get; set; }
        public int PictureId { get; set; }
        public int UserId { get; set; }



    }
}
