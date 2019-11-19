using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core
{
    public class Page<T> where T : class
    {
        public int Total { get; set; }

        public int PageSize { get; set; }

        public List<T> List { get; set; }

        public Page(List<T> list, int pageSize
            , int total)
        {
            this.List = list;
            this.PageSize = pageSize;
            this.Total = total;
        }

        public int GetPageCount()
        {
            return (Total + PageSize - 1) / PageSize;
        }
    }
}
