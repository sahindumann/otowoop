using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Galerry
{
    public class Slider : BaseEntity<int>
    {
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public bool IsAktif { get; set; }
    }
}
