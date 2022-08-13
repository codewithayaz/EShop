using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Models
{
    public class PaginationFilter
    {
        public object Predicate { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Search { get; set; }

        public string SortLabel { get; set; }

        public int SortDirection { get; set; }

        public PaginationFilter()
        {
            this.PageNumber = 0;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 0 ? 0 : pageNumber;
            this.PageSize = pageSize > 100 ? 10 : pageSize;
        }

        public PaginationFilter(string search)
        {
            this.PageNumber = 0;
            this.PageSize = 20;
            this.Search = search;
        }

        public Dictionary<string, string> SearchColumnList { get; set; }
    }
}
