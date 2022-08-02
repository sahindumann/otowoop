using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClassField.Core
{
    public class PageModel:IPageModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ShowPageCount { get; set; }
        public int TotalPages
        {
            get
            {
                var toplam = Convert.ToDouble(Convert.ToDouble(TotalCount) / PageSize);
                if (toplam % 2 != 0)
                    toplam++;

                return Convert.ToInt32(Math.Floor(toplam));
            }
            set
            {
                TotalPages = value;
            }
        }
        public int TotalCount { get; set; }
        public bool HasPreviousPage
        {
            get
            {
                if (PageIndex > 1 &&TotalPages>1)
                {
                    return true;
                }
                return false;

            }
        }
        public bool HasNextPage
        {
            get
            {
                if (PageIndex + 1 <= TotalPages)
                {
                    return true;
                }
                return false;

            }
        }

        public int Start
        {
            get

            {

                if (PageIndex > ShowPageCount)
                {
                    int total = TotalPages - PageIndex;
                    if (total >= ShowPageCount)
                        return PageIndex - 2;
                    else if (total < ShowPageCount)
                        return PageIndex - ShowPageCount;

                    return 0;
                }
                return 0;


            }

        }
        public int End
        {

            get
            {
                return (Start + ShowPageCount + 1) >= TotalPages ? TotalPages : (Start + ShowPageCount + 1);
            }

        }
    }
}
