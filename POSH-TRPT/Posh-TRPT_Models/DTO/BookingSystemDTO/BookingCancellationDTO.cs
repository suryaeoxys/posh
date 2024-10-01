using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.BookingSystemDTO
{
    public  class BookingCancellationDTO
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public Guid BookingId { get; set; }
        public string? LocalTime { get; set; }
    }
}
