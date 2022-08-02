using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Directory
{
   public class Country:BaseEntity<int>
    {
        public string Name { get; set; }
        public DbGeography Location { get; set; }
    }
}
