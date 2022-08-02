using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
namespace EClassField.Core.Domain.Directory
{
    public class City : BaseEntity<int>
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string PlateNo { get; set; }
        public string PhoneCode { get; set; }
        public DbGeography Location { get; set; }
    }
}
