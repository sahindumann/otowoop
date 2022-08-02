using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public class PaginatorModel
    {
        public int PageID { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string search { get; set; }
        public string orderby { get; set; }
        public string ordertype { get; set; }
    }
}
