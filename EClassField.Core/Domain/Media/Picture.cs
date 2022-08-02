using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core.Domain.Media
{
    public class Picture : BaseEntity<int>
    {
        public byte[] ByteImage { get; set; }
        public string FileName { get; set; }
        public string SeoName { get; set; }
        public string Exist { get; set; }
        public string MimeType { get; set; }
        public bool IsVitrin { get; set; }
    }
}
