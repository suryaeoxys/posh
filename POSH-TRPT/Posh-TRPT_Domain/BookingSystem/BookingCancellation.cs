using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.BookingSystem
{
    public class BookingCancellation
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public Guid BookingId { get; set; }
        public string? LocalTime { get; set; }
    }
}
