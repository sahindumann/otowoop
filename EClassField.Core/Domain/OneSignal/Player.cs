using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.OneSignal
{
    public class Player:BaseEntity<int>
    {
     
        public string PlayerID { get; set; }
        public string DeviceID { get; set; }
        public string DevicModel { get; set; }

        public bool IsActive { get; set; }
    }
}
