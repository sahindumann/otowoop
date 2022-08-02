using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public interface IPageModel
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int ShowPageCount { get; set; }
        int TotalPages { get; }
        int TotalCount { get; set; }

        bool HasNextPage { get; }

        int Start { get; }
        int End { get; }
    }
}
