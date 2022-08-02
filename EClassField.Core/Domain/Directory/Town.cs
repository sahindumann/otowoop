using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Directory
{
   public class Town:BaseEntity<int>
    {
     
        public int CityId { get; set; }
        public string Name { get; set; }
        public string TownName { get; set; }
        public int TownId { get; set; }
        public DbGeography Location { get; set; }
    }
}
