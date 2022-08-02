using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.OneSignal
{
 public   class PlayerUser:BaseEntity<int>
    {
        public int PlayerID{ get; set; }
        public int UserID { get; set; }

        public Player Player { get; set; }
        public User.User User { get; set; }
    }
}
