using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
namespace EClassField.Core.Domain.Directory
{
   public class Area:BaseEntity<int>
    {
     
        public int TownId { get; set; }
        public int CityId { get; set; }
        public string   Name { get; set; }
        public string TownName { get; set; }
        public string CityName { get; set; }

        public DbGeography Location { get; set; }
    }
}
