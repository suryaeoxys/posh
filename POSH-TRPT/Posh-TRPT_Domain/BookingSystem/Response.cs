using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T data)
        {
            this.Successed = true;
            this.Data = data;
            this.Errors = null!;
            this.Message = string.Empty;

        }
        public T? Data { get; set; }
        public bool Successed { get; set; }
        public string? Message { get; set; }
        public string[]? Errors { get; set; }

    }
}
