using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class PagedResponse<T> : Response<T>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Message = string.Empty;
            this.Errors = null!;
            this.TotalRecords = 0;
            this.TotalPages = 0;
            this.Successed = true;

        }
    }
}
