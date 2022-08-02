using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Directory
{
   public class Neighborhood:BaseEntity<int>
    {


  
        public int AreaID { get; set; }
        public string Name { get; set; }
        public string ILADI { get; set; }
        public string TownName { get; set; }
        public string AreaName { get; set; }
        public string Tıp { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }

        public int NeigborhoodId { get; set; }
        public string ZipCode { get; set; }
        public DbGeography Location { get; set; }
    }
}
